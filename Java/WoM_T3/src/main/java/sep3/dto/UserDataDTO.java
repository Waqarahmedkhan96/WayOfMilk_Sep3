package sep3.dto;


public class UserDataDTO
{
  private String name;
  private String email;
  private String address;
  private String phone;
  private long id;
  //the id has only informative value, this dto is not used to create or update employees

  public UserDataDTO()
  {
  }

  public UserDataDTO(String name, String email, String address, String phone, long id)
  {
    this.name = name;
    this.email = email;
    this.address = address;
    this.phone = phone;
    this.id = id;
  }



}