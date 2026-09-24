using AsisyaApi.Application.DTOs.Products;
using AsisyaApi.Application.Interfaces;
using AsisyaApi.Application.Services;
using AsisyaApi.Domain.Entities;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AsisyaApi.UnitTests;

public class ProductServiceTests
{
    [Fact]
    public async Task CreateAsync_maps_dto_and_persists_product()
    {
        var products = new Mock<IProductRepository>();
        var categories = new Mock<ICategoryRepository>();
        var queue = new Mock<IBackgroundJobQueue>();
        var jobs = new Mock<IProductBulkJobStore>();

        categories.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category { CategoryId = 1, CategoryName = "SERVIDORES" });
        products.Setup(x => x.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Callback<Product, CancellationToken>((p, _) => p.ProductId = 10);
        products.Setup(x => x.GetByIdWithCategoryAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { ProductId=10, ProductName="Test", CategoryId=1, Category=new Category { CategoryId=1, CategoryName="CLOUD" } });

        var service = new ProductService(products.Object, categories.Object, queue.Object, jobs.Object);
        var result = await service.CreateAsync(new CreateProductDto { ProductName="Test", CategoryId=1 });

        Assert.Equal(10, result.ProductId);
        Assert.Equal("CLOUD", result.CategoryName);
        products.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
