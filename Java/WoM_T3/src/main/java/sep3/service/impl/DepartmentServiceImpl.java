package sep3.service.impl;

import org.springframework.stereotype.Service;
import sep3.dto.cowDTO.CowDataDTO;
import sep3.dto.transferRecordDTO.TransferRecordDataDTO;
import sep3.mapping.CowMapper;
import sep3.mapping.DepartmentMapper;
import sep3.mapping.TransferRecordMapper;
import sep3.repository.DepartmentRepository;
import sep3.dto.departmentDTO.DepartmentCreationDTO;
import sep3.dto.departmentDTO.DepartmentDataDTO;
import sep3.entity.Department;
import sep3.entity.DepartmentType;
import sep3.service.interfaces.IDepartmentService;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

@Service
public class DepartmentServiceImpl implements IDepartmentService
{
    private final DepartmentRepository departmentRepository;

    public DepartmentServiceImpl(DepartmentRepository departmentRepository)
    {
        this.departmentRepository = departmentRepository;
    }

    @Override
    public DepartmentDataDTO addDepartment(DepartmentCreationDTO dto) {

        Department department = new Department(dto.getType(), dto.getName());
        Department saved = departmentRepository.save(department);

        return DepartmentMapper.convertDepartmentToDto(saved);
    }


    @Override
    public List<DepartmentDataDTO> getAllDepartments()
    {
        return departmentRepository.findAll()
                .stream()
                .map(DepartmentMapper::convertDepartmentToDto)
                .collect(Collectors.toList());
    }

    @Override
    public DepartmentDataDTO getDepartmentById(long departmentId)
    {
        Department department = departmentRepository.findById(departmentId)
                .orElseThrow(() -> new IllegalArgumentException("Department not found id: " + departmentId));

        return DepartmentMapper.convertDepartmentToDto(department);
    }

    @Override
    public List<DepartmentDataDTO> getDepartmentsByType(DepartmentType type)
    {
        return departmentRepository.findByType(type).stream()
                .map(DepartmentMapper::convertDepartmentToDto)
                .toList();
    }

    @Override
    public DepartmentDataDTO getDepartmentByName(String name)
    {
        Optional<Department> department = departmentRepository.findByName(name);
        if (department.isEmpty())
        {
            throw new IllegalArgumentException("Department not found name: " + name);
        }
        return DepartmentMapper.convertDepartmentToDto(department.get());
    }

    @Override
    public DepartmentDataDTO updateDepartment(DepartmentDataDTO request)
    {
        if (request.getId() == null)
            throw new IllegalArgumentException("ID required");

        Department dept = departmentRepository.findById(request.getId())
                .orElseThrow(() -> new RuntimeException("Department not found: " + request.getId()));

        if (request.getType() != null)

            dept.setType(request.getType());
            dept.setName(request.getName());


        Department saved = departmentRepository.save(dept);
        return DepartmentMapper.convertDepartmentToDto(saved);
    }

    @Override
    public void deleteDepartment(long departmentId)
    {
        departmentRepository.deleteById(departmentId);
    }

    @Override
    public List<CowDataDTO> getCowsByDepartment(long deptId) {
        return departmentRepository.findCowsByDepartmentId(deptId).stream()
                .map(CowMapper::convertCowToDto)
                .toList();
    }

    @Override
    public List<TransferRecordDataDTO> getTransferRecordsByDepartment(long deptId) {
        return departmentRepository.findTransferRecordsByDepartmentId(deptId).stream()
                .map(TransferRecordMapper::convertTransferRecordToDto)
                .toList();
    }


}
