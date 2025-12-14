package sep3.mapping;


import sep3.dto.cowDTO.CowDataDTO;
import sep3.entity.Cow;
import sep3.entity.Department;

import java.util.List;
import java.util.stream.Collectors;

public final class CowMapper
{
  //static methods to map between entities and DTOs for request handlers
  private CowMapper()
  {
  }
  //constructor private here because this is a utility class
  // and doesn't need to be instantiated'

  public static CowDataDTO convertCowToDto(Cow cow)
  {
    // Check if department exists before asking for its ID and name
    //shouldn't be null, but just in case the database chokes on this specifically
    Long deptId = (cow.getDepartment() != null) ?
        cow.getDepartment().getId() :
        null;
    String deptName = (cow.getDepartment() != null) ?
        cow.getDepartment().getName() : null;

    // You use the parameterized constructor of the CowDataDTO
    return new CowDataDTO(cow.getId(), cow.getRegNo(), cow.getBirthDate(),
        cow.isHealthy(), deptId, deptName);
  }

  public static Cow convertDtoToEntity(CowDataDTO dto, Cow cowToConvert,
      Department departmentFromDto)
  {
    Cow entity = cowToConvert;
    if (dto.getRegNo() != null && !dto.getRegNo().isEmpty())
    {
      entity.setRegNo(dto.getRegNo());
    }
    // Check for null (which parsing an empty string "" from proto would produce)
    if (dto.getBirthDate() != null)
    {
      entity.setBirthDate(dto.getBirthDate());
    }

    // since this is now a Long, not long, a null check can be made
    if (dto.getDepartmentId() != null)
    {
      entity.setDepartment(departmentFromDto);
      //keeping this entirely away from the repos
    }

    // boolean is no longer a primitive so null checks can be made
    if (dto.isHealthy() != null)
    {
      entity.setHealthy(dto.isHealthy());
    }

    return entity;
  }

  public static List<CowDataDTO> convertCowListToDTO(List<Cow> cows)
  {
    return cows.stream().map(CowMapper::convertCowToDto).collect(Collectors.toList());
  }

  //for update methods
  //because proto could be missing some fields on some occasions (ex on transfers)
  public static void updateCowFromDto(Cow entityToUpdate, CowDataDTO dto,
      Department department)
  {
    // Check if the DTO actually provided a new value before setting it.
    // We check for null and empty string (Protobuf default).
    if (dto.getRegNo() != null && !dto.getRegNo().isEmpty())
    {
      entityToUpdate.setRegNo(dto.getRegNo());
    }
    // Check for null (which parsing an empty string "" from proto would produce)
    if (dto.getBirthDate() != null)
    {
      entityToUpdate.setBirthDate(dto.getBirthDate());
    }

    // since this is now a Long, not long, a null check can be made
    if (dto.getDepartmentId() != null)
    {
      entityToUpdate.setDepartment(department);
    }

    //department name is irrelevant here, so we don't parse it

    // boolean is no longer a primitive so null checks can be made
    //this is now handled in the service layer, after the user is checked for role
//        if (dto.isHealthy() != null)
//        {
//          entityToUpdate.setHealthy(dto.isHealthy());
//        }
  }
}
