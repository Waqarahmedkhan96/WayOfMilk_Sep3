package sep3.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;

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


}
