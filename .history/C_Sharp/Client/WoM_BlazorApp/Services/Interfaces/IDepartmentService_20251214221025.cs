public interface IDepartmentService
{
    Task<ICollection<DepartmentDto>> GetAllAsync();
}
