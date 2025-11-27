using Client.Models;
using System.Net.Http.Json;

namespace Client.Services
{
    public class ProductEntryService(
        HttpClient http,
        LocalStorageService localStorage,
        SyncQueueService syncQueue,
        ConnectionStatusService connectionStatus)
    {
        private readonly HttpClient _http = http;
        private readonly LocalStorageService _localStorage = localStorage;
        private readonly SyncQueueService _syncQueue = syncQueue;
        private readonly ConnectionStatusService _connectionStatus = connectionStatus;
        private const string CacheKey = "cached_entries";
        private const string ApiUrl = "http://localhost:5188/api/productentries";

        public async Task<List<ProductEntryDto>> GetAllAsync()
        {
            if (_connectionStatus.IsOnline)
            {
                try
                {
                    var entries = await _http.GetFromJsonAsync<List<ProductEntryDto>>(ApiUrl);
                    if (entries != null)
                    {
                        // Cache for offline use
                        await _localStorage.SetItemAsync(CacheKey, entries);
                        return entries;
                    }
                }
                catch
                {
                    // Fall through to cached data
                }
            }

            // Return cached data if offline or API call failed
            return await _localStorage.GetItemAsync<List<ProductEntryDto>>(CacheKey)
                   ?? [];
        }

        public async Task<bool> CreateAsync(ProductEntryDto entry)
        {
            if (_connectionStatus.IsOnline)
            {
                try
                {
                    var response = await _http.PostAsJsonAsync(ApiUrl, entry);
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
                catch
                {
                    // Fall through to offline handling
                }
            }

            // Queue for sync when online
            entry.Id = -(new Random().Next(1, 1000000)); // Temporary negative ID
            entry.EntryDateTime = DateTime.Now; // Set the timestamp
            entry.CreatedAt = DateTime.Now; // Set the timestamp

            await _syncQueue.AddToQueueAsync(OperationType.Create, entry);

            // Add to local cache
            var cached = await GetAllAsync();
            cached.Add(entry);
            await _localStorage.SetItemAsync(CacheKey, cached);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // If it's a temporary negative ID (offline creation), just remove from cache
            if (id < 0)
            {
                // Don't queue for sync, just remove from local cache
                var localCache = await GetAllAsync();
                localCache.RemoveAll(e => e.Id == id);
                await _localStorage.SetItemAsync(CacheKey, localCache);
                return true;
            }

            if (_connectionStatus.IsOnline)
            {
                try
                {
                    var response = await _http.DeleteAsync($"{ApiUrl}/{id}");
                    if (response.IsSuccessStatusCode)
                    {
                        // Remove from cache too
                        var onlineCache = await GetAllAsync();
                        onlineCache.RemoveAll(e => e.Id == id);
                        await _localStorage.SetItemAsync(CacheKey, onlineCache);
                        return true;
                    }
                }
                catch
                {
                    // Fall through to offline handling
                }
            }

            // Queue for sync when online (only for real IDs)
            await _syncQueue.AddToQueueAsync(OperationType.Delete, entityId: id);

            // Remove from local cache
            var offlineCache = await GetAllAsync();
            offlineCache.RemoveAll(e => e.Id == id);
            await _localStorage.SetItemAsync(CacheKey, offlineCache);

            return true;
        }

        public async Task SyncAsync()
        {
            Console.WriteLine("🔄 Starting sync...");

            if (!_connectionStatus.IsOnline)
            {
                Console.WriteLine("❌ Offline, skipping sync");
                return;
            }

            var queue = await _syncQueue.GetQueueAsync();
            Console.WriteLine($"📋 Queue has {queue.Count} operations");

            var processedOperations = new List<Guid>();

            foreach (var operation in queue)
            {
                try
                {
                    bool success = false;

                    switch (operation.Type)
                    {
                        case OperationType.Create:
                            if (operation.Data != null)
                            {
                                Console.WriteLine($"Creating entry: {operation.Data.ProductModel}");
                                var response = await _http.PostAsJsonAsync(ApiUrl, operation.Data);

                                if (response.IsSuccessStatusCode)
                                {
                                    success = true;
                                    Console.WriteLine("✅ Create succeeded");
                                }
                                else
                                {
                                    var error = await response.Content.ReadAsStringAsync();
                                    Console.WriteLine($"❌ Create failed: {response.StatusCode} - {error}");
                                }
                            }
                            break;

                        case OperationType.Delete:
                            if (operation.EntityId.HasValue && operation.EntityId.Value > 0)
                            {
                                Console.WriteLine($"Deleting entry: {operation.EntityId}");
                                var response = await _http.DeleteAsync($"{ApiUrl}/{operation.EntityId.Value}");

                                // Consider 404 as success (already deleted)
                                if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NotFound)
                                {
                                    success = true;
                                    Console.WriteLine("✅ Delete succeeded");
                                }
                                else
                                {
                                    Console.WriteLine($"❌ Delete failed: {response.StatusCode}");
                                }
                            }
                            else
                            {
                                // Skip negative IDs (temporary offline IDs)
                                success = true;
                                Console.WriteLine("⏭️ Skipped delete of temporary ID");
                            }
                            break;
                    }

                    if (success)
                    {
                        processedOperations.Add(operation.Id);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Operation failed: {ex.Message}");
                    continue;
                }
            }

            // Remove successfully synced operations
            foreach (var opId in processedOperations)
            {
                await _syncQueue.RemoveFromQueueAsync(opId);
            }

            Console.WriteLine($"✅ Synced {processedOperations.Count} operations");

            // Refresh cache after sync
            if (processedOperations.Count != 0)
            {
                await GetAllAsync();
            }
        }
    }
}