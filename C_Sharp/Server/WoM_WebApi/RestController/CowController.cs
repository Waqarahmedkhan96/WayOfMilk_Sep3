using WoM_WebApi.Helper;
using WoM_WebApi.Log;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.RestController;

using ApiContracts;
using Microsoft.AspNetCore.Authorization;//for authorize
using System.Security.Claims; //for accessing User Claims
using Microsoft.AspNetCore.Mvc;
using WoM_WebApi.Services;

[ApiController]
[Route("[controller]")] // This maps to "http://localhost:PORT/cows"
[Authorize] //base rule: user must be logged in
public class CowsController : ControllerBase
{
    private readonly ICowService _cowLogic;

    public CowsController(ICowService cowLogic)
    {
        _cowLogic = cowLogic;
    }

    // POST cows
    //Owner only
    [HttpPost(Name = "AddCow")]
    [Authorize(Roles = "Owner")]
    public async Task<ActionResult<CowDto>> CreateAsync(
        [FromBody] CowCreationDto dto)
    {
        ActivityLog.Instance.Log("Create cow",
            $"User {User.GetJWTNameAndId()} trying to create a new cow",
            User.GetJWTName(), User.GetJWTId());
        try
        {
            //security measure: overwrite the ID in the DTO with the real ID from the token.
            // This prevents users from faking who registered the cow.
            dto.RegisteredByUserId = User.GetJWTId();

            CowDto createdCow = await _cowLogic.CreateAsync(dto);
            ActivityLog.Instance.Log("Created cow",
                $"Cow {createdCow.Id} with registration number {createdCow.RegNo} added by user {User.GetJWTNameAndId()}",
                User.GetJWTName(), User.GetJWTId());
            // Returns 201 Created and the location of the new resource
            //return Created($"/cows/{createdCow.Id}", createdCow);
            //createatroute updates changes to route paths
            return CreatedAtRoute("GetCowById", new { id = createdCow.Id },
                createdCow);
        }
        catch (UnauthorizedAccessException e)
        {
            ActivityLog.Instance.Log("Unauthorized",
                $"User {User.GetJWTId()} tried to create a new cow",
                User.GetJWTName(), User.GetJWTId());
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            ActivityLog.Instance.Log("Error",
                $"Error creating cow: {e.Message}", User.GetJWTName(),
                User.GetJWTId());
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    // GET cows
    // GET cows?regNo=123
    [HttpGet(Name = "GetAllCows")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<CowDto>>> GetAsync(
        [FromQuery] string? regNo)
    {
        try
        {
            if (!string.IsNullOrEmpty(regNo))
            {
                // If query param ?regNo=... is present, find specific cow
                CowDto cow = await _cowLogic.GetByRegNoAsync(regNo);
                return Ok(new List<CowDto>
                {
                    cow
                }); // Return as list for consistency? Or just the object.
            }

            // Otherwise get all
            IEnumerable<CowDto> cows = await _cowLogic.GetAllAsync();
            return Ok(cows);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ActivityLog.Instance.Log("Error",
                $"Error getting cows: {e.Message}", User.GetJWTName(),
                User.GetJWTId());
            return StatusCode(500, e.Message);
        }
    }

    // GET cows/5
    [HttpGet("{id:long}", Name = "GetCowById")]
    [Authorize]
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
            ActivityLog.Instance.Log("Error",
                $"Error getting cow {id}: {e.Message}", User.GetJWTName(),
                User.GetJWTId());
            return StatusCode(500, e.Message);
        }
    }

    // PUT cows
    [HttpPut(Name = "UpdateCow")]
    [Authorize(Roles =
        "Owner")] //only owner can change important data fields (ex: regNo, dateOfBirth)
    public async Task<ActionResult<CowDto>> UpdateAsync([FromBody] CowDto dto)
    {
        try
        {
            long requesterId = User.GetJWTId();

            CowDto updatedCow =
                await _cowLogic.UpdateCowAsync(dto, requesterId);
            ActivityLog.Instance.Log("Update Cow",
                $"Cow {updatedCow.Id} with registration number {updatedCow.RegNo} updated by user {User.GetJWTNameAndId()}",
                User.GetJWTName(), User.GetJWTId());
            return Ok(updatedCow);
        }
        catch (UnauthorizedAccessException e)
        {
            ActivityLog.Instance.Log("Unauthorized",
                $"User {User.GetJWTId()} tried to update cow {dto.Id}",
                User.GetJWTName(), User.GetJWTId());
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ActivityLog.Instance.Log("Error",
                $"Error updating cow {dto.Id}: {e.Message}", User.GetJWTName(),
                User.GetJWTId());
            return StatusCode(500, e.Message);
        }
    }

    // PUT cows/health
    // Updates health for a list of cows
    [HttpPut("health", Name = "updateHealth")]
    [Authorize]
    //any user can update health, but only to false, unless user is vet
    public async Task<ActionResult<IEnumerable<CowDto>>> UpdateHealthAsync(
        [FromBody] UpdateHealthRequest request)
    {
        // NOTE: You need a small DTO for this request body (Ids + Status)
        // Or pass them as query parameters, but Body is cleaner for arrays.
        ActivityLog.Instance.Log("Update Health",
            $"User {User.GetJWTNameAndId()} trying to update health for cows {request.CowIds}",
            User.GetJWTName(), User.GetJWTId());
        try
        {
            if (request.NewHealthStatus == true)
            {
                // If trying to make cow healthy, MUST be a vet
                // Fix: Avoid magic string "Vet". Use Enum, or IsInRole check.
                if (!User.IsInRole("Vet"))
                {
                    ActivityLog.Instance.Log("Unauthorized",
                        $"Non-Vet {User.GetJWTId()} tried to declare cows healthy.",
                        User.GetJWTName(), User.GetJWTId());
                    return Forbid();
                }
            }

            // If we get here, either:
            // a) They are setting it to False (Allowed for everyone)
            // b) They are setting it to True AND they are a Vet (Allowed)

            long requesterId = User.GetJWTId();
            // Collect the stream result into a List to return it
            List<CowDto> results = new();
            await foreach (var cow in _cowLogic.UpdateCowsHealthAsync(
                               request.CowIds, request.NewHealthStatus,
                               requesterId))
            {
                results.Add(cow);
            }

            ActivityLog.Instance.Log("Updated Health",
                $"Health of cows {request.CowIds} updated by user {User.GetJWTNameAndId()}",
                User.GetJWTName(), User.GetJWTId());

            return Ok(results);
        }
        catch (UnauthorizedAccessException e)
        {
            ActivityLog.Instance.Log("Unauthorized",
                $"User {User.GetJWTId()} tried to update health for cows {request.CowIds}",
                User.GetJWTName(), User.GetJWTId());
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ActivityLog.Instance.Log("Error",
                $"Error updating health for cows {request.CowIds}: {e.Message}",
                User.GetJWTName(), User.GetJWTId());
            return StatusCode(500, e.Message);
        }
    }

    // DELETE cows/5
    [HttpDelete("{id:long}", Name = "DeleteOneCow")]
    [Authorize(Roles = "Owner")]
    public async Task<ActionResult> DeleteAsync([FromRoute] long id)
    {
        ActivityLog.Instance.Log("Delete Cow",
            $"Cow {id} delete request by user {User.GetJWTNameAndId()}",
            User.GetJWTName(), User.GetJWTId());
        try
        {
            await _cowLogic.DeleteAsync(id);
            ActivityLog.Instance.Log("Deleted Cow",
                $"Cow {id} deleted by user {User.GetJWTNameAndId()}",
                User.GetJWTName(), User.GetJWTId());
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ActivityLog.Instance.Log("Error",
                $"Error deleting cow {id}: {e.Message}", User.GetJWTName(),
                User.GetJWTId());
            return StatusCode(500, e.Message);
        }
    }

    // DELETE cows/batch
    [HttpDelete("batch-delete", Name = "DeleteBatchCows")]
    [Authorize(Roles = "Owner")]
    public async Task<ActionResult> DeleteBatchAsync([FromBody] long[] ids)
    {
        ActivityLog.Instance.Log("Delete Batch",
            $"Batch delete request for cows {ids} by user {User.GetJWTNameAndId()}",
            User.GetJWTName(), User.GetJWTId());
        try
        {
            await _cowLogic.DeleteBatchAsync(ids);
            ActivityLog.Instance.Log("Deleted Batch",
                $"Batch delete request for cows {ids} by user {User.GetJWTNameAndId()}",
                User.GetJWTName(), User.GetJWTId());
            return NoContent();
        }
        catch (Exception e)
        {
            ActivityLog.Instance.Log("Error",
                $"Error deleting batch of cows {ids}: {e.Message}",
                User.GetJWTName(), User.GetJWTId());
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("department/{id:long}", Name = "GetCowsFromDepartment")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<CowDto>>> getCowsFromDepartment(
        [FromRoute] long id)
    {

        try
        {
            IEnumerable<CowDto> cows =
                await _cowLogic.GetCowsByDepartmentAsync(id);
            return Ok(cows);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ActivityLog.Instance.Log("Error",
                $"Error getting cows from department {id}: {e.Message}",
                User.GetJWTName(), User.GetJWTId());
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("milk/{id:long}", Name = "GetMilkFromCow")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<MilkDto>>> getMilkFromCow(
        [FromRoute] long id)
    {
        try
        {
            IEnumerable<MilkDto> milkRecords =
                await _cowLogic.GetMilkByCowIdAsync(id);
            return Ok(milkRecords);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ActivityLog.Instance.Log("Error",
                $"Error getting milk from cow {id}: {e.Message}",
                User.GetJWTName(), User.GetJWTId());
            return StatusCode(500, e.Message);
        }

    }
}