using ApiContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoM_WebApi.Services.Interfaces;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("departments")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    // GET /departments
    [HttpGet]
    [Authorize(Roles = "Owner,Worker,Vet")]
    public async Task<ActionResult<DepartmentListDto>> GetAll()
    {
        // 1) fetch all departments
        var list = await _departmentService.GetAllAsync();
        // 2) return list
        return Ok(list);
    }

    // GET /departments/{id}
    [HttpGet("{id:long}")]
    [Authorize(Roles = "Owner,Worker,Vet")]
    public async Task<ActionResult<DepartmentDto>> GetById(long id)
    {
        // 1) fetch department
        var dept = await _departmentService.GetByIdAsync(id);
        // 2) return dto
        return Ok(dept);
    }

    // POST /departments   (Owner only)
    [HttpPost]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<ActionResult<DepartmentDto>> Create(CreateDepartmentDto dto)
    {
        // 1) create via service
        var created = await _departmentService.CreateAsync(dto);
        // 2) return 201
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /departments/{id}   (Owner only)
    [HttpPut("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<ActionResult<DepartmentDto>> Update(long id, UpdateDepartmentDto dto)
    {
        // 1) update via service
        var updated = await _departmentService.UpdateAsync(id, dto);
        // 2) return dto
        return Ok(updated);
    }

    // DELETE /departments/{id}   (Owner only)
    [HttpDelete("{id:long}")]
    [Authorize(Policy = "OwnerOnly")]
    public async Task<IActionResult> Delete(long id)
    {
        // 1) delete via service
        await _departmentService.DeleteAsync(id);
        // 2) return 204
        return NoContent();
    }
}
