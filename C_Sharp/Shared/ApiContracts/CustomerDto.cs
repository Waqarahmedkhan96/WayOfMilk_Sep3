using System;
using System.Collections.Generic;

namespace ApiContracts;

// DTO: create customer
public class CreateCustomerDto
{
    public required string CompanyName { get; set; }
    public string PhoneNo { get; set; } = "";
    public string Email { get; set; } = "";
    public string CompanyCVR { get; set; } = "";
    public long RegisteredByUserId { get; set; } // WebApi only (not proto)
}

// DTO: single customer
public class CustomerDto
{
    public long Id { get; set; }
    public required string CompanyName { get; set; }
    public string PhoneNo { get; set; } = "";
    public string Email { get; set; } = "";
    public string CompanyCVR { get; set; } = "";
}

// DTO: update customer
public class UpdateCustomerDto
{
    public required string CompanyName { get; set; }
    public string PhoneNo { get; set; } = "";
    public string Email { get; set; } = "";
    public string CompanyCVR { get; set; } = "";
}

// DTO: delete customers batch
public class DeleteCustomersDto
{
    public required long[] Ids { get; set; }
}

// DTO: list of customers
public class CustomerListDto
{
    public List<CustomerDto> Customers { get; set; } = new();
}

// DTO: customer filters
public class CustomerQueryParameters
{
    public string? CompanyContains { get; set; }
    public string? CvrEquals { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
