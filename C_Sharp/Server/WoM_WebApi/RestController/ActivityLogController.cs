using ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("activity-logs")]
[Authorize(Roles = "Owner")] // Only Owners should see logs
public class ActivityLogController : ControllerBase
{
    private readonly ILogService _logService;

    public ActivityLogController(ILogService logService)
    {
        _logService = logService;
    }

    // GET /activity-logs?userId=1&action=Login
    // GET /activity-logs?from=2023-01-01
    [HttpGet]
    public ActionResult<IEnumerable<LogEntry>> GetLogs([FromQuery] LogSearchParameters filters)
    {
        var logs = _logService.SearchLogs(filters);
        return Ok(logs);
    }
}