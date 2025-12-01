namespace ApiContracts.Customers;

// Create
public class CreateCustomerDto
{
    public required string CompanyName { get; set; }
    public string PhoneNo { get; set; } = "";
    public string Email { get; set; } = "";
    public string CompanyCVR { get; set; } = "";
}

// Read
public class CustomerDto
{
    public int Id { get; set; }
    public required string CompanyName { get; set; }
    public string PhoneNo { get; set; } = "";
    public string Email { get; set; } = "";
    public string CompanyCVR { get; set; } = "";
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
    public required int[] Ids { get; set; }
}

// List
public class CustomerListDto
{
    public List<CustomerDto> Customers { get; set; } = new();
}

// Query
public class CustomerQueryParameters
{
    public string? CompanyContains { get; set; }
    public string? CvrEquals { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
