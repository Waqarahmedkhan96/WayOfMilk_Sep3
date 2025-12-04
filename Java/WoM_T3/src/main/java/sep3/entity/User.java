package sep3.entity;

import jakarta.persistence.*;

@Entity
// Specify the inheritance strategy
@Inheritance(strategy = InheritanceType.JOINED) public abstract class User
{
  @Id @GeneratedValue(strategy = GenerationType.IDENTITY) private long id;
  private String name;
  private String email;
  private String phone;
  private String address;
  private String hashedPassword;
  //database will only retain the hashed version of the password

  //TODO think of security since true password will be parsed between servers to get here so maybe hash somewhere earlier on the way?

  protected User()
  {
  }

  //for creating a new user
  public User(String name, String email, String phone, String address,
      String rawPassword)
  {
    this.name = name;
    this.email = email;
    this.phone = phone;
    this.address = address;
    this.hashedPassword = hashPassword(rawPassword);
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

  //HELPERS for password hashing

  private String hashPassword(String password)
  {
    return Integer.toString(password.hashCode());
  }

  private void checkPassword(String password)
  {
    if (!password.equals(hashPassword(password)))
    {
      throw new RuntimeException("Invalid password");
    }
  }
}
