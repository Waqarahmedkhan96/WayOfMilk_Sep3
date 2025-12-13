package sep3.mapping;

import sep3.entity.Department;
import sep3.dto.departmentDTO.DepartmentDataDTO;

public class DepartmentMapper
{
    private DepartmentMapper() { }

    public static DepartmentDataDTO convertDepartmentToDto(Department department)
    {
        if (department == null)
        {
            return null;
        }

        return new DepartmentDataDTO(
                department.getId(),
                department.getType()
        );
    }
}
