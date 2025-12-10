using ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("transfers")]
public class TransferRecordController : ControllerBase
{
    private readonly ITransferRecordService _transferService;

    public TransferRecordController(ITransferRecordService transferService)
    {
        _transferService = transferService;
    }

    // GET /transfers   (Owner only)
    [HttpGet]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<ActionResult<TransferRecordListDto>> GetAll()
    {
        // 1) fetch all transfers
        var list = await _transferService.GetAllAsync();
        // 2) return list
        return Ok(list);
    }

    // GET /transfers/{id}   (Owner only)
    [HttpGet("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<ActionResult<TransferRecordDto>> GetById(long id)
    {
        // 1) fetch transfer
        var tr = await _transferService.GetByIdAsync(id);
        // 2) return dto
        return Ok(tr);
    }

    // GET /transfers/by-cow/{cowId}   (Owner only for now)
    [HttpGet("by-cow/{cowId:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<ActionResult<TransferRecordListDto>> GetForCow(long cowId)
    {
        // 1) fetch transfers for cow
        var list = await _transferService.GetForCowAsync(cowId);
        // 2) return list
        return Ok(list);
    }

    // POST /transfers   (Worker or Owner)
    [HttpPost]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<ActionResult<TransferRecordDto>> Create(CreateTransferRecordDto dto)
    {
        // 1) create new transfer
        var created = await _transferService.CreateAsync(dto);
        // 2) return 201
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /transfers/{id}   (Owner only)
    [HttpPut("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<ActionResult<TransferRecordDto>> Update(long id, UpdateTransferRecordDto dto)
    {
        // 1) set id from route
        dto.Id = id;
        // 2) update via service
        var updated = await _transferService.UpdateAsync(dto);
        return Ok(updated);
    }

    // POST /transfers/{id}/approve   (Vet only)
    [HttpPost("{id:long}/approve")]
    [Authorize(Policy = "VetOnly")]
    public async Task<IActionResult> Approve(long id, ApproveTransferDto dto)
    {
        // 1) fix transfer id
        dto.TransferId = id;
        // 2) call approve
        await _transferService.ApproveAsync(dto);
        return NoContent();
    }

    // DELETE /transfers/{id}   (Owner only)
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> Delete(long id)
    {
        // 1) delete via service
        await _transferService.DeleteAsync(id);
        // 2) return 204
        return NoContent();
    }
}
