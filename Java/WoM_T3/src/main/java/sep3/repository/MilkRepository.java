package sep3.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import sep3.entity.Milk;
import sep3.entity.Container;
import sep3.entity.Cow;

import java.util.List;

@Repository
public interface MilkRepository extends JpaRepository<Milk, Long> {

    // FindByContainer
    List<Milk> findByContainer(Container container);

    // FindByCow
    List<Milk> findByCow(Cow cow);
}
