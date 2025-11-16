namespace Entities;

public class Sale
{
    public int Id { get; set; }
    public int CreatedByUserId { get; set; }
    public DateOnly DateOnly { get; set; }
    public int ContainerId { get; set; }
    public double QuantityL { get; set; }
    public decimal Price { get; set; }
    public bool RecallCase { get; set; }
    public int CustomerId { get; set; }
}
