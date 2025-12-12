using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using ApiContracts;

namespace WoM_WebApi.Log;



public sealed class ActivityLog
{
    // Singleton
    // "Lazy" ensures it's only created when first needed and is thread-safe.
    private static readonly Lazy<ActivityLog> _instance =
        new Lazy<ActivityLog>(() => new ActivityLog());

    public static ActivityLog Instance => _instance.Value;

    private readonly string _directoryPath;
    private readonly string _filePath;
    private readonly object _fileLock = new object(); // Prevents file access conflicts

    // Private constructor prevents external instantiation
    private ActivityLog()
    {
        // Base directory on the running application's location
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        _directoryPath = Path.Combine(baseDir, "Activity_Logs");
        _filePath = Path.Combine(_directoryPath, "activity_logs.jsonl"); // .jsonl = JSON Lines

        if (!Directory.Exists(_directoryPath))
        {
            Directory.CreateDirectory(_directoryPath);
        }
    }

    //Log Method
    public void Log(string action, string details, string? userName = null, long userId = 0)
    {
        var entry = new LogEntry
        {
            Timestamp = DateTime.UtcNow,
            Action = action,
            Details = details,
            UserId = userId,
            UserName = userName
        };

        string jsonLine = JsonSerializer.Serialize(entry);

        // Lock the file so multiple threads don't crash the app
        lock (_fileLock)
        {
            File.AppendAllText(_filePath, jsonLine + Environment.NewLine);
        }
    }

    // Query Method
    public IEnumerable<LogEntry> Query(Func<LogEntry, bool> filter)
    {
        lock (_fileLock)
        {
            if (!File.Exists(_filePath)) return new List<LogEntry>();

            // Read the file line by line and deserialize
            var allLogs = File.ReadLines(_filePath)
                              .Select(line => JsonSerializer.Deserialize<LogEntry>(line))
                              // Filter out any nulls if a line is corrupt
                              .Where(x => x != null)
                              .Cast<LogEntry>();

            // Apply the requested filter (time, user, etc.)
            return allLogs.Where(filter).ToList();
        }
    }

    //get all logs
    public IEnumerable<LogEntry> QueryAll() => Query(_ => true);
}