package sep3.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import sep3.entity.Department;
import sep3.entity.DepartmentType;

import java.util.Optional;

public interface DepartmentRepository extends JpaRepository<Department, Long>
{

  public Optional<Department> findByType(DepartmentType type);
}
