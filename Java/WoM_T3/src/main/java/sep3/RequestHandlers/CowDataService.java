package sep3.RequestHandlers;

import sep3.DTOs.CowCreationDTO;
import sep3.DTOs.CowDataDTO;
import sep3.entities.Cow;
import sep3.DAOs.CowDAO;
import sep3.Mapping.CowMappper;

import org.springframework.stereotype.Service;
import java.util.List;
import java.util.stream.Collectors;

// Marks this class as a Spring business service
@Service
public class CowDataService
{

  // Inject the Data Access Object (DAO) / Repository
  private final CowDAO cowDAO;

  // Constructor Injection (Spring automatically provides the CowDAO instance)
  public CowDataService(CowDAO cowDAO)
  {
    this.cowDAO = cowDAO;
  }

  /**
   * Retrieves all Cow entities from the database and maps them to DTOs.
   * This is the public contract of the T3 (Core Server).
   *
   * @return A list of CowDataDTO objects.
   */
  public List<CowDataDTO> getAllCows()
  {

    // 1. Fetch all entities from the PostgreSQL database
    List<Cow> cows = cowDAO.findAll();

    // 2. Map the list of Cow Entities (database objects) to
    //    CowDataDTO DTOs (data transfer objects).
    return cows.stream()
        .map(this::convertToDto) // Use the private helper method
        .collect(Collectors.toList());
  }

  public CowDataDTO addCow(CowCreationDTO cow)
  {
    Cow addedCow = cowDAO.save(new Cow(cow.getRegNo(), cow.getBirthDate()));
    return convertToDto(addedCow);
  }

  //using a static mapper class to convert between entity and DTO because direct conversion is not effective everywhere
  private CowDataDTO convertToDto(Cow cow)
  {
    return CowMappper.convertCowToDto(cow);
  }



}
