package sep3.dao;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import sep3.entity.Department;
import sep3.entity.DepartmentType;

import java.util.Optional;

@Repository
public interface DepartmentDAO extends JpaRepository<Department, Long> {

    // Find by enum type (RESTING, MILKING, QUARANTINE)
    Optional<Department> findByType(DepartmentType type);
}
