package sep3.entity;

import jakarta.persistence.*;
import java.util.List;

@Entity
@Table(name = "users") // "user" is a reserved keyword in some SQL databases
public class User
{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    @Enumerated(EnumType.STRING)
    private UserRole role;

    private String name;
    private String email;
    private String phoneNumber;
    private String password;

    @OneToMany(mappedBy = "registeredBy")
    private List<Cow> cows;

    @OneToMany(mappedBy = "registeredBy")
    private List<Milk> milks;

    @OneToMany(mappedBy = "registeredBy")
    private List<Customer> customers;

    @OneToMany(mappedBy = "createdBy")
    private List<Sale> sales;

    protected User() {}

    public User(UserRole role, String name, String email, String phoneNumber, String password)
    {
        this.role = role;
        this.name = name;
        this.email = email;
        this.phoneNumber = phoneNumber;
        this.password = password;
    }

    // Gettery/settery
    public long getId() { return id; }
    public UserRole getRole() { return role; }
    public void setRole(UserRole role) { this.role = role; }
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }
    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }
    public String getPhoneNumber() { return phoneNumber; }
    public void setPhoneNumber(String phoneNumber) { this.phoneNumber = phoneNumber; }
    public String getPassword() { return password; }
    public void setPassword(String password) { this.password = password; }
}
