package sep3.dto.departmentDTO;

import sep3.entity.DepartmentType;

public class DepartmentDataDTO
{
  private Long id;
  private DepartmentType type;

  public DepartmentDataDTO() { }

  public DepartmentDataDTO(Long id, DepartmentType type)
  {
    this.id = id;
    this.type = type;
  }

  public Long getId() { return id; }

  public void setId(Long id) { this.id = id; }

  public DepartmentType getType() { return type; }

  public void setType(DepartmentType type) { this.type = type; }

}
