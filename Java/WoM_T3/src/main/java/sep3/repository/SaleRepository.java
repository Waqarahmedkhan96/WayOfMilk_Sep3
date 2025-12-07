package sep3.dao;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import sep3.entity.Sale;
import sep3.entity.user.User;

import java.time.LocalDateTime;
import java.util.List;

@Repository
public interface SaleRepository extends JpaRepository<Sale, Long> {

    // All sales created by a specific user (e.g. for reports)
    List<Sale> findAllByCreatedBy(User createdBy);

    // Same but by id, in case you only have the id
    List<Sale> findAllByCreatedById(Long userId);

    // Filter by period (for statistics / reports)
    List<Sale> findAllByDateTimeBetween(LocalDateTime from, LocalDateTime to);

    // Find all sales that are recall cases
    List<Sale> findAllByRecallCaseTrue();
}
