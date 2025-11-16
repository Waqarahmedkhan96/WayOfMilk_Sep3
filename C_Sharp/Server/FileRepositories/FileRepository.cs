// FileRepository.cs  (base class)
using System.Text.Json;
using System.Threading;

namespace FileRepositories;

public abstract class FileRepository<T> where T : class
{
    private static readonly Dictionary<string, SemaphoreSlim> _locks = new();
    private readonly string _path;
    private readonly JsonSerializerOptions _json = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    protected FileRepository(string relativePath)
    {
        var dir = Path.GetDirectoryName(relativePath);
        if (!string.IsNullOrWhiteSpace(dir)) Directory.CreateDirectory(dir);
        _path = relativePath;
    }

    private SemaphoreSlim Gate
    {
        get
        {
            lock (_locks)
            {
                if (!_locks.TryGetValue(_path, out var g))
                {
                    g = new SemaphoreSlim(1, 1);
                    _locks[_path] = g;
                }
                return g;
            }
        }
    }

    protected async Task<List<T>> LoadAsync()
    {
        await Gate.WaitAsync();
        try
        {
            if (!File.Exists(_path)) return new List<T>();
            var json = await File.ReadAllTextAsync(_path);
            return JsonSerializer.Deserialize<List<T>>(json, _json) ?? new List<T>();
        }
        finally { Gate.Release(); }
    }

    protected async Task SaveAsync(List<T> items)
    {
        await Gate.WaitAsync();
        try
        {
            var json = JsonSerializer.Serialize(items, _json);
            await File.WriteAllTextAsync(_path, json);
        }
        finally { Gate.Release(); }
    }
}
