namespace WoM_WebApi.Services.Interfaces;

using ApiContracts;


public interface ILogService
{
    IEnumerable<LogEntry> SearchLogs(LogSearchParameters parameters);
}