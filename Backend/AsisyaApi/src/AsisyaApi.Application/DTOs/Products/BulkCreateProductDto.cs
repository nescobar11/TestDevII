using System.ComponentModel.DataAnnotations;

namespace AsisyaApi.Application.DTOs.Products;

public class BulkCreateProductDto
{
    [Range(1, 1_000_000)]
    public int Quantity { get; set; }

    [Required, MinLength(1)]
    public List<int> CategoryIds { get; set; } = [];

    [Range(100, 10_000)]
    public int BatchSize { get; set; } = 2000;
}