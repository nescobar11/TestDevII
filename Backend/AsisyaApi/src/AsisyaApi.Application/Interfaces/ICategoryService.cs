using AsisyaApi.Application.DTOs.Categories;
namespace AsisyaApi.Application.Interfaces;
public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync(CancellationToken ct = default);
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken ct = default);
}
