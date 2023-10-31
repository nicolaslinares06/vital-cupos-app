using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TestUnit.WEB
{
    public class FakeSession : ISession
    {
        private readonly Dictionary<string, byte[]> _sessionData = new Dictionary<string, byte[]>();

        public bool IsAvailable => true;
        public string Id => Guid.NewGuid().ToString();

        public IEnumerable<string> Keys => _sessionData.Keys;

        public void Clear()
        {
            _sessionData.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            _sessionData.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _sessionData[key] = value;
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            return _sessionData.TryGetValue(key, out value);
        }

        // Métodos adicionales para manejar valores de cadena en la sesión

        public string GetString(string key)
        {
            if (_sessionData.TryGetValue(key, out var value))
            {
                return System.Text.Encoding.UTF8.GetString(value);
            }
            return null;
        }

        public void SetString(string key, string value)
        {
            _sessionData[key] = System.Text.Encoding.UTF8.GetBytes(value);
        }
    }
}
