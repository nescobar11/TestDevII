using AsisyaApi.Application.DTOs.Products;
namespace AsisyaApi.Application.Interfaces;
public interface IProductBulkJobStore
{
    Guid CreateJob(int requested);
    void UpdateProgress(Guid jobId, int processed);
    void MarkCompleted(Guid jobId);
    void MarkFailed(Guid jobId, string error);
    BulkJobDto? GetStatus(Guid jobId);
}
