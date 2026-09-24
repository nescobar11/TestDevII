using AsisyaApi.Application.DTOs.Common;
using AsisyaApi.Application.DTOs.Products;
using AsisyaApi.Application.Interfaces;
using AsisyaApi.Application.Mappings;
using AsisyaApi.Domain.Entities;
using Bogus;
using Microsoft.Extensions.DependencyInjection;

namespace AsisyaApi.Application.Services;

public sealed class ProductService(
    IProductRepository productRepository,
    ICategoryRepository categoryRepository,
    IBackgroundJobQueue jobQueue,
    IProductBulkJobStore jobStore) : IProductService
{
    private const int DefaultBatchSize = 2000;

    public async Task<PagedResultDto<ProductDto>> GetPagedAsync(ProductQueryDto query, CancellationToken ct = default)
    {
        query.PageNumber = Math.Max(1, query.PageNumber);
        query.PageSize = Math.Clamp(query.PageSize, 1, 100);
        var (items, total) = await productRepository.GetPagedAsync(query, ct);
        return new PagedResultDto<ProductDto>
        {
            Items = items.Select(x => x.ToDto()).ToList(),
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = total
        };
    }

    public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct = default)
        => (await productRepository.GetByIdWithCategoryAsync(id, ct))?.ToDto();

    public async Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken ct = default)
    {
        if (await categoryRepository.GetByIdAsync(dto.CategoryId, ct) is null)
            throw new KeyNotFoundException("La categoría indicada no existe.");

        var entity = dto.ToEntity();
        await productRepository.AddAsync(entity, ct);
        await productRepository.SaveChangesAsync(ct);
        var saved = await productRepository.GetByIdWithCategoryAsync(entity.ProductId, ct);
        return saved!.ToDto();
    }

    public async Task<bool> UpdateAsync(int id, UpdateProductDto dto, CancellationToken ct = default)
    {
        var entity = await productRepository.GetByIdAsync(id, ct);
        if (entity is null) return false;
        if (await categoryRepository.GetByIdAsync(dto.CategoryId, ct) is null)
            throw new KeyNotFoundException("La categoría indicada no existe.");
        dto.Apply(entity);
        productRepository.Update(entity);
        await productRepository.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await productRepository.GetByIdAsync(id, ct);
        if (entity is null) return false;
        productRepository.Delete(entity);
        await productRepository.SaveChangesAsync(ct);
        return true;
    }

    public Guid EnqueueBulkGeneration(BulkCreateProductDto dto)
    {
        var jobId = jobStore.CreateJob(dto.Quantity);
        var batchSize = dto.BatchSize > 0 ? dto.BatchSize : DefaultBatchSize;
        jobQueue.QueueWorkItem(async (sp, ct) =>
        {
            var repository = sp.GetRequiredService<IProductRepository>();
            try
            {
                var faker = new Faker<Product>()
                    .RuleFor(p => p.ProductName, f => f.Commerce.ProductName())
                    .RuleFor(p => p.CategoryId, f => f.PickRandom(dto.CategoryIds))
                    .RuleFor(p => p.QuantityPerUnit, f => $"{f.Random.Int(1, 50)} unidades")
                    .RuleFor(p => p.UnitPrice, f => Math.Round(f.Random.Decimal(1, 5000), 2))
                    .RuleFor(p => p.UnitsInStock, f => (short)f.Random.Int(0, 500))
                    .RuleFor(p => p.UnitsOnOrder, f => (short)f.Random.Int(0, 100))
                    .RuleFor(p => p.ReorderLevel, f => (short)f.Random.Int(0, 50))
                    .RuleFor(p => p.Discontinued, f => f.Random.Bool(0.05f));

                var processed = 0;
                while (processed < dto.Quantity)
                {
                    ct.ThrowIfCancellationRequested();
                    var size = Math.Min(batchSize, dto.Quantity - processed);
                    var batch = faker.Generate(size);
                    await repository.AddRangeAsync(batch, ct);
                    await repository.SaveChangesAsync(ct);
                    processed += size;
                    jobStore.UpdateProgress(jobId, processed);
                }
                jobStore.MarkCompleted(jobId);
            }
            catch (Exception ex) { jobStore.MarkFailed(jobId, ex.Message); }
        });
        return jobId;
    }

    public BulkJobDto? GetJobStatus(Guid jobId) => jobStore.GetStatus(jobId);
}
