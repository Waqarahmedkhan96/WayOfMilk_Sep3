package sep3.entity.user;

import jakarta.persistence.*;
import java.util.ArrayList;
import java.util.List;
import sep3.entity.Customer;

@Entity
//"user" is a reserved keyword in PostgreSQL.
// We must explicitly name the table "users" (plural) to avoid the syntax error.
@Table(name = "users")
// Specify the inheritance strategy
@Inheritance(strategy = InheritanceType.SINGLE_TABLE)
//switched from JOINED to SINGLE_TABLE because joined would have made separate tables for each child class
@DiscriminatorColumn(name = "user_type")
//this will be used to determine the type of user
public abstract class User
{
  @Id @GeneratedValue(strategy = GenerationType.IDENTITY) private long id;
  private String name;
  private String email;
  private String phone;
  private String address;
    // 1 User .. * Customers (customers registered by this user)
    @OneToMany(mappedBy = "registeredBy", cascade = CascadeType.ALL)
    private List<Customer> customers = new ArrayList<>();


    //private String hashedPassword;
  //database will only retain the hashed version of the password
  //now handling the hashing via bcrypt configuration from the service layer
  private String password;
  @Enumerated(EnumType.STRING) // Always use String for Enums!
  // (the default is integers and that can cause problems if new roles are added in the future)
  private UserRole role;
  //since we have the enum, we can still use it for role management,
  // but we'll force the children to declare their own roles upon creation


  protected User()
  {
  }

  //for creating a new user
  public User(String name, String email, String phone, String address,
      String password, UserRole role)
  {
    this.name = name;
    this.email = email;
    this.phone = phone;
    this.address = address;
    this.password = password;
    //this.hashedPassword = hashPassword(rawPassword);
    this.role = role;
    this.customers = new ArrayList<>();
  }

  //constructor for authentication only
  public User(String email, String rawPassword)
  {
    this.email = email;
    this.password = rawPassword;
  }

  //getters and setters here unless we add any new attributes in child classes

  public long getId()
  {
    return id;
  }

  public void setId(long id)
  {
    this.id = id;
  }

  public String getName()
  {
    return name;
  }

  public void setName(String name)
  {
    this.name = name;
  }

  public String getEmail()
  {
    return email;
  }

  public void setEmail(String email)
  {
    this.email = email;
  }

  public String getPhone()
  {
    return phone;
  }

  public void setPhone(String phone)
  {
    this.phone = phone;
  }

  public String getAddress()
  {
    return address;
  }

  public void setAddress(String address)
  {
    this.address = address;
  }

  public String getPassword()
  {
    return password;
  }

  public void setPassword(String password)
  {
    //this.hashedPassword = hashPassword(rawPassword);
    this.password = password;
  }

  // User List methods:
  public List<Customer> getCustomers() {
      return customers;
  }

  public void addCustomer(Customer customer) {
      customers.add(customer);
  }

    public Object getRole() {
      return role;
    }

    //HELPERS for password hashing
  //no longer needed
  //
  //  private String hashPassword(String password)
  //  {
  //    return Integer.toString(password.hashCode());
  //  }
  //
  //  public boolean checkPassword(String rawPassword) {
  //    // Compare 'this.hashedPassword' (DB) with 'hashPassword(raw)' (Input)
  //    if (!this.hashedPassword.equals(hashPassword(rawPassword))) {
  //      throw new RuntimeException("Invalid password");
  //    }
  //    return true;
  //  }

}
