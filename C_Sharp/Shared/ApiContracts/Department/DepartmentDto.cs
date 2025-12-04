namespace ApiContracts.Department;

using Entities;

// Create
public class CreateDepartmentDto
{
    public Department Name { get; set; }
}

// Read
public class DepartmentDto
{
    public int Id { get; set; }
    public Department Name { get; set; }
}

// Update
public class UpdateDepartmentDto
{
    public Department Name { get; set; }
}

// Delete (batch)
public class DeleteDepartmentsDto
{
    public required int[] Ids { get; set; }
}

// List
public class DepartmentListDto
{
    public List<DepartmentDto> Departments { get; set; } = new();
}

// Query
public class DepartmentQueryParameters
{
    public Department? Name { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
