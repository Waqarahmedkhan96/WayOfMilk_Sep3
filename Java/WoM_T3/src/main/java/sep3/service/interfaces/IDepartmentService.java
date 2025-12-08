package sep3.service.interfaces;

import sep3.dto.departmentDTO.DepartmentCreationDTO;
import sep3.dto.departmentDTO.DepartmentDataDTO;
import sep3.entity.DepartmentType;

import java.util.List;

public interface IDepartmentService
{
    DepartmentDataDTO addDepartment(DepartmentCreationDTO department);
    List<DepartmentDataDTO> getAllDepartments();
    DepartmentDataDTO getDepartmentById(long departmentId);
    List<DepartmentDataDTO> getDepartmentsByType(DepartmentType type);
    DepartmentDataDTO updateDepartment(DepartmentDataDTO department);
    void deleteDepartment(long departmentId);
}
