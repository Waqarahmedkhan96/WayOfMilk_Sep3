using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("milk")]
public class MilkController : ControllerBase
{
    private readonly IMilkService _milkService;

    public MilkController(IMilkService milkService)
    {
        _milkService = milkService;
    }

    // GET /milk
    [HttpGet]
    [Authorize(Roles = "Owner,Worker,Vet")]
    public async Task<ActionResult<MilkListDto>> GetAll()
    {
        // 1) fetch all milk records
        var list = await _milkService.GetAllAsync();
        // 2) return list
        return Ok(list);
    }

    // GET /milk/{id}
    [HttpGet("{id:long}")]
    [Authorize(Roles = "Owner,Worker,Vet")]
    public async Task<ActionResult<MilkDto>> GetById(long id)
    {
        // 1) fetch record
        var milk = await _milkService.GetByIdAsync(id);
        // 2) return record
        return Ok(milk);
    }

    // GET /milk/by-container/{containerId}
    [HttpGet("by-container/{containerId:long}")]
    [Authorize(Roles = "Owner,Worker,Vet")]
    public async Task<ActionResult<MilkListDto>> GetByContainer(long containerId)
    {
        // 1) fetch list by container
        var list = await _milkService.GetByContainerAsync(containerId);
        // 2) return list
        return Ok(list);
    }

    // POST /milk  (Worker or Owner)
    [HttpPost]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<ActionResult<MilkDto>> Create(CreateMilkDto dto)
    {
        // 1) create record via service
        var created = await _milkService.CreateAsync(dto);
        // 2) return 201
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /milk/{id}  (Owner only)
    [HttpPut("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<ActionResult<MilkDto>> Update(long id, UpdateMilkDto dto)
    {
        // 1) ensure path id used
        dto.Id = id;
        // 2) call update
        var updated = await _milkService.UpdateAsync(dto);
        return Ok(updated);
    }

    // POST /milk/{id}/approve  (Vet only)
    [HttpPost("{id:long}/approve")]
    [Authorize(Policy = "VetOnly")]
    public async Task<IActionResult> Approve(long id, ApproveMilkStorageDto dto)
    {
        // 1) fix dto id from route
        dto.Id = id;
        // 2) call approve
        await _milkService.ApproveStorageAsync(dto);
        return NoContent();
    }

    // DELETE /milk/{id}  (Owner only)
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> Delete(long id)
    {
        // 1) delete via service
        await _milkService.DeleteAsync(id);
        // 2) return 204
        return NoContent();
    }
}
