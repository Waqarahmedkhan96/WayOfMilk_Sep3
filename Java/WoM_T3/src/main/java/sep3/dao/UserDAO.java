package sep3.dao;

import sep3.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository public interface UserDAO extends JpaRepository<User, Long>
{
}
