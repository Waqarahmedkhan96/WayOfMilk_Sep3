package Mapping;

import DTOs.CowInfoDTO;
import entities.Cow;

public class CowMappper
{
  //static methods to map between entities and DTOs for request handlers
  public CowMappper(){}

  public static CowInfoDTO convertCowToDto(Cow cow)
  {
    // You use the parameterized constructor of the CowInfoDTO
    return new CowInfoDTO(cow.getId(), cow.getRegNo(), cow.getBirthDate(),
        cow.isHealthy() // Use the isHealthy getter
    );
  }
}
