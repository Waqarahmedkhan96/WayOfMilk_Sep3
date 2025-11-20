package sep3.DAOs;

import sep3.entities.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository public interface UserDAO extends JpaRepository<User, Long>
{
}
