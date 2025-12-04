package sep3.entity;

public class Worker extends User
{
  protected Worker()
  {
    super();
  }

  public Worker(String name, String email, String phone, String address, String password)
  {
    super(name, email, phone, address, password);
  }

  //getters and setters already in parent class
}
