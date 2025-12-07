package sep3.dto.departmentDTO;

import sep3.entity.DepartmentType;

public class DepartmentCreationDTO
{
    private DepartmentType type;

    public DepartmentCreationDTO() { }

    public DepartmentCreationDTO(DepartmentType type)
    {
        this.type = type;
    }

    public DepartmentType getType()
    {
        return type;
    }

    public void setType(DepartmentType type)
    {
        this.type = type;
    }
}
