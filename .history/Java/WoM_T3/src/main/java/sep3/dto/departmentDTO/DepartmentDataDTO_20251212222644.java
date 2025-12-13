package sep3.dto.departmentDTO;

import sep3.entity.DepartmentType;

public class DepartmentDataDTO
{
  private Long id;
  private DepartmentType type;
  private String name;

  public DepartmentDataDTO() { }

  public DepartmentDataDTO(Long id, DepartmentType type, String name)
  {
    this.id = id;
    this.type = type;
    this.name = name;
  }

  public Long getId() { return id; }

  public void setId(Long id) { this.id = id; }

  public DepartmentType getType() { return type; }

  public void setType(DepartmentType type) { this.type = type; }

  public String getName() { return name; }

  public void setName(String name) { this.name = name; }

}
