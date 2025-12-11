package sep3.repository;

import sep3.entity.user.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import sep3.entity.user.UserRole;

import java.util.List;
import java.util.Optional;

@Repository public interface UserRepository extends JpaRepository<User, Long>
{
  // For Login: Finds a user by email so you can check their password
  // Returns Optional because the user might not exist
  Optional<User> findByEmail(String email);

  // For Registration: Checks if an email is already taken
  // Returns true/false, very fast database check
  boolean existsByEmail(String email);

  // Finds all users of a specific role
  // Example usage: userDAO.findAllByRole(UserRole.VET)
  List<User> findAllByRole(UserRole role);

  //Find user(s) by name
  List<User> findByNameContainingIgnoreCase(String name);

    // NEW: count users by role (used to see if an OWNER exists)
    long countByRole(UserRole role);
}
