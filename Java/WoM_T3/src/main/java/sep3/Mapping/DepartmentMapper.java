package sep3.Mapping;

import sep3.entity.Department;
import sep3.dto.DepartmentDataDTO;


public class DepartmentMapper {

  private DepartmentMapper() {}

  public static DepartmentDataDTO convertDepartmentToDto(Department department) {
    return new DepartmentDataDTO(
        department.getId(),
        department.getType()
    );
  }
}
