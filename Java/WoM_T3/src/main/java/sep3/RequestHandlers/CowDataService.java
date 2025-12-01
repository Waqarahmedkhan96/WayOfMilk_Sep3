package sep3.RequestHandlers;

import sep3.DTOs.CowCreationDTO;
import sep3.DTOs.CowDataDTO;
import sep3.entities.Cow;
import sep3.DAOs.CowDAO;
import sep3.Mapping.CowMapper;

import org.springframework.stereotype.Service;
import java.util.List;
import java.util.stream.Collectors;

// Marks this class as a Spring business service
@Service public class CowDataService implements ICowDataService
{

  // Inject the Data Access Object (DAO) / Repository
  private final CowDAO cowDAO;

  // Constructor Injection (Spring automatically provides the CowDAO instance)
  public CowDataService(CowDAO cowDAO)
  {
    this.cowDAO = cowDAO;
  }

  //CREATE/ADD
  public CowDataDTO addCow(CowCreationDTO cow)
  {
    Cow addedCow = cowDAO.save(new Cow(cow.getRegNo(), cow.getBirthDate()));
    return CowMapper.convertCowToDto(addedCow);

    //JPA is smart and performs the INSERT operation here since the cow object doesn't have an ID yet
  }

  //READ/GET
  public List<CowDataDTO> getAllCows()
  {
    // 1. Fetch all entities from the PostgreSQL database
    List<Cow> cows = cowDAO.findAll();

    // 2. Map the list of Cow Entities (database objects) to
    //    CowDataDTO DTOs (data transfer objects).
    return cows.stream()
        .map(CowMapper::convertCowToDto) // Use the private helper method
        .collect(Collectors.toList());
  }

  //UPDATE

  public CowDataDTO updateCow(CowDataDTO changesToCow)
  {
    Cow cowToUpdate = cowDAO.findById(changesToCow.getId()).orElseThrow(() -> new RuntimeException("Cow not found: " + changesToCow.getId()));
    //with throw because findbyid returns an optional, so we need to handle the case where the cow isn't found

    //making sure everything is mapped safe to update
    CowMapper.updateCowFromDto(cowToUpdate, changesToCow);

    //now we can safely update the cow
    Cow savedCow = cowDAO.save(cowToUpdate);
    //since this cow will already have an ID, JPA will automatically look for the corresponding cow
    // and perform the UPDATE operation instead
    //ahh, convenience
    return CowMapper.convertCowToDto(savedCow);

  }

  //DELETE
  public void deleteCow(long id)
  {
    cowDAO.deleteById(id);
  }

  public CowDataDTO getCowById(long cowToFindId)
  {
    Cow foundCow = cowDAO.findById(cowToFindId).orElseThrow(() -> new RuntimeException("Cow not found: " + cowToFindId));
    return CowMapper.convertCowToDto(foundCow);
  }
}
