package sep3.mapping;

import sep3.entity.Department;
import sep3.dto.departmentDTO.DepartmentDataDTO;

public class DepartmentMapper
{
    private DepartmentMapper() { }

    public static DepartmentDataDTO convertDepartmentToDto(Department department) {

        DepartmentDataDTO dto = new DepartmentDataDTO();
        dto.setId(department.getId());
        dto.setType(department.getType());   // ! department.setType(DepartmentType.valueOf(proto.getType()));


        dto.setCows(null);                          // !!!
        dto.setTransferRecordsFrom(null);           // !!!  Delete if problem
        dto.setTransferRecordsTo(null);             // !!!

        return dto;
    }

}
