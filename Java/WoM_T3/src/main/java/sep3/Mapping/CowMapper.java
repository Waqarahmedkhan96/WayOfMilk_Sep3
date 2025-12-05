package sep3.Mapping;


import sep3.dto.cowDTO.CowDataDTO;
import sep3.entity.Cow;
import sep3.entity.Department;

public class CowMapper
{
  //static methods to map between entities and DTOs for request handlers
  public CowMapper()
  {
  }

  public static CowDataDTO convertCowToDto(Cow cow)
  {
    // Check if department exists before asking for its ID
    //shouldn't be null, but just in case the database chokes on this specifically
    Long deptId = (cow.getDepartment() != null) ? cow.getDepartment().getId() : null;

    // You use the parameterized constructor of the CowDataDTO
    return new CowDataDTO(cow.getId(), cow.getRegNo(), cow.getBirthDate(),
        cow.isHealthy(), deptId
    );
  }

  //not sure if needed, but better have than not have
  //update methods might use it, come to think of it
  //because proto could be missing some fields on some occasions (ex on transfers)
  public static void updateCowFromDto(Cow entityToUpdate, CowDataDTO dto, Department department)
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

    // boolean is no longer a primitive so null checks can be made
    if (dto.isHealthy() != null)
    {
      entityToUpdate.setHealthy(dto.isHealthy());
    }
  }
}
