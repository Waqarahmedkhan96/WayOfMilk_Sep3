package sep3.dto.userDTO;


public class UserDataDTO
{
  private String name;
  private String email;
  private String address;
  private String phone;
  private long id; //informative only
  private String role; //this is stored as a string in the database
  private String licenseNumber;

  public UserDataDTO()
  {
  }

  //added licenseNumber for the case that it's a vet class but can be null if the user is not a vet
  //any new attributes added to the user classes must be added here as well
  public UserDataDTO(String name, String email, String address, String phone,
      long id, String role, String licenseNumber)
  {
    this.name = name;
    this.email = email;
    this.address = address;
    this.phone = phone;
    this.id = id;
    this.role = role;
    this.licenseNumber = licenseNumber;
  }
  //getters and setters

  public String getLicenseNumber()
  {
    return licenseNumber;
  }

  public void setLicenseNumber(String licenseNumber)
  {
    this.licenseNumber = licenseNumber;
  }

  public long getId()
  {
    return id;
  }

  public void setId(long id)
  {
    this.id = id;
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

  public String getEmail()
  {
    return email;
  }

  public void setEmail(String email)
  {
    this.email = email;
  }

  public String getName()
  {
    return name;
  }

  public void setName(String name)
  {
    this.name = name;
  }

  public String getRole()
  {
    return role;
  }
  public void setRole(String role)
  {
    this.role = role;
  }
}