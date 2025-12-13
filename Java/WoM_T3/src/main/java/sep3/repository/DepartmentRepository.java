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

    @Query("SELECT d.cows FROM Department d WHERE d.id = :id")
    List<Cow> findCowsByDepartmentId(@Param("id") long id);

    @Query("""
       SELECT tr FROM TransferRecord tr
       WHERE tr.fromDept.id = :id OR tr.toDept.id = :id
       """)
    List<TransferRecord> findTransferRecordsByDepartmentId(@Param("id") long id);


}
