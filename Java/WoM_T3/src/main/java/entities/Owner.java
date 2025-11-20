package entities;

//no annotations needed in this class, since the inheritance was already
// declared in the parent class and jpa is smart enough to figure it out
//aaaah, convenience
public class Owner extends User
{

  protected Owner()
  {
    super();
  }

  public Owner(String name, String email, String phone, String address, String password)
  {
    super(name, email, phone, address, password);
  }

  //getters and setters already in parent class

}
