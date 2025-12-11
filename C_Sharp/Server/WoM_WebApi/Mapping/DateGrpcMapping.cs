using ApiContracts;
using Sep3.WayOfMilk.Grpc;

namespace WoM_WebApi.Mapping;

public class DateGrpcMapping
{
    //=========== DATE ===========
    public static string ToGrpcDate(DateOnly date)
        => date.ToString("yyyy-MM-dd");

    // Safe parsing: Prevents crashes if DB sends empty string
    public static DateOnly FromGrpcDate(string? dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
        {
            // Fallback date (e.g., 2000-01-01) or DateOnly.MinValue
            return new DateOnly(2000, 1, 1);
        }
        return DateOnly.Parse(dateString);
    }

    //=========== DATETIME ===========

    public static string ToGrpcDateTime(DateTime dt)
        => dt.ToString("yyyy-MM-dd'T'HH:mm:ss");

    public static DateTime FromGrpcDateTime(string? s)
        => string.IsNullOrWhiteSpace(s)
            ? DateTime.MinValue
            : DateTime.Parse(s);


}