using AsisyaApi.Application.Interfaces;
using AsisyaApi.Domain.Entities;
using AsisyaApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AsisyaApi.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _db;

        public CategoryRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<List<Category>> GetAllAsync(CancellationToken ct = default) =>
            _db.Categories.AsNoTracking().OrderBy(x => x.CategoryName).ToListAsync(ct);

        public Task<Category?> GetByIdAsync(int id, CancellationToken ct = default) =>
            _db.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.CategoryId == id, ct);

        public Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default) =>
            _db.Categories.AnyAsync(x => x.CategoryName.ToLower() == name.Trim().ToLower(), ct);

        public Task AddAsync(Category category, CancellationToken ct = default) =>
            _db.Categories.AddAsync(category, ct).AsTask();

        public Task SaveChangesAsync(CancellationToken ct = default) =>
            _db.SaveChangesAsync(ct);
    }
}