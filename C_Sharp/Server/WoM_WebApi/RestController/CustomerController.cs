using ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("customers")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET /customers
    [HttpGet]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<ActionResult<CustomerListDto>> GetAll([FromQuery] CustomerQueryParameters? query)
    {
        // 1) simple: ignore filters, just fetch all for now
        var list = await _customerService.GetAllAsync();
        // 2) return list
        return Ok(list);
    }

    // GET /customers/{id}
    [HttpGet("{id:long}")]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<ActionResult<CustomerDto>> GetById(long id)
    {
        // 1) fetch customer
        var customer = await _customerService.GetByIdAsync(id);
        // 2) return dto
        return Ok(customer);
    }

    // POST /customers  (Worker or Owner)
    [HttpPost]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<ActionResult<CustomerDto>> Create(CreateCustomerDto dto)
    {
        // 1) create via service
        var created = await _customerService.CreateAsync(dto);
        // 2) return 201
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // DELETE /customers/{id} (Owner only)
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> Delete(long id)
    {
        // 1) delete via service
        await _customerService.DeleteAsync(id);
        // 2) return 204
        return NoContent();
    }
}
