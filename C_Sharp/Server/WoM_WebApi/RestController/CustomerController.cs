using ApiContracts.Customers;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerRepository _customers;

    public CustomersController(ICustomerRepository customer)
    {
        _customers = customer;
    }

    // POST /customers
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> AddCustomer([FromBody] CreateCustomerDto request)
    {
        var created = await _customers.AddAsync(new Customer
        {
            CompanyName = request.CompanyName,
            PhoneNo = request.PhoneNo,
            Email = request.Email,
            CompanyCVR = request.CompanyCVR
        });

        var dto = new CustomerDto
        {
            Id = created.Id,
            CompanyName = created.CompanyName,
            PhoneNo = created.PhoneNo,
            Email = created.Email,
            CompanyCVR = created.CompanyCVR
        };
        return Created($"/customers/{dto.Id}", dto);
    }

    // GET /customers/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
    {
        var c = await _customers.GetSingleAsync(id);
        var dto = new CustomerDto
        {
            Id = c.Id,
            CompanyName = c.CompanyName,
            PhoneNo = c.PhoneNo,
            Email = c.Email,
            CompanyCVR = c.CompanyCVR
        };
        return Ok(dto);
    }

    // GET /customers?companyContains=...&cvrEquals=...
    [HttpGet]
    public ActionResult<IEnumerable<CustomerDto>> GetCustomers(
        [FromQuery] string? companyContains,
        [FromQuery] string? cvrEquals)
    {
        var query = _customers.GetManyAsync();

        if (!string.IsNullOrWhiteSpace(companyContains))
            query = query.Where(c => c.CompanyName.Contains(companyContains, StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(cvrEquals))
            query = query.Where(c => c.CompanyCVR == cvrEquals);

        var list = query.Select(c => new CustomerDto
        {
            Id = c.Id,
            CompanyName = c.CompanyName,
            PhoneNo = c.PhoneNo,
            Email = c.Email,
            CompanyCVR = c.CompanyCVR
        }).ToList();

        return Ok(list);
    }

    // PUT /customers/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDto request)
    {
        var customer = new Customer
        {
            Id = id,
            CompanyName = request.CompanyName,
            PhoneNo = request.PhoneNo,
            Email = request.Email,
            CompanyCVR = request.CompanyCVR
        };
        await _customers.UpdateAsync(customer);
        return NoContent();
    }

    // DELETE /customers/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        await _customers.DeleteAsync(id);
        return NoContent();
    }
}
