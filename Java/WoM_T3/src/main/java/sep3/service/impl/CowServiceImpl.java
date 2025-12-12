package sep3.service.impl;

import sep3.dto.MilkDtos;
import sep3.entity.Milk;
import sep3.mapping.MilkMapper;
import sep3.repository.CowRepository;
import sep3.repository.DepartmentRepository;
import sep3.repository.UserRepository;
import sep3.dto.cowDTO.CowCreationDTO;
import sep3.dto.cowDTO.CowDataDTO;
import sep3.mapping.CowMapper;
import sep3.entity.Cow;

import org.springframework.stereotype.Service;
import sep3.entity.Department;
import sep3.entity.DepartmentType;
import sep3.entity.user.*;
import sep3.service.interfaces.ICowService;

import java.util.List;

// Marks this class as a Spring business service
@Service public class CowServiceImpl implements ICowService
{

  // Inject the Data Access Object (DAO) / Repository
  private final CowRepository cowRepository;
  private final DepartmentRepository departmentRepository;
  private final UserRepository userRepository;

  // Constructor Injection (Spring automatically provides the CowRepository instance)
  public CowServiceImpl(CowRepository cowRepository, DepartmentRepository departmentRepository, UserRepository userRepository)
  {
    this.cowRepository = cowRepository;
    this.departmentRepository = departmentRepository;
    this.userRepository = userRepository;
  }

  //CREATE/ADD
  @Override
  public CowDataDTO addCow(CowCreationDTO cow)
  {
    //making sure new cows are added to a quarrantine department
    Department quarantine = departmentRepository.findById(cow.getDepartmentId()).orElseThrow(() -> new RuntimeException("Department not found"));
    if (quarantine == null)
    {
      //if no specific department is provided, find the first 'available' quarantine department
      //we currently have no limit to how many cows can be in a department, so this should always work
      //especially now that there's a bean that automatically gets created for us
      quarantine = departmentRepository.findByType(DepartmentType.QUARANTINE).
          stream().findFirst().orElseThrow(() -> new RuntimeException("Quarantine department not found."));
    }
    if (!quarantine.getType().equals(DepartmentType.QUARANTINE))
    {
        throw new RuntimeException("Cows can only be added to a quarantine department.");
    }
    User addedBy = userRepository.findById(cow.getRegisteredByUserId()).orElseThrow(() -> new RuntimeException("User not found: " + cow.getRegisteredByUserId()));
    Cow addedCow = cowRepository.save(new Cow(cow.getRegNo(), cow.getBirthDate(), quarantine, addedBy));
    return CowMapper.convertCowToDto(addedCow);

    //JPA is smart and performs the INSERT operation here since the cow object doesn't have an ID yet
  }

  //READ/GET
  @Override
  public CowDataDTO getCowById(long cowToFindId)
  {
    Cow foundCow = cowRepository.findById(cowToFindId)
        .orElseThrow(() -> new RuntimeException("Cow not found: " + cowToFindId));
    return CowMapper.convertCowToDto(foundCow);
  }
  @Override
  public List<CowDataDTO> getAllCows()
  {
    // 1. Fetch all entities from the PostgreSQL database
    List<Cow> cows = cowRepository.findAll();

    // 2. Map the list of Cow Entities (database objects) to
    //    CowDataDTO DTOs (data transfer objects).
      List<CowDataDTO> returnedDTOs = CowMapper.convertCowListToDTO(cows);

  //return converted list
    return returnedDTOs;
  }
  @Override
  public CowDataDTO getCowByRegNo (String regNo)
  {
    Cow foundCow = cowRepository.findByRegNo(regNo)
        .orElseThrow(() -> new RuntimeException("Cow not found with regNo: " + regNo));
    return CowMapper.convertCowToDto(foundCow);
  }
  //for maybe later use
  @Override
  public List<CowDataDTO> getCowsByDepartmentId(long departmentId)
  {
    List<Cow> cowsInDepartment = cowRepository.findByDepartmentId(departmentId);
    return CowMapper.convertCowListToDTO(cowsInDepartment);
  }

  //this one likely belongs in milkservice, but I'm leaving it here for now
  @Override
  public List<MilkDtos.MilkDto> getCowMilk(long cowId)
  {
    List<Milk> milkFromCow = cowRepository.findById(cowId)
        .orElseThrow(() -> new RuntimeException("Cow not found: " + cowId))
        .getMilk();
    return MilkMapper.toListDto(milkFromCow).getMilkRecords();
  }




  //UPDATE
  @Override
  public CowDataDTO updateCow(CowDataDTO changesToCow, long userId)
  {
    Cow cowToUpdate = cowRepository.findById(changesToCow.getId()).orElseThrow(() -> new RuntimeException("Cow not found: " + changesToCow.getId()));
    //with throw because findbyid returns an optional, so we need to handle the case where the cow isn't found
    //create the department object to be attached to the cow
    // FIX START: Only fetch department if the DTO actually has a new Department ID
    Department department = null;
    //find the user trying to perform the update
    User requester = userRepository.findById(userId).orElseThrow(() -> new RuntimeException("User not found: " + userId));
    if (changesToCow.getDepartmentId() != null) {
      department = departmentRepository.findById(changesToCow.getDepartmentId())
          .orElseThrow(() -> new RuntimeException("Department not found"));
    }

    //making sure everything is mapped safe to update
      //this will not update the health yet!
    CowMapper.updateCowFromDto(cowToUpdate, changesToCow, department);
  //check if the user has the right to update the health status of the cow and update it if so
    boolean newHealthy = changesToCow.isHealthy();
    if (newHealthy) {
      if (requester instanceof Vet) {
        cowToUpdate.setHealthy(true);
      } else {
        //throw new RuntimeException("Only a veterinarian can mark a cow as healthy.");
        //soft denial: leave the field as is
        System.out.println("WARNING: User " + userId + " tried to mark a cow as healthy, but is not a vet.");
      }
    } else {
      // anyone (including vets) can mark a cow as not healthy
      cowToUpdate.setHealthy(false);
    }


    //now we can safely update the cow
    Cow savedCow = cowRepository.save(cowToUpdate);

    //since this cow will already have an ID, JPA will automatically look for the corresponding cow
    // and perform the UPDATE operation instead of creating a new one
    //ahh, convenience
    return CowMapper.convertCowToDto(savedCow);

  }


  //update one or many cows' health status
  @Override
  public void updateManyCowsHealth(List<Long> cowsIds, boolean healthUpdate, long userId)
  {
    User updatingUser = userRepository.findById(userId)
        .orElseThrow(() -> new RuntimeException("User not found: " + userId));
    if (!(updatingUser instanceof Vet) && healthUpdate)
    {
      throw new RuntimeException(
          "Only a veterinarian can confirm a cow as healthy.");
    }
    //if marking the cows as not healthy, to send to quarantine, any user can do it
    List<Cow> cows = cowRepository.findAllById(cowsIds);
    if (cows.size() != cowsIds.size())
    {
      throw new RuntimeException("One or more cows not found.");
    }
    //this just confirms the save
    cows.forEach(cow -> cow.setHealthy(healthUpdate));
    cowRepository.saveAll(cows);
  }

  //DELETE
  public void deleteCow(long id)
  {
    if (!cowRepository.existsById(id))
    {
      throw new RuntimeException("Cow not found: " + id);
    }
    cowRepository.deleteById(id);
  }
}
