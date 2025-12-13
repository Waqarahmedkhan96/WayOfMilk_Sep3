// File: Server/WoM_WebApi/RestController/CowController.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("cows")]
public class CowController : ControllerBase
{
    private readonly ICowService _cowService;

    public CowController(ICowService cowService)
    {
        _cowService = cowService;
    }

    // helper: get current user id from JWT
    private long GetCurrentUserId()
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)
                  ?? User.FindFirst(JwtRegisteredClaimNames.Sub);

        return sub != null && long.TryParse(sub.Value, out var id)
            ? id
            : 0;
    }

    // GET /cows  (Owner, Worker, Vet)
    [HttpGet]
    [Authorize(Roles = "Owner,Worker,Vet")]
    public async Task<ActionResult<CowListDto>> GetAll()
    {
        // 1) fetch list from service
        var list = await _cowService.GetAllAsync();

        // 2) return ok with cows
        return Ok(list);
    }

    // GET /cows/{id}
    [HttpGet("{id:long}")]
    [Authorize(Roles = "Owner,Worker,Vet")]
    public async Task<ActionResult<CowDto>> GetById(long id)
    {
        // 1) fetch cow by id
        var cow = await _cowService.GetByIdAsync(id);

        // 2) return single cow
        return Ok(cow);
    }

    // POST /cows   (Worker or Owner)
    [HttpPost]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<ActionResult<CowDto>> Create(CreateCowDto dto)
    {
        // 1) take user id from token
        var userId = GetCurrentUserId();

        // 2) call service to create cow
        var created = await _cowService.CreateAsync(dto, userId);

        // 3) return 201 with location
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /cows/{id}   (Worker or Owner)
    [HttpPut("{id:long}")]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<ActionResult<CowDto>> Update(long id, UpdateCowDto dto)
    {
        // 1) sync route id → dto
        dto.Id = id;

        // 2) get user id for audit
        var userId = GetCurrentUserId();

        // 3) call service update
        var updated = await _cowService.UpdateAsync(dto, userId);

        return Ok(updated);
    }

    // DELETE /cows/{id}   (Owner only)
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> Delete(long id)
    {
        // 1) ask service to delete
        await _cowService.DeleteAsync(id);

        // 2) return 204
        return NoContent();
    }
}
