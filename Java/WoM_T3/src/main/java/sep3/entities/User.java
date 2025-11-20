package sep3.entities;

import jakarta.persistence.*;

@Entity
// Specify the inheritance strategy
@Inheritance(strategy = InheritanceType.JOINED)
public abstract class User
{
  @Id @GeneratedValue(strategy = GenerationType.IDENTITY) private long id;
  private String name;
  private String email;
  private String phone;
  private String address;
  private String password;
  //TODO: make sure to add propper security (hashing) after security is taught

  protected User()
  {
  }

  public User(String name, String email, String phone, String address,
      String password)
  {
    this.name = name;
    this.email = email;
    this.phone = phone;
    this.address = address;
    this.password = password;
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
    this.password = password;
  }
}
