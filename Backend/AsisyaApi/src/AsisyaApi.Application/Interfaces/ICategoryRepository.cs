using AsisyaApi.Domain.Entities;
namespace AsisyaApi.Application.Interfaces;
public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(CancellationToken ct = default);
    Task<Category?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
    Task AddAsync(Category category, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
