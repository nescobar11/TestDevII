namespace AsisyaApi.Application.DTOs.Products;

public class BulkJobDto
{
    public Guid JobId { get; set; }
    public string Status { get; set; } = string.Empty;
    public int Requested { get; set; }
    public int Processed { get; set; }
    public string? Error { get; set; }
}