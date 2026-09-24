using AsisyaApi.Application.DTOs.Products;
using AsisyaApi.Application.Interfaces;
using AsisyaApi.Domain.Entities;
using AsisyaApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AsisyaApi.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(List<Product> Items, int TotalCount)> GetPagedAsync(ProductQueryDto query, CancellationToken ct = default)
        {
            var q = _db.Products.AsNoTracking().Include(x => x.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim();
                q = q.Where(x => x.ProductName.ToLower().Contains(search.ToLower()) ||
                                 (x.Category != null && x.Category.CategoryName.ToLower().Contains(search.ToLower())));
            }

            if (query.CategoryId.HasValue) q = q.Where(x => x.CategoryId == query.CategoryId.Value);
            if (query.MinPrice.HasValue) q = q.Where(x => x.UnitPrice >= query.MinPrice.Value);
            if (query.MaxPrice.HasValue) q = q.Where(x => x.UnitPrice <= query.MaxPrice.Value);
            if (query.Discontinued.HasValue) q = q.Where(x => x.Discontinued == query.Discontinued.Value);

            var total = await q.CountAsync(ct);
            var items = await q.OrderBy(x => x.ProductId)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(ct);

            return (items, total);
        }

        public Task<Product?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _db.Products.FirstOrDefaultAsync(x => x.ProductId == id, ct);

        public Task<Product?> GetByIdWithCategoryAsync(int id, CancellationToken ct = default) =>
            _db.Products.AsNoTracking().Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.ProductId == id, ct);

        public Task AddAsync(Product product, CancellationToken ct = default) =>
            _db.Products.AddAsync(product, ct).AsTask();

        public Task AddRangeAsync(IEnumerable<Product> products, CancellationToken ct = default) =>
            _db.Products.AddRangeAsync(products, ct);

        public void Update(Product product) => _db.Products.Update(product);

        public void Delete(Product product) => _db.Products.Remove(product);

        public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
    }
}