using Client.Models;
using System.Text.Json;

namespace Client.Services
{
    public enum OperationType
    {
        Create,
        Update,
        Delete
    }

    public class QueuedOperation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public OperationType Type { get; set; }
        public ProductEntryDto? Data { get; set; }
        public int? EntityId { get; set; }
        public DateTime QueuedAt { get; set; } = DateTime.Now;
    }

    public class SyncQueueService
    {
        private readonly LocalStorageService _localStorage;
        private const string QueueKey = "sync_queue";

        public SyncQueueService(LocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<List<QueuedOperation>> GetQueueAsync()
        {
            return await _localStorage.GetItemAsync<List<QueuedOperation>>(QueueKey)
                   ?? new List<QueuedOperation>();
        }

        public async Task AddToQueueAsync(OperationType type, ProductEntryDto? data = null, int? entityId = null)
        {
            var queue = await GetQueueAsync();
            queue.Add(new QueuedOperation
            {
                Type = type,
                Data = data,
                EntityId = entityId
            });
            await _localStorage.SetItemAsync(QueueKey, queue);
        }

        public async Task RemoveFromQueueAsync(Guid operationId)
        {
            var queue = await GetQueueAsync();
            queue.RemoveAll(op => op.Id == operationId);
            await _localStorage.SetItemAsync(QueueKey, queue);
        }

        public async Task ClearQueueAsync()
        {
            await _localStorage.RemoveItemAsync(QueueKey);
        }
    }
}