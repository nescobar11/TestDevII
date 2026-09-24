using System.Threading.Channels;
using AsisyaApi.Application.Interfaces;

namespace AsisyaApi.Infrastructure.Background
{
    public class BackgroundJobQueue : IBackgroundJobQueue
    {
        private readonly Channel<Func<IServiceProvider, CancellationToken, Task>> _queue =
            Channel.CreateUnbounded<Func<IServiceProvider, CancellationToken, Task>>();

        public void QueueWorkItem(Func<IServiceProvider, CancellationToken, Task> workItem)
        {
            _queue.Writer.TryWrite(workItem);
        }

        public ValueTask<Func<IServiceProvider, CancellationToken, Task>> DequeueAsync(CancellationToken ct)
        {
            return _queue.Reader.ReadAsync(ct);
        }
    }
}