package sep3.service.impl;

import org.springframework.stereotype.Service;
import sep3.mapping.DepartmentMapper;
import sep3.dao.DepartmentDAO;
import sep3.dto.departmentDTO.DepartmentCreationDTO;
import sep3.dto.departmentDTO.DepartmentDataDTO;
import sep3.entity.Department;
import sep3.entity.DepartmentType;
import sep3.service.interfaces.IDepartmentDataService;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class DepartmentDataService implements IDepartmentDataService
{
    private final DepartmentDAO departmentDAO;

    public DepartmentDataService(DepartmentDAO departmentDAO)
    {
        this.departmentDAO = departmentDAO;
    }

    @Override
    public DepartmentDataDTO addDepartment(DepartmentCreationDTO request)
    {
        if (request.getType() == null)
        {
            throw new IllegalArgumentException("Department type must be provided.");
        }

        Department department = new Department(request.getType());
        Department saved = departmentDAO.save(department);
        return DepartmentMapper.convertDepartmentToDto(saved);
    }

    @Override
    public List<DepartmentDataDTO> getAllDepartments()
    {
        return departmentDAO.findAll()
                .stream()
                .map(DepartmentMapper::convertDepartmentToDto)
                .collect(Collectors.toList());
    }

    @Override
    public DepartmentDataDTO getDepartmentById(long departmentId)
    {
        Department department = departmentDAO.findById(departmentId)
                .orElseThrow(() -> new IllegalArgumentException("Department not found id: " + departmentId));

        return DepartmentMapper.convertDepartmentToDto(department);
    }

    @Override
    public DepartmentDataDTO getDepartmentByType(DepartmentType type)
    {
        Department department = departmentDAO.findByType(type)
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

        Department departmentToUpdate = departmentDAO.findById(id)
                .orElseThrow(() -> new RuntimeException("Department not found: " + id));

        if (request.getType() != null)
        {
            departmentToUpdate.setType(request.getType());
        }

        Department updated = departmentDAO.save(departmentToUpdate);
        return DepartmentMapper.convertDepartmentToDto(updated);
    }


    @Override
    public void deleteDepartment(long departmentId)
    {
        departmentDAO.deleteById(departmentId);
    }
}
