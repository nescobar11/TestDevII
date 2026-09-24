using System.ComponentModel.DataAnnotations;

namespace AsisyaApi.Application.DTOs.Categories;

public class CreateCategoryDto
{
    [Required]
    [MaxLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(1000)]
    public string? Picture { get; set; }
}