using ApiContracts.Containers;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Linq;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ContainersController : ControllerBase
{
    private readonly IContainerRepository _containers;

    public ContainersController(IContainerRepository container)
    {
        _containers = container;
    }

    // POST /containers
    [HttpPost]
    public async Task<ActionResult<ContainerDto>> AddContainer([FromBody] CreateContainerDto request)
    {
        var created = await _containers.AddAsync(new Container { CapacityL = request.CapacityL });

        var dto = new ContainerDto { Id = created.Id, CapacityL = created.CapacityL };
        return Created($"/containers/{dto.Id}", dto);
    }

    // GET /containers/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ContainerDto>> GetContainer(int id)
    {
        var c = await _containers.GetSingleAsync(id);
        var dto = new ContainerDto { Id = c.Id, CapacityL = c.CapacityL };
        return Ok(dto);
    }

    // GET /containers?minCapacityL=...&maxCapacityL=...
    [HttpGet]
    public ActionResult<IEnumerable<ContainerDto>> GetContainers([FromQuery] double? minCapacityL, [FromQuery] double? maxCapacityL)
    {
        var query = _containers.GetManyAsync();

        if (minCapacityL.HasValue)
            query = query.Where(c => c.CapacityL >= minCapacityL.Value);
        if (maxCapacityL.HasValue)
            query = query.Where(c => c.CapacityL <= maxCapacityL.Value);

        var list = query.Select(c => new ContainerDto
        {
            Id = c.Id,
            CapacityL = c.CapacityL
        }).ToList();

        return Ok(list);
    }

    // PUT /containers/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateContainer(int id, [FromBody] UpdateContainerDto request)
    {
        var container = new Container { Id = id, CapacityL = request.CapacityL };
        await _containers.UpdateAsync(container);
        return NoContent();
    }

    // DELETE /containers/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteContainer(int id)
    {
        await _containers.DeleteAsync(id);
        return NoContent();
    }
}
