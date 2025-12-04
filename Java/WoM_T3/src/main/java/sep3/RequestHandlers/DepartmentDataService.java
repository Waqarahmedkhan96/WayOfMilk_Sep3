package sep3.RequestHandlers;

import org.springframework.stereotype.Service;
import sep3.Mapping.CowMapper;
import sep3.Mapping.DepartmentMapper;
import sep3.dao.DepartmentDAO;
import sep3.dto.DepartmentDataDTO;
import sep3.entity.Cow;
import sep3.entity.Department;
import sep3.entity.DepartmentType;

import java.util.List;
import java.util.stream.Collectors;

@Service
public class DepartmentDataService implements IDepartmentDataService {

  private final DepartmentDAO departmentDAO;

  public DepartmentDataService(DepartmentDAO departmentDAO) {
    this.departmentDAO = departmentDAO;
  }

  @Override
  public List<DepartmentDataDTO> getAllDepartments() {
    return departmentDAO.findAll()
        .stream()
        .map(DepartmentMapper::convertDepartmentToDto)
        .collect(Collectors.toList());
  }

  @Override
  public DepartmentDataDTO addDepartment(DepartmentType type) {
    Department department = new Department(type);
    Department saved = departmentDAO.save(department);
    return DepartmentMapper.convertDepartmentToDto(saved);
  }

  @Override
  public DepartmentDataDTO getDepartmentById(long departmentId) {
    Department department = departmentDAO.findById(departmentId).orElseThrow(() -> new IllegalArgumentException("Department not found id: " + departmentId));

    return DepartmentMapper.convertDepartmentToDto(department);
  }

  @Override
  public DepartmentDataDTO getDepartmentByType(DepartmentType type) {
    return departmentDAO.findAll()
        .stream()
        .filter(x -> x.getType() == type)
        .findFirst()
        .map(DepartmentMapper::convertDepartmentToDto)
        .orElseThrow(() -> new IllegalArgumentException("Department not found type: " + type));
  }
}
