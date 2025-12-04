package sep3.RequestHandlers;

import sep3.dto.DepartmentDataDTO;
import sep3.entity.DepartmentType;

import java.util.List;

public interface IDepartmentDataService
{
  List<DepartmentDataDTO> getAllDepartments();

  DepartmentDataDTO addDepartment(DepartmentType departmentType);

  DepartmentDataDTO getDepartmentById(long departmentId);

  DepartmentDataDTO getDepartmentByType(DepartmentType departmentType);
}