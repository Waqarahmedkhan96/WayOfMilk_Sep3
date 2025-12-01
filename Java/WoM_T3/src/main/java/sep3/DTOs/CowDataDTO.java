package sep3.DTOs;


import java.time.LocalDate;

public class CowDataDTO
{
  private long id;
  private String regNo;
  private LocalDate birthDate;
  private boolean isHealthy;

  public CowDataDTO()
  {
  }

  public CowDataDTO(long id, String regNo, LocalDate birthDate, boolean isHealthy)
  {
    this.id = id;
    this.regNo = regNo;
    this.birthDate = birthDate;
    this.isHealthy = isHealthy;
  }

  //Getters only for now, since it's an information retrieval dto
  public long getId()
  {
    return id;
  }

  public String getRegNo()
  {
    return regNo;
  }

  public LocalDate getBirthDate()
  {
    return birthDate;
  }

  public boolean isHealthy()
  {
    return isHealthy;
  }
}

