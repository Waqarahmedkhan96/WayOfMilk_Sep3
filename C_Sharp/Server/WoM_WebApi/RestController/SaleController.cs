using ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("sales")]
public class SaleController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SaleController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    // GET /sales   (Owner only)
    [HttpGet]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<ActionResult<SaleListDto>> GetAll()
    {
        // 1) fetch all sales
        var list = await _saleService.GetAllAsync();
        // 2) return list
        return Ok(list);
    }

    // GET /sales/{id}   (Owner only)
    [HttpGet("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<ActionResult<SaleDto>> GetById(long id)
    {
        // 1) fetch sale
        var sale = await _saleService.GetByIdAsync(id);
        // 2) return dto
        return Ok(sale);
    }

    // POST /sales   (Worker or Owner)
    [HttpPost]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<ActionResult<SaleDto>> Create(CreateSaleDto dto)
    {
        // 1) create via service
        var created = await _saleService.CreateAsync(dto);
        // 2) return 201
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // DELETE /sales/{id}   (Owner only)
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> Delete(long id)
    {
        // 1) delete via service
        await _saleService.DeleteAsync(id);
        // 2) return 204
        return NoContent();
    }
}
