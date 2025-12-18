package sep3.config;

import org.springframework.boot.CommandLineRunner;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.crypto.password.PasswordEncoder;
import sep3.entity.Department;
import sep3.entity.DepartmentType;
import sep3.entity.user.Owner;
import sep3.entity.user.UserRole;
import sep3.repository.DepartmentRepository;
import sep3.repository.UserRepository;

@Configuration
public class DatabaseConfiguration
{
  // NEW: Seed one default OWNER user if none exists
  @Bean CommandLineRunner seedOwner(UserRepository userRepository, PasswordEncoder passwordEncoder) {
    return args -> {
      long ownerCount = userRepository.countByRole(UserRole.OWNER); // count owners

      if (ownerCount == 0) { // seed once
        Owner owner = new Owner();
        owner.setRole(UserRole.OWNER); // owner role
        owner.setName("Admin");
        owner.setEmail("admin@gmail.com");
        owner.setPhone("00000000");
        owner.setAddress("Admin Street 00");
        owner.setPassword(passwordEncoder.encode("1234")); // hashed pass since we do this directly in the DB

        userRepository.save(owner);
        System.out.println(">>> Seeded default OWNER: admin@gmail.com / 1234");
      }
    };
  }

  @Bean
  CommandLineRunner seedQuarantineDepartment(
      DepartmentRepository departmentRepository)
  {
    return args ->{
      long departmentCount = departmentRepository.count();
      if(departmentCount == 0)
      {
        departmentRepository.save(new Department(DepartmentType.QUARANTINE, "Quarantine"));
        System.out.println(">>> Seeded default QUARANTINE department");
      }
    };
  }
}
