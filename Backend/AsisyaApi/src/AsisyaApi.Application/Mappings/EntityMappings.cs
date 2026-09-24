using AsisyaApi.Application.DTOs.Categories;
using AsisyaApi.Application.DTOs.Products;
using AsisyaApi.Domain.Entities;

namespace AsisyaApi.Application.Mappings;

public static class EntityMappings
{
    public static Product ToEntity(this CreateProductDto dto) => new()
    {
        ProductName = dto.ProductName.Trim(),
        CategoryId = dto.CategoryId,
        QuantityPerUnit = dto.QuantityPerUnit,
        UnitPrice = dto.UnitPrice,
        UnitsInStock = dto.UnitsInStock,
        UnitsOnOrder = dto.UnitsOnOrder,
        ReorderLevel = dto.ReorderLevel,
        Discontinued = dto.Discontinued
    };

    public static void Apply(this UpdateProductDto dto, Product entity)
    {
        entity.ProductName = dto.ProductName.Trim();
        entity.CategoryId = dto.CategoryId;
        entity.QuantityPerUnit = dto.QuantityPerUnit;
        entity.UnitPrice = dto.UnitPrice;
        entity.UnitsInStock = dto.UnitsInStock;
        entity.UnitsOnOrder = dto.UnitsOnOrder;
        entity.ReorderLevel = dto.ReorderLevel;
        entity.Discontinued = dto.Discontinued;
    }

    public static ProductDto ToDto(this Product p) => new()
    {
        ProductId = p.ProductId,
        ProductName = p.ProductName,
        CategoryId = p.CategoryId,
        CategoryName = p.Category?.CategoryName ?? string.Empty,
        CategoryPicture = p.Category?.Picture,
        QuantityPerUnit = p.QuantityPerUnit,
        UnitPrice = p.UnitPrice,
        UnitsInStock = p.UnitsInStock,
        UnitsOnOrder = p.UnitsOnOrder,
        ReorderLevel = p.ReorderLevel,
        Discontinued = p.Discontinued
    };

    public static Category ToEntity(this CreateCategoryDto dto) => new()
    {
        CategoryName = dto.CategoryName.Trim(),
        Description = dto.Description,
        Picture = dto.Picture
    };

    public static CategoryDto ToDto(this Category c) => new()
    {
        CategoryId = c.CategoryId,
        CategoryName = c.CategoryName,
        Description = c.Description,
        Picture = c.Picture
    };
}
