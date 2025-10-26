using System.Collections.Concurrent;
using System.Collections.Generic;

namespace UserManagementAPI.Api.Middleware
{
    public class EndpointUsageTracker
    {
        private readonly ConcurrentDictionary<string, long> _counts = new();

        public void Increment(string key) => _counts.AddOrUpdate(key, 1, (_, v) => v + 1);

        public IReadOnlyDictionary<string, long> GetCounts() => new Dictionary<string, long>(_counts);

        public long GetCount(string key) => _counts.TryGetValue(key, out var v) ? v : 0;
    }
}