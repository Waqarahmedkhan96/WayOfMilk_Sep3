package sep3.entity.user;

// no annotations needed in this class, since the inheritance was already
// declared in the parent class and jpa is smart enough to figure it out

import jakarta.persistence.DiscriminatorValue;
import jakarta.persistence.Entity;

@Entity // "Hey Hibernate, this class maps to a database row"
@DiscriminatorValue("OWNER") // "If the column 'user_type' says 'OWNER', make this object"
public class Owner extends User
{

  protected Owner()
  {
    super();
  }

  public Owner(String name, String email, String phone, String address, String password)
  {
    // We pass UserRole.OWNER to the parent.
    // An Owner object can simply NEVER have a different role.
    super(name, email, phone, address, password, UserRole.OWNER);
  }

  // getters and setters already in parent class

}
