using System.Collections.Concurrent;
using AsisyaApi.Application.DTOs.Products;
using AsisyaApi.Application.Interfaces;

namespace AsisyaApi.Infrastructure.Background
{
    public class ProductBulkJobStore : IProductBulkJobStore
    {
        private class State
        {
            public int Requested { get; set; }
            public int Processed { get; set; }
            public string Status { get; set; } = "Pending";
            public string? Error { get; set; }
        }

        private readonly ConcurrentDictionary<Guid, State> _states = new();

        public Guid CreateJob(int requested)
        {
            var id = Guid.NewGuid();
            _states[id] = new State { Requested = requested };
            return id;
        }

        public void UpdateProgress(Guid id, int processed)
        {
            if (_states.TryGetValue(id, out var s))
            {
                s.Processed = processed;
                s.Status = processed >= s.Requested ? "Completed" : "Processing";
            }
        }

        public void MarkCompleted(Guid id)
        {
            if (_states.TryGetValue(id, out var s))
            {
                s.Status = "Completed";
            }
        }

        public void MarkFailed(Guid id, string error)
        {
            if (_states.TryGetValue(id, out var s))
            {
                s.Status = "Failed";
                s.Error = error;
            }
        }

        public BulkJobDto? GetStatus(Guid id)
        {
            if (_states.TryGetValue(id, out var s))
            {
                return new BulkJobDto
                {
                    JobId = id,
                    Requested = s.Requested,
                    Processed = s.Processed,
                    Status = s.Status,
                    Error = s.Error
                };
            }

            return null;
        }
    }
}