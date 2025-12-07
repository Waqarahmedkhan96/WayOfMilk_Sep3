package sep3.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import sep3.entity.Cow;

import java.util.Optional;

@Repository public interface CowRepository extends JpaRepository<Cow, Long>
{
  void deleteCowById(long id);

  Optional<Cow> findByRegNo(String regNo);

  //spring automatically generates the methods for us, but we can
  // add new ones here if needed, despite this being declared as an interfaces
}
