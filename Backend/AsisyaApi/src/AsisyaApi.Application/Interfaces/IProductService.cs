using AsisyaApi.Application.DTOs.Common;
using AsisyaApi.Application.DTOs.Products;
namespace AsisyaApi.Application.Interfaces;
public interface IProductService
{
    Task<PagedResultDto<ProductDto>> GetPagedAsync(ProductQueryDto query, CancellationToken ct = default);
    Task<ProductDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(int id, UpdateProductDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    Guid EnqueueBulkGeneration(BulkCreateProductDto dto);
    BulkJobDto? GetJobStatus(Guid jobId);
}
