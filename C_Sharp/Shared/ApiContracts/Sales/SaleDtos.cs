namespace ApiContracts.Sales;

// Read
public class SaleDto
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

// Create
public class CreateSaleDto
{
    public int CreatedByUserId { get; set; }
    public DateOnly DateOnly { get; set; }
    public int ContainerId { get; set; }
    public double QuantityL { get; set; }
    public decimal Price { get; set; }
    public int CustomerId { get; set; }
}

// Update
public class UpdateSaleDto
{
    public DateOnly DateOnly { get; set; }
    public int ContainerId { get; set; }
    public double QuantityL { get; set; }
    public decimal Price { get; set; }
    public bool RecallCase { get; set; }
    public int CustomerId { get; set; }
}

// Delete (batch)
public class DeleteSalesDto
{
    public required int[] Ids { get; set; }
}

// List
public class SaleListDto
{
    public List<SaleDto> Sales { get; set; } = new();
}

// Query
public class SaleQueryParameters
{
    public int? CustomerId { get; set; }
    public int? CreatedByUserId { get; set; }
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
