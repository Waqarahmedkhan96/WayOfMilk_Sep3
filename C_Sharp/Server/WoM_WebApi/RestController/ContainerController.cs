// File: Server/WoM_WebApi/RestController/ContainerController.cs
using ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("containers")]
public class ContainerController : ControllerBase
{
    private readonly IContainerService _containerService;

    public ContainerController(IContainerService containerService)
    {
        _containerService = containerService;
    }

    // GET /containers  (Owner, Worker)
    [HttpGet]
    [Authorize(Roles = "Owner,Worker")]
    public async Task<ActionResult<ContainerListDto>> GetAll()
    {
        // 1) fetch all containers
        var list = await _containerService.GetAllAsync();

        // 2) return ok list
        return Ok(list);
    }

    // GET /containers/{id}
    [HttpGet("{id:long}")]
    [Authorize(Roles = "Owner,Worker")]
    public async Task<ActionResult<ContainerDto>> GetById(long id)
    {
        // 1) fetch container by id
        var container = await _containerService.GetByIdAsync(id);

        // 2) return single dto
        return Ok(container);
    }

    // POST /containers   (Worker or Owner)
    [HttpPost]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<ActionResult<ContainerDto>> Create(CreateContainerDto dto)
    {
        // 1) call service to create
        var created = await _containerService.CreateAsync(dto);

        // 2) return 201 with location
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /containers/{id}   (Owner or Worker only)
    [HttpPut("{id:long}")]
    [Authorize(Policy = "WorkerOrOwner")]
    public async Task<ActionResult<ContainerDto>> Update(long id, UpdateContainerDto dto)
    {
        // 1) call service to update
        var updated = await _containerService.UpdateAsync(id, dto);

        // 2) return updated dto
        return Ok(updated);
    }

    // DELETE /containers/{id}   (Owner only)
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> Delete(long id)
    {
        // 1) ask service to delete
        await _containerService.DeleteAsync(id);

        // 2) return 204
        return NoContent();
    }
}
