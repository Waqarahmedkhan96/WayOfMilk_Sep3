package sep3.dto;

import sep3.entity.DepartmentType;

public class DepartmentDataDTO
{
  private long id;
  private DepartmentType type;

  public DepartmentDataDTO() { }

  public DepartmentDataDTO(long id, DepartmentType type)
  {
    this.id = id;
    this.type = type;
  }

  public long getId() { return id; }

  public void setId(long id) { this.id = id; }

  public DepartmentType getType() { return type; }

  public void setType(DepartmentType type) { this.type = type; }

}
