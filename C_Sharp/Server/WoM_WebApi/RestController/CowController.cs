using ApiContracts.Cow;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Linq;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("[controller]")]
public class CowsController : ControllerBase
{
    private readonly ICowRepository _cows;

    public CowsController(ICowRepository cow)
    {
        _cows = cow;
    }

    // POST /cows
    [HttpPost]
    public async Task<ActionResult<CowDto>> AddCow([FromBody] CreateCowDto request)
    {
        var created = await _cows.AddAsync(new Cow
        {
            RegNo = request.RegNo,
            BirthDate = request.BirthDate,
            IsHealthy = request.IsHealthy
        });

        var dto = new CowDto
        {
            Id = created.Id,
            RegNo = created.RegNo,
            BirthDate = created.BirthDate,
            IsHealthy = created.IsHealthy
        };
        return Created($"/cows/{dto.Id}", dto);
    }

    // GET /cows/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CowDto>> GetCow(int id)
    {
        var cow = await _cows.GetSingleAsync(id);
        var dto = new CowDto
        {
            Id = cow.Id,
            RegNo = cow.RegNo,
            BirthDate = cow.BirthDate,
            IsHealthy = cow.IsHealthy
        };
        return Ok(dto);
    }

    // GET /cows?regNoEquals=...&bornAfter=...&bornBefore=...&isHealthy=...
    [HttpGet]
    public ActionResult<IEnumerable<CowDto>> GetCows(
        [FromQuery] string? regNoEquals,
        [FromQuery] DateOnly? bornAfter,
        [FromQuery] DateOnly? bornBefore,
        [FromQuery] bool? isHealthy)
    {
        var query = _cows.GetManyAsync();

        if (!string.IsNullOrWhiteSpace(regNoEquals))
            query = query.Where(c => c.RegNo == regNoEquals);
        if (bornAfter.HasValue)
            query = query.Where(c => c.BirthDate >= bornAfter.Value);
        if (bornBefore.HasValue)
            query = query.Where(c => c.BirthDate <= bornBefore.Value);
        if (isHealthy.HasValue)
            query = query.Where(c => c.IsHealthy == isHealthy.Value);

        var list = query.Select(c => new CowDto
        {
            Id = c.Id,
            RegNo = c.RegNo,
            BirthDate = c.BirthDate,
            IsHealthy = c.IsHealthy
        }).ToList();

        return Ok(list);
    }

    // PUT /cows/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCow(int id, [FromBody] UpdateCowDto request)
    {
        var cow = new Cow
        {
            Id = id,
            RegNo = request.RegNo,
            BirthDate = request.BirthDate,
            IsHealthy = request.IsHealthy
        };

        await _cows.UpdateAsync(cow);
        return NoContent();
    }

    // DELETE /cows/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCow(int id)
    {
        await _cows.DeleteAsync(id);
        return NoContent();
    }
}
