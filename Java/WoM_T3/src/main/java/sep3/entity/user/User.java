package sep3.entity.user;

import jakarta.persistence.*;

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
  private String hashedPassword;
  //database will only retain the hashed version of the password
  @Enumerated(EnumType.STRING) // Always use String for Enums!
  // (the default is integers and that can cause problems if new roles are added in the future)
  private UserRole role;
  //since we have the enum, we can still use it for role management,
  // but we'll force the children to declare their own roles upon creation

  //TODO think of security since true password will be parsed between servers to get here so maybe hash somewhere earlier on the way?
  //TLSCert?

  protected User()
  {
  }

  //for creating a new user
  public User(String name, String email, String phone, String address,
      String rawPassword, UserRole role)
  {
    this.name = name;
    this.email = email;
    this.phone = phone;
    this.address = address;
    this.hashedPassword = hashPassword(rawPassword);
    this.role = role;
  }

  //constructor for authentication only
  public User(String email, String rawPassword)
  {
    this.email = email;
    checkPassword(rawPassword);
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
    return hashedPassword;
  }

  public void setPassword(String rawPassword)
  {
    this.hashedPassword = hashPassword(rawPassword);
  }

  public UserRole getRole()
  {
    return role;
  }

  //HELPERS for password hashing

  private String hashPassword(String password)
  {
    return Integer.toString(password.hashCode());
  }

  public boolean checkPassword(String rawPassword) {
    // Compare 'this.hashedPassword' (DB) with 'hashPassword(raw)' (Input)
    if (!this.hashedPassword.equals(hashPassword(rawPassword))) {
      throw new RuntimeException("Invalid password");
    }
    return true;
  }
}
