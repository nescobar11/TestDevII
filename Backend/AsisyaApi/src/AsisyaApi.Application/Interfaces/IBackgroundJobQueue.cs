namespace AsisyaApi.Application.Interfaces;
public interface IBackgroundJobQueue
{
    void QueueWorkItem(Func<IServiceProvider, CancellationToken, Task> workItem);
    ValueTask<Func<IServiceProvider, CancellationToken, Task>> DequeueAsync(CancellationToken ct);
}
