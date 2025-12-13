package sep3.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import sep3.entity.Cow;
import sep3.entity.Department;
import sep3.entity.DepartmentType;
import sep3.entity.TransferRecord;

import java.util.List;
import java.util.Optional;

public interface DepartmentRepository extends JpaRepository<Department, Long>
{

  List<Department> findByType(DepartmentType type);
  Optional<Department> findByName(String name);

  List<Cow> findCowsById(long deptId);
  List<TransferRecord> findTransferRecordsById(long deptId);
  
}
