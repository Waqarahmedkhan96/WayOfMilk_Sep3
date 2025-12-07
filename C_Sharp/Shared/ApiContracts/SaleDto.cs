namespace ApiContracts;

// Create
public class CreateSaleDto
{
    public long CreatedByUserId { get; set; }
    public DateOnly Date { get; set; }
    public long ContainerId { get; set; }
    public double QuantityL { get; set; }
    public decimal Price { get; set; }
    public long CustomerId { get; set; }
    public bool RecallCase { get; set; } // default false
}

// Read/ Get
public class SaleDto
{
    public long Id { get; set; }
    public long CreatedByUserId { get; set; }
    public DateOnly Date { get; set; }
    public long ContainerId { get; set; }
    public double QuantityL { get; set; }
    public decimal Price { get; set; }
    public bool RecallCase { get; set; }
    public long CustomerId { get; set; }
}

// Update
public class UpdateSaleDto
{
    public DateOnly Date { get; set; }
    public long ContainerId { get; set; }
    public double QuantityL { get; set; }
    public decimal Price { get; set; }
    public bool RecallCase { get; set; }
    public long CustomerId { get; set; }
}

// Delete (batch)
public class DeleteSalesDto
{
    public required long[] Ids { get; set; }
}

// List
public class SaleListDto
{
    public List<SaleDto> Sales { get; set; } = new();
}

// Query
public class SaleQueryParameters
{
    public long? CustomerId { get; set; }
    public long? CreatedByUserId { get; set; }
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
