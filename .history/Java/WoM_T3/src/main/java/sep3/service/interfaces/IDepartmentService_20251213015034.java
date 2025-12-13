package sep3.service.interfaces;

import sep3.dto.cowDTO.CowDataDTO;
import sep3.dto.departmentDTO.DepartmentCreationDTO;
import sep3.dto.departmentDTO.DepartmentDataDTO;
import sep3.dto.transferRecordDTO.TransferRecordDataDTO;
import sep3.entity.DepartmentType;

import java.util.List;

public interface IDepartmentService
{
    DepartmentDataDTO addDepartment(DepartmentCreationDTO department);
    List<DepartmentDataDTO> getAllDepartments();
    DepartmentDataDTO getDepartmentById(long departmentId);
    List<DepartmentDataDTO> getDepartmentsByType(DepartmentType type);
    DepartmentDataDTO getDepartmentByName(String name);
    DepartmentDataDTO updateDepartment(DepartmentDataDTO department);
    void deleteDepartment(long departmentId);
    List<CowDataDTO> getCowsByDepartment(long deptId);
    List<TransferRecordDataDTO> getTransferRecordsByDepartment(long deptId);

}
