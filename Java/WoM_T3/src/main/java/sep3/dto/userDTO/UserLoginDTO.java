package sep3.dto.userDTO;

public class UserLoginDTO
{
  private String email;
  private String password;

  //dumb dto meant for password authentication only
  public UserLoginDTO()
  {
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
  }
}
