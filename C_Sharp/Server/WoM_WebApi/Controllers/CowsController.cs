using ApiContracts.Cows;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Linq;

namespace WoM_WebApi.Controllers;

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
        var created = await _cows.AddAsync(new Cow { BirthDate = request.BirthDate });

        var dto = new CowDto { Id = created.Id, BirthDate = created.BirthDate };
        return Created($"/cows/{dto.Id}", dto);
    }

    // GET /cows/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CowDto>> GetCow(int id)
    {
        var cow = await _cows.GetSingleAsync(id);
        var dto = new CowDto { Id = cow.Id, BirthDate = cow.BirthDate };
        return Ok(dto);
    }

    // GET /cows?bornAfter=...&bornBefore=...
    [HttpGet]
    public ActionResult<IEnumerable<CowDto>> GetCows([FromQuery] DateOnly? bornAfter, [FromQuery] DateOnly? bornBefore)
    {
        var query = _cows.GetManyAsync();

        if (bornAfter.HasValue)
            query = query.Where(c => c.BirthDate >= bornAfter.Value);
        if (bornBefore.HasValue)
            query = query.Where(c => c.BirthDate <= bornBefore.Value);

        var list = query.Select(c => new CowDto
        {
            Id = c.Id,
            BirthDate = c.BirthDate
        }).ToList();

        return Ok(list);
    }

    // PUT /cows/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCow(int id, [FromBody] UpdateCowDto request)
    {
        var cow = new Cow { Id = id, BirthDate = request.BirthDate };
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
