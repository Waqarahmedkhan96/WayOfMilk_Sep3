package sep3.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import sep3.entity.Container;

@Repository
public interface ContainerRepository extends JpaRepository<Container, Long> {
    // basic CRUD only
}
