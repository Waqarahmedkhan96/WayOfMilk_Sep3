using WoM_WebApi.Log;
using ApiContracts;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.Services;

//no interface
public class LogServiceImpl : ILogService
{
    //only querying logs functions here
    public IEnumerable<LogEntry> SearchLogs(LogSearchParameters p)
    {
        // We pass a Lambda to the ActivityLog.Instance.Query method
        return ActivityLog.Instance.Query(log =>
        {
            // Time Filter (From)
            if (p.From.HasValue && log.Timestamp < p.From.Value)
                return false;

            // Time Filter (To)
            if (p.To.HasValue && log.Timestamp > p.To.Value)
                return false;

            // User Filter
            if (p.UserId.HasValue && log.UserId != p.UserId.Value)
                return false;

            // Action Filter (Case insensitive)
            if (!string.IsNullOrEmpty(p.Action) &&
                !string.Equals(log.Action, p.Action, StringComparison.OrdinalIgnoreCase))
                return false;

            // Text Search (Check details or username)
            if (!string.IsNullOrEmpty(p.SearchText))
            {
                bool matchesDetails = log.Details.Contains(p.SearchText, StringComparison.OrdinalIgnoreCase);
                bool matchesUser = log.UserName?.Contains(p.SearchText, StringComparison.OrdinalIgnoreCase) ?? false;

                if (!matchesDetails && !matchesUser) return false;
            }

            // If it passed all checks, include it!
            return true;
        });
    }
}


