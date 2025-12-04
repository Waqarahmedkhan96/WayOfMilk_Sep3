package sep3.dto;

public class UserCreationDTO
{
  private String name;
  private String email;
  private String phone;
  private String address;
  private String password;
  private String role;
  private String licenseNumber;

  public UserCreationDTO()
  {
  }

  //very general constructor, used for all user types
  //if any new attributes are added to any of the user-related classes, they must be added here as well
  public UserCreationDTO(String name, String email, String phone, String address, String password, String role, String licenseNumber)
  {
    this.name = name;
    this.email = email;
    this.phone = phone;
    this.address = address;
    this.password = password;
    this.role = role;
    this.licenseNumber = licenseNumber;
  }

  //getters and setters

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

  public String getPassword()
  {
    return password;
  }

  public void setPassword(String password)
  {
    this.password = password;
  }

  public String getRole()
  {
    return role;
  }

  public void setRole(String role)
  {
    this.role = role;
  }

  public String getLicenseNumber()
  {
    return licenseNumber;
  }

  public void setLicenseNumber(String licenseNumber)
  {
    this.licenseNumber = licenseNumber;
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
}
