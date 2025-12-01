namespace Entities;

public class Cow
{
    public int Id { get; set; }

    // Unique registration number for each cow
    public string RegNo { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    // Whether the cow is currently healthy
    public bool IsHealthy { get; set; }
}
