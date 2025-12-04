package sep3.dao;

import org.springframework.data.jpa.repository.JpaRepository;
import sep3.entity.Department;
import sep3.entity.DepartmentType;

import java.util.Optional;

public interface DepartmentDAO extends JpaRepository<Department, Long>
{

  public Optional<Department> findByType(DepartmentType type);
}
