using ApiContracts.Milk;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("[controller]")]
public class MilkController : ControllerBase
{
    private readonly IMilkRepository _milk;

    public MilkController(IMilkRepository milk)
    {
        _milk = milk;
    }

    // POST /milks
    [HttpPost]
    public async Task<ActionResult<MilkDto>> AddMilk([FromBody] CreateMilkDto request)
    {
        // rule: if approved, test must be PASS
        if (request.ApprovedForStorage && request.MilkTestResult != MilkTestResult.Pass)
            return BadRequest("Milk can be approved only when test result is PASS.");

        // rule: cannot assign container before approval
        if (!request.ApprovedForStorage && request.ContainerId is not null)
            return BadRequest("Cannot assign a container before milk is approved.");

        var created = await _milk.AddAsync(new Milk
        {
            DateOnly = request.DateOnly,
            VolumeL = request.VolumeL,
            CowId = request.CowId,
            ContainerId = request.ContainerId,
            ApprovedForStorage = request.ApprovedForStorage,
            MilkTestResult = request.MilkTestResult,
            CreatedByUserId = request.CreatedByUserId
        });

        var dto = new MilkDto
        {
            Id = created.Id,
            DateOnly = created.DateOnly,
            VolumeL = created.VolumeL,
            MilkTestResult = created.MilkTestResult,
            ApprovedForStorage = created.ApprovedForStorage,
            CowId = created.CowId,
            ContainerId = created.ContainerId,
            CreatedByUserId = created.CreatedByUserId
        };
        return Created($"/milks/{dto.Id}", dto);
    }

    // GET /milks/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MilkDto>> GetMilk(int id)
    {
        var m = await _milk.GetSingleAsync(id);
        var dto = new MilkDto
        {
            Id = m.Id,
            DateOnly = m.DateOnly,
            VolumeL = m.VolumeL,
            MilkTestResult = m.MilkTestResult,
            ApprovedForStorage = m.ApprovedForStorage,
            CowId = m.CowId,
            ContainerId = m.ContainerId,
            CreatedByUserId = m.CreatedByUserId
        };
        return Ok(dto);
    }

    // GET /milks?cowId=...&containerId=...&from=...&to=...
    [HttpGet]
    public ActionResult<IEnumerable<MilkDto>> GetMilks(
        [FromQuery] int? cowId,
        [FromQuery] int? containerId,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to)
    {
        var query = _milk.GetManyAsync();

        if (cowId.HasValue)
            query = query.Where(m => m.CowId == cowId.Value);
        if (containerId.HasValue)
            query = query.Where(m => m.ContainerId == containerId.Value);
        if (from.HasValue)
            query = query.Where(m => m.DateOnly >= from.Value);
        if (to.HasValue)
            query = query.Where(m => m.DateOnly <= to.Value);

        var list = query.Select(m => new MilkDto
        {
            Id = m.Id,
            DateOnly = m.DateOnly,
            VolumeL = m.VolumeL,
            MilkTestResult = m.MilkTestResult,
            ApprovedForStorage = m.ApprovedForStorage,
            CowId = m.CowId,
            ContainerId = m.ContainerId,
            CreatedByUserId = m.CreatedByUserId
        }).ToList();

        return Ok(list);
    }

    // PUT /milks/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateMilk(int id, [FromBody] UpdateMilkDto request)
    {
        // rule: cannot assign container before approval
        if (!request.ApprovedForStorage && request.ContainerId is not null)
            return BadRequest("Cannot assign a container before milk is approved.");

        // rule: if approved, test must be PASS
        if (request.ApprovedForStorage && request.MilkTestResult != MilkTestResult.Pass)
            return BadRequest("Milk can be approved only when test result is PASS.");

        var milk = new Milk
        {
            Id = id,
            DateOnly = request.DateOnly,
            VolumeL = request.VolumeL,
            MilkTestResult = request.MilkTestResult,
            ApprovedForStorage = request.ApprovedForStorage,
            CowId = request.CowId,
            ContainerId = request.ContainerId,
            CreatedByUserId = request.CreatedByUserId
        };
        await _milk.UpdateAsync(milk);
        return NoContent();
    }

    // DELETE /milks/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMilk(int id)
    {
        await _milk.DeleteAsync(id);
        return NoContent();
    }
}
