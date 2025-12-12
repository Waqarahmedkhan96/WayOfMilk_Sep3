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

    private long GetCurrentUserId()
    {
        // adjust claim type if your JWT uses something else for id
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("id");
        if (idClaim == null)
            throw new InvalidOperationException("User id claim missing in JWT.");
        return long.Parse(idClaim.Value);
    }

    // GET /milk
    [HttpGet]
    [Authorize(Roles = "Owner,Worker")]
    public async Task<ActionResult<MilkListDto>> GetAll()
    {
        var list = await _milkService.GetAllAsync();
        return Ok(list);
    }

    // GET /milk/{id}
    [HttpGet("{id:long}")]
    [Authorize(Roles = "Owner,Worker")]
    public async Task<ActionResult<MilkDto>> GetById(long id)
    {
        var milk = await _milkService.GetByIdAsync(id);
        return Ok(milk);
    }

    // GET /milk/by-container/{containerId}
    [HttpGet("by-container/{containerId:long}")]
    [Authorize(Roles = "Owner,Worker")]
    public async Task<ActionResult<MilkListDto>> GetByContainer(long containerId)
    {
        var list = await _milkService.GetByContainerAsync(containerId);
        return Ok(list);
    }

    // POST /milk  (Worker or Owner)
    [HttpPost]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<ActionResult<MilkDto>> Create(CreateMilkDto dto)
    {
        // 1) create milk in Java (approved=false initially)
        var created = await _milkService.CreateAsync(dto);

        // 2) if user requested approval, call approval flow
        if (dto.ApprovedForStorage)
        {
            var userId = GetCurrentUserId();

            await _milkService.ApproveStorageAsync(new ApproveMilkStorageDto
            {
                Id = created.Id,
                ApprovedByUserId = userId,
                ApprovedForStorage = true
            });

            // reload so response shows updated flag
            created = await _milkService.GetByIdAsync(created.Id);
        }

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /milk/{id}  (Owner or Worker)
    [HttpPut("{id:long}")]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<ActionResult<MilkDto>> Update(long id, UpdateMilkDto dto)
    {
        dto.Id = id;

        // 1) update fields
        var updated = await _milkService.UpdateAsync(dto);

        // 2) if client sent approval change, trigger approve/deny
        if (dto.ApprovedForStorage.HasValue)
        {
            var userId = GetCurrentUserId();

            await _milkService.ApproveStorageAsync(new ApproveMilkStorageDto
            {
                Id = id,
                ApprovedByUserId = userId,
                ApprovedForStorage = dto.ApprovedForStorage.Value
            });

            updated = await _milkService.GetByIdAsync(id);
        }

        return Ok(updated);
    }

    // POST /milk/{id}/approve  (WorkerOrOwner) – explicit endpoint still available
    [HttpPost("{id:long}/approve")]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<IActionResult> Approve(long id, ApproveMilkStorageDto dto)
    {
        dto.Id = id;
        dto.ApprovedByUserId = GetCurrentUserId(); // ignore body value

        await _milkService.ApproveStorageAsync(dto);
        return NoContent();
    }

    // DELETE /milk/{id}  (Owner only)
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> Delete(long id)
    {
        await _milkService.DeleteAsync(id);
        return NoContent();
    }
}
