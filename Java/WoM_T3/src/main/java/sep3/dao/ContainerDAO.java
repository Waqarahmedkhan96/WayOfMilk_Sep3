package sep3.dao;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import sep3.entity.Container;

@Repository
public interface ContainerDAO extends JpaRepository<Container, Long> {
    // no extra methods needed for now
}
