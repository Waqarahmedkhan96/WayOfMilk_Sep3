namespace ApiContracts;

// Create
public class CreateDepartmentDto
{
    public DepartmentType Type { get; set; }
}

// Read
public class DepartmentDto
{
    public long Id { get; set; }
    public DepartmentType Type { get; set; }
}

// Update
public class UpdateDepartmentDto
{
    public DepartmentType Type { get; set; }
}

// Delete (batch)
public class DeleteDepartmentsDto
{
    public required long[] Ids { get; set; }
}

// List
public class DepartmentListDto
{
    public List<DepartmentDto> Departments { get; set; } = new();
}

// Query
public class DepartmentQueryParameters
{
    public DepartmentType? Type { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}
