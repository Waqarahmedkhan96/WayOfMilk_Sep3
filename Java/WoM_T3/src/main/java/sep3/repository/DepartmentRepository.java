package sep3.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import sep3.entity.Department;
import sep3.entity.DepartmentType;

import java.util.List;

public interface DepartmentRepository extends JpaRepository<Department, Long>
{

  List<Department> findByType(DepartmentType type);

}
