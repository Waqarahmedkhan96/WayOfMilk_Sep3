using ApiContracts.Sale;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleRepository _sales;

    public SalesController(ISaleRepository sale)
    {
        _sales = sale;
    }

    // POST /sales
    [HttpPost]
    public async Task<ActionResult<SaleDto>> AddSale([FromBody] CreateSaleDto request)
    {
        var created = await _sales.AddAsync(new Sale
        {
            CreatedByUserId = request.CreatedByUserId,
            DateOnly = request.DateOnly,
            ContainerId = request.ContainerId,
            QuantityL = request.QuantityL,
            Price = request.Price,
            CustomerId = request.CustomerId
        });

        var dto = new SaleDto
        {
            Id = created.Id,
            CreatedByUserId = created.CreatedByUserId,
            DateOnly = created.DateOnly,
            ContainerId = created.ContainerId,
            QuantityL = created.QuantityL,
            Price = created.Price,
            RecallCase = created.RecallCase,
            CustomerId = created.CustomerId
        };
        return Created($"/sales/{dto.Id}", dto);
    }

    // GET /sales/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SaleDto>> GetSale(int id)
    {
        var s = await _sales.GetSingleAsync(id);
        var dto = new SaleDto
        {
            Id = s.Id,
            CreatedByUserId = s.CreatedByUserId,
            DateOnly = s.DateOnly,
            ContainerId = s.ContainerId,
            QuantityL = s.QuantityL,
            Price = s.Price,
            RecallCase = s.RecallCase,
            CustomerId = s.CustomerId
        };
        return Ok(dto);
    }

    // GET /sales?customerId=...&createdByUserId=...&from=...&to=...
    [HttpGet]
    public ActionResult<IEnumerable<SaleDto>> GetSales(
        [FromQuery] int? customerId,
        [FromQuery] int? createdByUserId,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to)
    {
        var query = _sales.GetManyAsync();

        if (customerId.HasValue)
            query = query.Where(s => s.CustomerId == customerId.Value);
        if (createdByUserId.HasValue)
            query = query.Where(s => s.CreatedByUserId == createdByUserId.Value);
        if (from.HasValue)
            query = query.Where(s => s.DateOnly >= from.Value);
        if (to.HasValue)
            query = query.Where(s => s.DateOnly <= to.Value);

        var list = query.Select(s => new SaleDto
        {
            Id = s.Id,
            CreatedByUserId = s.CreatedByUserId,
            DateOnly = s.DateOnly,
            ContainerId = s.ContainerId,
            QuantityL = s.QuantityL,
            Price = s.Price,
            RecallCase = s.RecallCase,
            CustomerId = s.CustomerId
        }).ToList();

        return Ok(list);
    }

    // PUT /sales/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateSale(int id, [FromBody] UpdateSaleDto request)
    {
        var sale = new Sale
        {
            Id = id,
            DateOnly = request.DateOnly,
            ContainerId = request.ContainerId,
            QuantityL = request.QuantityL,
            Price = request.Price,
            RecallCase = request.RecallCase,
            CustomerId = request.CustomerId
        };
        await _sales.UpdateAsync(sale);
        return NoContent();
    }

    // DELETE /sales/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSale(int id)
    {
        await _sales.DeleteAsync(id);
        return NoContent();
    }
}
