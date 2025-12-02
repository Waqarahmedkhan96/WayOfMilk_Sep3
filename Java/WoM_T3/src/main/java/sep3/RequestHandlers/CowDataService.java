package sep3.RequestHandlers;

import sep3.dto.CowCreationDTO;
import sep3.dto.CowDataDTO;
import sep3.entity.Cow;
import sep3.dao.CowDAO;
import sep3.dao.UserDAO;
import sep3.Mapping.CowMappper;

import org.springframework.stereotype.Service;
import sep3.entity.User;

import java.util.List;
import java.util.stream.Collectors;

// Marks this class as a Spring business service
@Service
public class CowDataService
{

  // Inject the Data Access Object (DAO) / Repository
  private final CowDAO cowDAO;
  private final UserDAO userDAO;

  // Constructor Injection (Spring automatically provides the CowDAO instance)
  public CowDataService(CowDAO cowDAO, UserDAO userDAO)
  {
    this.cowDAO = cowDAO;
    this.userDAO = userDAO;
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
  User user = userDAO.findById(cow.getRegisteredByUserId())
      .orElseThrow(() -> new IllegalArgumentException("User not found with id: " + cow.getRegisteredByUserId()));
  Cow addedCow = cowDAO.save(
    new Cow(cow.getRegNo(), cow.getBirthDate(), true, user)
  );

  return convertToDto(addedCow);
}

  //using a static mapper class to convert between entity and DTO because direct conversion is not effective everywhere
  private CowDataDTO convertToDto(Cow cow)
  {
    return CowMappper.convertCowToDto(cow);
  }



}
