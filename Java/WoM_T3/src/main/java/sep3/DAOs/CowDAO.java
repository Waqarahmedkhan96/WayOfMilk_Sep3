package sep3.DAOs;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import sep3.entities.Cow;

@Repository public interface CowDAO extends JpaRepository<Cow, Long>
{
  void deleteCowById(long id);

  //spring automatically generates the methods for us, but we can
  // add new ones here if needed, despite this being declared as an interface
}
