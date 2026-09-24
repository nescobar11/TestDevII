using AsisyaApi.Application.DTOs.Products;
using AsisyaApi.Domain.Entities;

namespace AsisyaApi.Application.Interfaces;

public interface IProductRepository
{
    Task<(List<Product> Items, int TotalCount)> GetPagedAsync(ProductQueryDto query, CancellationToken ct = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken ct = default);
    Task AddAsync(Product product, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<Product> products, CancellationToken ct = default);
    void Update(Product product);
    void Delete(Product product);
    Task SaveChangesAsync(CancellationToken ct = default);
}
