package sep3.entity.user;

import jakarta.persistence.DiscriminatorValue;
import jakarta.persistence.Entity;

@Entity
@DiscriminatorValue("VET")
public class Vet extends User
{
  private String licenseNumber;
  //added for experimenting with differences between children

  protected Vet()
  {
    super();
  }

  public Vet(String name, String email, String phone, String address, String password, String licenseNumber) {
    // 1. Call parent
    super(name, email, phone, address, password, UserRole.VET);

    // null check only if we have time to play with this some more. currently it's fine if it can have a null
//    if (licenseNumber == null || licenseNumber.isBlank()) {
//      throw new IllegalArgumentException("Vets must have a license number");
//    }
    this.licenseNumber = licenseNumber;
  }

  //getter and setter for new attribute
  public void setLicenseNumber(String authno)
  {
    this.licenseNumber = authno;
  }

  public String getLicenseNumber()
  {
    return licenseNumber;
  }

}
