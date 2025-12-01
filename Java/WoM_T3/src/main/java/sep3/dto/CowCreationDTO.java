package sep3.dto;

import java.time.LocalDate;

public class CowCreationDTO
{
  private String regNo;
  private LocalDate birthDate;
  // We can omit 'isHealthy', as the client won't send it on creation,
  // or the Service layer might enforce the initial 'false' value.
  private long registeredByUserId;


  public CowCreationDTO() {
  }

  public CowCreationDTO(String regNo, LocalDate birthDate, long registeredByUserId) {
    this.regNo = regNo;
    this.birthDate = birthDate;
    this.registeredByUserId = registeredByUserId;
  }

  // --- Getters and Setters (Mandatory for JSON conversion) ---
  public String getRegNo() {
    return regNo;
  }

  public void setRegNo(String regNo) {
    this.regNo = regNo;
  }

  public LocalDate getBirthDate() {
    return birthDate;
  }

  public void setBirthDate(LocalDate birthDate) {
    this.birthDate = birthDate;
  }

  public long getRegisteredByUserId() {
    return registeredByUserId;
  }

  public void setRegisteredByUserId(long registeredByUserId) {
    this.registeredByUserId = registeredByUserId;
  }
}
