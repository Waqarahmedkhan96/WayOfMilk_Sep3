package sep3.dao;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import sep3.entity.Customer;

import java.util.List;
import java.util.Optional;

@Repository
public interface CustomerRepository extends JpaRepository<Customer, Long> {

    // For checking uniqueness of company CVR during registration / creation
    boolean existsByCompanyCVR(String companyCVR);

    // For quickly fetching one specific customer (e.g. for a sale)
    Optional<Customer> findByCompanyCVR(String companyCVR);

    // For searching customers by name in UI / filters
    List<Customer> findByCompanyNameContainingIgnoreCase(String companyName);

    // Optional: search by email
    Optional<Customer> findByEmail(String email);
}
