package sep3.entity;

import jakarta.persistence.DiscriminatorValue;
import jakarta.persistence.Entity;

@Entity
@DiscriminatorValue("WORKER")
public class Worker extends User
{
  protected Worker()
  {
    super();
  }

  public Worker(String name, String email, String phone, String address, String password)
  {
    super(name, email, phone, address, password, UserRole.WORKER);
  }

  //getters and setters already in parent class
}
