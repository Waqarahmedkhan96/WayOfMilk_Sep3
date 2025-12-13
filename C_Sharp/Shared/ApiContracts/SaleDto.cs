using System;
using System.Collections.Generic;

namespace ApiContracts;

// DTO: create sale
public class CreateSaleDto
{
    public long CustomerId { get; set; }
    public long ContainerId { get; set; }
    public double QuantityL { get; set; }
    public double Price { get; set; }           // proto: double price
    public long CreatedByUserId { get; set; }
    public DateTime? DateTime { get; set; }     // null => backend sets now
    public bool RecallCase { get; set; }        // default false
}

// DTO: single sale
public class SaleDto
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public long ContainerId { get; set; }
    public double QuantityL { get; set; }
    public double Price { get; set; }
    public DateTime DateTime { get; set; }
    public bool RecallCase { get; set; }
    public long CreatedByUserId { get; set; }
}

// DTO: update sale (WebApi only)
public class UpdateSaleDto
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public long ContainerId { get; set; }
    public double QuantityL { get; set; }
    public double Price { get; set; }
    public DateTime DateTime { get; set; }
    public bool RecallCase { get; set; }
}

// DTO: delete sales batch
public class DeleteSalesDto
{
    public required long[] Ids { get; set; }
}

// DTO: list of sales
public class SaleListDto
{
    public List<SaleDto> Sales { get; set; } = new();
}

// DTO: sale filters
public class SaleQueryParameters
{
    public long? CustomerId { get; set; }
    public long? CreatedByUserId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
