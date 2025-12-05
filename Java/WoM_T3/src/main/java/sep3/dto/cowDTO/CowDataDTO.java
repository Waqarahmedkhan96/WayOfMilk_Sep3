package sep3.dto.cowDTO;


import java.time.LocalDate;

public class CowDataDTO
{
  private Long id;
  private String regNo;
  private LocalDate birthDate;
  private Boolean isHealthy;
  private Long departmentId;

  //using Boolean instead of boolean and Long instead of long to allow null values
  //and avoid conversion errors
  public CowDataDTO()
  {
  }

  public CowDataDTO(Long id, String regNo, LocalDate birthDate, Boolean isHealthy, Long departmentId)
  {
    this.id = id;
    this.regNo = regNo;
    this.birthDate = birthDate;
    this.isHealthy = isHealthy;
    this.departmentId = departmentId;
  }

  //Getters
  public Long getId()
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

  public Boolean isHealthy()
  {
    return isHealthy;
  }

  public Long getDepartmentId()
  {
    return departmentId;
  }

  //Setters

  public void setId(Long id)
  {
    this.id = id;
  }

  public void setRegNo(String regNo)
  {
    this.regNo = regNo;
  }

  public void setBirthDate(LocalDate birthDate)
  {
    this.birthDate = birthDate;
  }

  public void setHealthy(Boolean healthy)
  {
    isHealthy = healthy;
  }

  public void setDepartmentId(Long departmentId)
  {
    this.departmentId = departmentId;
  }
}

