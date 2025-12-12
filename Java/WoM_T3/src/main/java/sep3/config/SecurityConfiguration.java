package sep3.config;

import org.springframework.boot.CommandLineRunner;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;
import sep3.entity.Department;
import sep3.entity.DepartmentType;
import sep3.entity.user.Owner;
import sep3.entity.user.UserRole;
import sep3.repository.DepartmentRepository;
import sep3.repository.UserRepository;

@Configuration
public class SecurityConfiguration {

    @Bean
    public PasswordEncoder passwordEncoder() {
        return new BCryptPasswordEncoder();
    }

    /**
     * Security configuration for HTTP was disabled because
     * gRPC does not run through Spring Security filters.
     * (kept commented as before)
     */
    //  @Bean
    //  public SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
    //      http
    //              .csrf().disable()
    //              .authorizeHttpRequests()
    //              .anyRequest().permitAll();
    //
    //      return http.build();
    //  }

    // NEW: Seed one default OWNER user if none exists
    @Bean
    CommandLineRunner seedOwner(UserRepository userRepository, PasswordEncoder passwordEncoder) {
        return args -> {
            long ownerCount = userRepository.countByRole(UserRole.OWNER); // count owners

            if (ownerCount == 0) { // seed once
                Owner owner = new Owner();
                owner.setRole(UserRole.OWNER); // owner role
                owner.setName("Waqar");
                owner.setEmail("Waqar@gmail.com");
                owner.setPhone("00000000");
                owner.setAddress("Vejle");
                owner.setPassword(passwordEncoder.encode("12345")); // hashed pass

                userRepository.save(owner);
                System.out.println(">>> Seeded default OWNER: Waqar@gmail.com / 12345");
            }
        };
    }

    @Bean
    CommandLineRunner seedQuarantineDepartment(DepartmentRepository departmentRepository)
    {
      return args ->{
        long departmentCount = departmentRepository.count();
        if(departmentCount == 0)
        {
          departmentRepository.save(new Department(DepartmentType.QUARANTINE));
          System.out.println(">>> Seeded default QUARANTINE department");
        }
    };
  }
}
