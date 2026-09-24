using AsisyaApi.Application.DTOs.Categories;
using AsisyaApi.Application.Interfaces;
using AsisyaApi.Application.Mappings;

namespace AsisyaApi.Application.Services;

public sealed class CategoryService(ICategoryRepository repository) : ICategoryService
{
    public async Task<List<CategoryDto>> GetAllAsync(CancellationToken ct = default)
        => (await repository.GetAllAsync(ct)).Select(x => x.ToDto()).ToList();

    public async Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct = default)
        => (await repository.GetByIdAsync(id, ct))?.ToDto();

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct = default)
    {
        if (await repository.ExistsByNameAsync(dto.CategoryName, ct))
            throw new InvalidOperationException($"La categoría '{dto.CategoryName}' ya existe.");

        var entity = dto.ToEntity();
        await repository.AddAsync(entity, ct);
        await repository.SaveChangesAsync(ct);
        return entity.ToDto();
    }
}
