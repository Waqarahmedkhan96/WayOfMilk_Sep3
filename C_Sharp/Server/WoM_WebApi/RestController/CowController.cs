namespace WoM_WebApi.RestController;

using ApiContracts;
using Microsoft.AspNetCore.Authorization;//for authorize
using System.Security.Claims; //for accessing User Claims
using Microsoft.AspNetCore.Mvc;
using WoM_WebApi.Service;
[ApiController]
[Route("[controller]")] // This maps to "http://localhost:PORT/cows"
[Authorize] //base rule: user must be logged in
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
            //security measure: overwrite the ID in the DTO with the real ID from the token.
            // This prevents users from faking who registered the cow.
            dto.RegisteredByUserId = GetCurrentUserId();

            CowDto createdCow = await _cowLogic.CreateAsync(dto);
            // Returns 201 Created and the location of the new resource
            return Created($"/cows/{createdCow.Id}", createdCow);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
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
    [Authorize(Policy = "OwnerOnly")]//only owner can change important data fields (ex: regNo, dateOfBirth)
    public async Task<ActionResult<CowDto>> UpdateAsync([FromBody] CowDto dto)
    {
        try
        {
            long requesterId = GetCurrentUserId();

            CowDto updatedCow = await _cowLogic.UpdateCowAsync(dto, _userId);
            return Ok(updatedCow);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
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
    //any user can update health, but only to false, unless user is vet
    public async Task<ActionResult<IEnumerable<CowDto>>> UpdateHealthAsync([FromBody] UpdateHealthRequest request)
    {
        // NOTE: You need a small DTO for this request body (Ids + Status)
        // Or pass them as query parameters, but Body is cleaner for arrays.
        try
        {
            if (request.NewHealthStatus == true)
            {
                // If trying to make cow healthy, MUST be a vet
                if (!IsVet())
                {
                    return Forbid("Only a Veterinarian can declare a cow healthy.");
                }
            }

            // If we get here, either:
            // a) They are setting it to False (Allowed for everyone)
            // b) They are setting it to True AND they are a Vet (Allowed)

            long requesterId = GetCurrentUserId();
            // Collect the stream result into a List to return it
            List<CowDto> results = new();
            await foreach (var cow in _cowLogic.UpdateCowsHealthAsync(request.CowIds, request.NewHealthStatus, _userId))
            {
                results.Add(cow);
            }

            return Ok(results);
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
             Console.WriteLine(e);
             return StatusCode(500, e.Message);
        }
    }

    // DELETE cows/5
    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Owner")]
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
    [Authorize(Roles = "Owner")]
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

    //=========== HELPER methods ============

    // Gets the ID of the current user from the JWT token
    private long GetCurrentUserId()
    {
        // "ClaimTypes.NameIdentifier" is the standard name for the ID inside a token
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (idClaim != null && long.TryParse(idClaim.Value, out long userId))
        {
            return userId;
        }

        // If we can't find the ID, it means something is wrong with the token
        // or the middleware configuration.
        throw new UnauthorizedAccessException("Invalid User ID in token.");
    }

    // Helper to check role
    private bool IsVet()
    {
        return User.IsInRole("Vet"); // Or check specific claim depending on your JWT setup
    }
}