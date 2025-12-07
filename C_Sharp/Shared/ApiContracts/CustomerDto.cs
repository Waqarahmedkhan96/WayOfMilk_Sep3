namespace ApiContracts;

// Create
public class CreateCustomerDto
{
    public required string CompanyName { get; set; }
    public string PhoneNo { get; set; } = "";
    public string Email { get; set; } = "";
    public string CompanyCVR { get; set; } = "";
    public long RegisteredByUserId { get; set; } // who created
}

// Read
public class CustomerDto
{
    public long Id { get; set; }
    public required string CompanyName { get; set; }
    public string PhoneNo { get; set; } = "";
    public string Email { get; set; } = "";
    public string CompanyCVR { get; set; } = "";
    // NOTE: no RegisteredByUserId here – not in proto
}

// Update
public class UpdateCustomerDto
{
    public required string CompanyName { get; set; }
    public string PhoneNo { get; set; } = "";
    public string Email { get; set; } = "";
    public string CompanyCVR { get; set; } = "";
}

// Delete (batch)
public class DeleteCustomersDto
{
    public required long[] Ids { get; set; }
}

// List
public class CustomerListDto
{
    public List<CustomerDto> Customers { get; set; } = new();
}

// Query (in WebApi, not gRPC)
public class CustomerQueryParameters
{
    public string? CompanyContains { get; set; }
    public string? CvrEquals { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
