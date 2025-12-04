package sep3.RequestHandlers;

import sep3.dao.CowDAO;
import sep3.dao.DepartmentDAO;
import sep3.dao.UserDAO;
import sep3.dto.CowCreationDTO;
import sep3.dto.CowDataDTO;
import sep3.Mapping.CowMapper;
import sep3.entity.Cow;

import org.springframework.stereotype.Service;
import sep3.entity.Department;
import sep3.entity.DepartmentType;
import sep3.entity.User;

import java.util.List;
import java.util.stream.Collectors;

// Marks this class as a Spring business service
@Service public class CowDataService implements ICowDataService
{

  // Inject the Data Access Object (DAO) / Repository
  private final CowDAO cowDAO;
  private final DepartmentDAO departmentDAO;
  private final UserDAO userDAO;

  // Constructor Injection (Spring automatically provides the CowDAO instance)
  public CowDataService(CowDAO cowDAO, DepartmentDAO departmentDAO, UserDAO userDAO)
  {
    this.cowDAO = cowDAO;
    this.departmentDAO = departmentDAO;
    this.userDAO = userDAO;
  }

  //CREATE/ADD
  public CowDataDTO addCow(CowCreationDTO cow)
  {
    //making sure new cows are added to a quarrantine department
    Department quarantine = departmentDAO.findByType(DepartmentType.QUARANTINE).orElseThrow(() -> new RuntimeException("Quarantine department not found"));
    User addedBy = userDAO.findById(cow.getRegisteredByUserId()).orElseThrow(() -> new RuntimeException("User not found: " + cow.getRegisteredByUserId()));
    Cow addedCow = cowDAO.save(new Cow(cow.getRegNo(), cow.getBirthDate(), quarantine, addedBy));
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
    //create the department object to be attached to the cow
    // FIX START: Only fetch department if the DTO actually has a new Department ID
    Department department = null;
    if (changesToCow.getDepartmentId() != null) {
      department = departmentDAO.findById(changesToCow.getDepartmentId())
          .orElseThrow(() -> new RuntimeException("Department not found"));
    }

    //making sure everything is mapped safe to update
    CowMapper.updateCowFromDto(cowToUpdate, changesToCow, department);

    //now we can safely update the cow
    Cow savedCow = cowDAO.save(cowToUpdate);
    //since this cow will already have an ID, JPA will automatically look for the corresponding cow
    // and perform the UPDATE operation instead of creating a new one
    //ahh, convenience
    return CowMapper.convertCowToDto(savedCow);

  }

  //update cow health specifically (t2 must permit this to vet only)
  @Override
  public CowDataDTO updateCowHealth(CowDataDTO cow)
  {
    //check if the health status is declared
    if (cow.isHealthy() == null)
    {
      throw new RuntimeException("Health status must be declared.");
    }
    Cow cowToUpdate = cowDAO.findById(cow.getId())
        .orElseThrow(() -> new RuntimeException("Cow not found: " + cow.getId()));
    cowToUpdate.setHealthy(cow.isHealthy());
    Cow savedCow = cowDAO.save(cowToUpdate);
    return CowMapper.convertCowToDto(savedCow);
  }

  //DELETE
  public void deleteCow(long id)
  {
    cowDAO.deleteById(id);
  }

  public CowDataDTO getCowById(long cowToFindId)
  {
    Cow foundCow = cowDAO.findById(cowToFindId)
        .orElseThrow(() -> new RuntimeException("Cow not found: " + cowToFindId));
    return CowMapper.convertCowToDto(foundCow);
  }
}
