package sep3.entity;

import jakarta.persistence.*;
import java.time.LocalDate;
import java.util.List;

@Entity
public class Cow
{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    private String regNo;
    private LocalDate birthDate;
    private boolean isHealthy;

    @ManyToOne
    private User registeredBy;

    @OneToMany(mappedBy = "cow")
    private List<TransferRecord> transferRecords;

    @OneToMany(mappedBy = "cow")
    private List<Milk> milk;

    protected Cow() {}

    public Cow(String regNo, LocalDate birthDate, boolean isHealthy, User registeredBy)
    {
        this.regNo = regNo;
        this.birthDate = birthDate;
        this.isHealthy = isHealthy;
        this.registeredBy = registeredBy;
    }


  //getters and setters

  public long getId()
  {
    return id;
  }

  public void setId(long id)
  {
    this.id = id;
  }

  public String getRegNo()
  {
    return regNo;
  }

  public void setRegNo(String regNo)
  {
    this.regNo = regNo;
  }

  public LocalDate getBirthDate()
  {
    return birthDate;
  }

  public void setBirthDate(LocalDate birthDate)
  {
    this.birthDate = birthDate;
  }

  public boolean isHealthy()
  {
    return isHealthy;
  }

  public void setHealthy(boolean healthy)
  {
    isHealthy = healthy;
  }

  public User getRegisteredBy()
  {
    return registeredBy;
  }
  
  public void setRegisteredBy(User registeredBy)
  {
    this.registeredBy = registeredBy;
  }
}
