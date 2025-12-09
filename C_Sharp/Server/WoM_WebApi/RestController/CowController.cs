namespace WoM_WebApi.RestController;

using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using WoM_WebApi.Service;
[ApiController]
[Route("[controller]")] // This maps to "http://localhost:PORT/cows"
public class CowsController : ControllerBase
{
    private readonly ICowBusinessLogic _cowLogic;
    private long _userId = 1; // Hardcoded for now, later will get from authentication

    public CowsController(ICowBusinessLogic cowLogic)
    {
        _cowLogic = cowLogic;
    }

    // POST cows
    [HttpPost]
    public async Task<ActionResult<CowDto>> CreateAsync([FromBody] CowCreationDto dto)
    {
        try
        {
            CowDto createdCow = await _cowLogic.CreateAsync(dto);
            // Returns 201 Created and the location of the new resource
            return Created($"/cows/{createdCow.Id}", createdCow);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    // GET cows
    // GET cows?regNo=123
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CowDto>>> GetAsync([FromQuery] string? regNo)
    {
        try
        {
            if (!string.IsNullOrEmpty(regNo))
            {
                // If query param ?regNo=... is present, find specific cow
                CowDto cow = await _cowLogic.GetByRegNoAsync(regNo);
                return Ok(new List<CowDto> { cow }); // Return as list for consistency? Or just the object.
            }

            // Otherwise get all
            IEnumerable<CowDto> cows = await _cowLogic.GetAllAsync();
            return Ok(cows);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    // GET cows/5
    [HttpGet("{id:long}")]
    public async Task<ActionResult<CowDto>> GetByIdAsync([FromRoute] long id)
    {
        try
        {
            CowDto cow = await _cowLogic.GetByIdAsync(id);
            return Ok(cow);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    // PUT cows
    [HttpPut]
    public async Task<ActionResult<CowDto>> UpdateAsync([FromBody] CowDto dto)
    {
        try
        {
            CowDto updatedCow = await _cowLogic.UpdateCowAsync(dto, _userId);
            return Ok(updatedCow);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    // PUT cows/health
    // Updates health for a list of cows
    [HttpPut("health")]
    public async Task<ActionResult<IEnumerable<CowDto>>> UpdateHealthAsync([FromBody] UpdateHealthRequest request)
    {
        // NOTE: You need a small DTO for this request body (Ids + Status)
        // Or pass them as query parameters, but Body is cleaner for arrays.
        try
        {
            // Collect the stream result into a List to return it
            List<CowDto> results = new();
            await foreach (var cow in _cowLogic.UpdateCowsHealthAsync(request.CowIds, request.NewHealthStatus, _userId))
            {
                results.Add(cow);
            }

            return Ok(results);
        }
        catch (Exception e)
        {
             Console.WriteLine(e);
             return StatusCode(500, e.Message);
        }
    }

    // DELETE cows/5
    [HttpDelete("{id:long}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] long id)
    {
        try
        {
            await _cowLogic.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    // DELETE cows/batch
    [HttpDelete("batch")]
    public async Task<ActionResult> DeleteBatchAsync([FromBody] long[] ids)
    {
        try
        {
            await _cowLogic.DeleteBatchAsync(ids);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}