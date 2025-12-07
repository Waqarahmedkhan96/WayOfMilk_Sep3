package sep3.service.impl;

import org.springframework.stereotype.Service;
import sep3.mapping.DepartmentMapper;
import sep3.repository.DepartmentRepository;
import sep3.dto.departmentDTO.DepartmentCreationDTO;
import sep3.dto.departmentDTO.DepartmentDataDTO;
import sep3.entity.Department;
import sep3.entity.DepartmentType;
import sep3.service.interfaces.IDepartmentService;

import java.util.List;
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
    public DepartmentDataDTO addDepartment(DepartmentCreationDTO request)
    {
        if (request.getType() == null)
        {
            throw new IllegalArgumentException("Department type must be provided.");
        }

        Department department = new Department(request.getType());
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
    public DepartmentDataDTO getDepartmentByType(DepartmentType type)
    {
        Department department = departmentRepository.findByType(type)
                .orElseThrow(() -> new IllegalArgumentException("Department not found type: " + type));

        return DepartmentMapper.convertDepartmentToDto(department);
    }

    @Override
    public DepartmentDataDTO updateDepartment(DepartmentDataDTO request)
    {
        if (request.getId() == null)
        {
            throw new IllegalArgumentException("Department ID must be provided for update.");
        }

        long id = request.getId();

        Department departmentToUpdate = departmentRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Department not found: " + id));

        if (request.getType() != null)
        {
            departmentToUpdate.setType(request.getType());
        }

        Department updated = departmentRepository.save(departmentToUpdate);
        return DepartmentMapper.convertDepartmentToDto(updated);
    }


    @Override
    public void deleteDepartment(long departmentId)
    {
        departmentRepository.deleteById(departmentId);
    }
}
