package sep3.Mapping;

import sep3.dto.CowDataDTO;
import sep3.entity.Cow;

public class CowMappper
{
  //static methods to map between entities and DTOs for request handlers
  public CowMappper(){}

  public static CowDataDTO convertCowToDto(Cow cow)
  {
    // You use the parameterized constructor of the CowDataDTO
    return new CowDataDTO(cow.getId(), cow.getRegNo(), cow.getBirthDate(),
        cow.isHealthy() // Use the isHealthy getter
    );
  }
}
