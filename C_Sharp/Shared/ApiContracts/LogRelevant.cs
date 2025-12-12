namespace ApiContracts;

public class LogRelevant
{
    
}
public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public long? UserId { get; set; }
    public string? UserName { get; set; }
    public string Action { get; set; }
    public string Details { get; set; }
}

public class LogSearchParameters
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public long? UserId { get; set; }
    public string? Action { get; set; }
    // You could even add a generic search for the "Details" message
    public string? SearchText { get; set; }
}