package sep3.dto.departmentDTO;

import sep3.entity.DepartmentType;

public class DepartmentCreationDTO
{
    private DepartmentType type;
    private String name;

    public DepartmentCreationDTO() { }

    public DepartmentCreationDTO(DepartmentType type, String name)
    {
        this.type = type;
        this.name = name;
    }

    public DepartmentType getType()
    {
        return type;
    }

    public void setType(DepartmentType type)
    {
        this.type = type;
    }

    public String getName()
    {
        return name;
    }

    public void setName(String name)
    {
        this.name = name;
    }

}
