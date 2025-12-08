package sep3.mapping;

import sep3.dto.userDTO.UserCreationDTO;
import sep3.dto.userDTO.UserDataDTO;
import sep3.entity.user.*;

public class UserMapper
{

  // DTO -> Entity (For Registration)
  public static User toEntity(UserCreationDTO dto)
  {
    // Normalize the role string (handle "vet", "VET", "Vet")
    String roleString = dto.getRole() != null ? dto.getRole().toUpperCase() : "";

    UserRole roleEnum;
    try
    {
      roleEnum = UserRole.valueOf(roleString);
    }
    catch (IllegalArgumentException e)
    {
      // enforcing a role (error if no role is provided)
      throw new RuntimeException("Invalid or missing User Role: " + dto.getRole());
    }

    switch (roleEnum)
    {
      case VET:
        return new Vet(dto.getName(), dto.getEmail(), dto.getPhone(),
            dto.getAddress(), dto.getPassword(), dto.getLicenseNumber());

      case OWNER:
        return new Owner(dto.getName(), dto.getEmail(), dto.getPhone(),
            dto.getAddress(), dto.getPassword());

      case WORKER:
        return new Worker(dto.getName(), dto.getEmail(), dto.getPhone(),
            dto.getAddress(), dto.getPassword());

      default:
        throw new RuntimeException("Role logic not implemented for: " + roleEnum);
    }

  }

  //DTO -> Entity for updating
  public static User updateEntity(UserDataDTO dto, User userToUpdate)
  {
    if (dto.getName() != null && !dto.getName().isBlank())
    {
      userToUpdate.setName(dto.getName());
    }
    if (dto.getEmail() != null && !dto.getEmail().isBlank())
    {
      userToUpdate.setEmail(dto.getEmail());
    }
    if (dto.getPhone() != null && !dto.getPhone().isBlank())
    {
      userToUpdate.setPhone(dto.getPhone());
    }
    if (dto.getAddress() != null && !dto.getAddress().isBlank())
    {
      userToUpdate.setAddress(dto.getAddress());
    }

    // Polymorphism check for Vet License
    if (dto.getLicenseNumber() != null && !dto.getLicenseNumber().isBlank())
    {
      if (userToUpdate instanceof Vet)
      {
        ((Vet) userToUpdate).setLicenseNumber(dto.getLicenseNumber());
      }
    }

    return userToUpdate;
  }

  // Entity -> DTO (For Viewing Users)
  public static UserDataDTO toDTO(User user)
  {
    String licenseNumber = null;

    // check if user is a vet
    if (user instanceof Vet)
    {
      licenseNumber = ((Vet) user).getLicenseNumber();
    }

    return new UserDataDTO(user.getName(), user.getEmail(), user.getAddress(),
        user.getPhone(), user.getId(), user.getRole().toString(), licenseNumber
        // Will be null for Owners and Workers
    );
  }


}