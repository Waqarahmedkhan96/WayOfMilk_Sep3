package sep3.RequestHandlers;

import sep3.DTOs.CowCreationDTO;
import sep3.DTOs.CowInfoDTO;
import sep3.entities.Cow;
import sep3.DAOs.CowDAO;
import sep3.Mapping.CowMappper;

import org.springframework.stereotype.Service;
import java.util.List;
import java.util.stream.Collectors;

// Marks this class as a Spring business service
@Service
public class CowInfoService
{

  // Inject the Data Access Object (DAO) / Repository
  private final CowDAO cowDAO;

  // Constructor Injection (Spring automatically provides the CowDAO instance)
  public CowInfoService(CowDAO cowDAO)
  {
    this.cowDAO = cowDAO;
  }

  /**
   * Retrieves all Cow entities from the database and maps them to DTOs.
   * This is the public contract of the T3 (Core Server).
   *
   * @return A list of CowInfoDTO objects.
   */
  public List<CowInfoDTO> getAllCows()
  {

    // 1. Fetch all entities from the PostgreSQL database
    List<Cow> cows = cowDAO.findAll();

    // 2. Map the list of Cow Entities (database objects) to
    //    CowInfoDTO DTOs (data transfer objects).
    return cows.stream()
        .map(this::convertToDto) // Use the private helper method
        .collect(Collectors.toList());
  }

  public CowInfoDTO addCow(CowCreationDTO cow)
  {
    Cow addedCow = cowDAO.save(new Cow(cow.getRegNo(), cow.getBirthDate()));
    return convertToDto(addedCow);
  }

  //using a static mapper class to convert between entity and DTO because direct conversion is not effective everywhere
  private CowInfoDTO convertToDto(Cow cow)
  {
    return CowMappper.convertCowToDto(cow);
  }

}
