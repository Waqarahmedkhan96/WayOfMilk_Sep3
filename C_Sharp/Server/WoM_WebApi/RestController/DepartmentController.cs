using ApiContracts.Department;
using Entities;
using Microsoft.AspNetCore.Mvc;

namespace WoM_WebApi.RestController;

[ApiController]
[Route("[controller]")]
public class DepartmentsController : ControllerBase
{
    // GET /departments
    [HttpGet]
    public ActionResult<IEnumerable<DepartmentDto>> GetDepartments()
    {
        var values = Enum.GetValues<Department>();

        var list = values
            .Select(d => new DepartmentDto
            {
                Id = (int)d,
                Name = d
            })
            .ToList();

        return Ok(list);
    }

    // GET /departments/{id}
    [HttpGet("{id:int}")]
    public ActionResult<DepartmentDto> GetDepartment(int id)
    {
        if (!Enum.IsDefined(typeof(Department), id))
            return NotFound();

        var d = (Department)id;

        var dto = new DepartmentDto
        {
            Id = id,
            Name = d
        };

        return Ok(dto);
    }
}
