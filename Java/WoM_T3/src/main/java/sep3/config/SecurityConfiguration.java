package sep3.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.security.web.SecurityFilterChain;

@Configuration
public class SecurityConfiguration {

  @Bean
  public PasswordEncoder passwordEncoder() {
    return new BCryptPasswordEncoder();
  }

    /**
        * Security configuration that effectively disables HTTP security.
     * - gRPC does not go through Spring MVC filters, so Spring Security can't protect it.
     * - We disable all HTTP security to avoid blocking health checks or future REST endpoints.
     * - gRPC auth (if needed) should be implemented via interceptors, not Spring Security.
     */
  @Bean
  public SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
      http
              .csrf().disable()
              .authorizeHttpRequests()
              .anyRequest().permitAll();

      return http.build();
  }
}