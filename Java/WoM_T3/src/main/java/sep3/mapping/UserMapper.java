package sep3.mapping;

import sep3.dto.userDTO.UserCreationDTO;
import sep3.dto.userDTO.UserDataDTO;
import sep3.entity.user.*;

/**
 * UserMapper
 * Converts between DTOs and User entities.
 * Works with polymorphic classes: Owner, Worker, Vet.
 */
public class UserMapper
{

    // ---------------------------------------------------------------------------
    // DTO → ENTITY (For Registration)
    // ---------------------------------------------------------------------------
    public static User toEntity(UserCreationDTO dto)
    {
        // Normalize incoming role (UI may send worker, WORKER, Worker)
        String roleString = dto.getRole() != null ? dto.getRole().toUpperCase() : "";

        UserRole roleEnum;
        try {
            roleEnum = UserRole.valueOf(roleString);
        }
        catch (IllegalArgumentException e) {
            throw new RuntimeException("Invalid or missing User Role: " + dto.getRole());
        }

        switch (roleEnum) {
            case VET:
                return new Vet(
                        dto.getName(), dto.getEmail(), dto.getPhone(),
                        dto.getAddress(), dto.getPassword(), dto.getLicenseNumber()
                );

            case OWNER:
                return new Owner(
                        dto.getName(), dto.getEmail(), dto.getPhone(),
                        dto.getAddress(), dto.getPassword()
                );

            case WORKER:
            default:
                return new Worker(
                        dto.getName(), dto.getEmail(), dto.getPhone(),
                        dto.getAddress(), dto.getPassword()
                );
        }
    }

    // ---------------------------------------------------------------------------
    // DTO → ENTITY (For Updating)
    // Only applies basic updates (name, email, phone, address)
    // Polymorphic fields (Vet license) handled inside updateUser()
    // ---------------------------------------------------------------------------
    public static User updateEntity(UserDataDTO dto, User userToUpdate)
    {
        if (dto.getName() != null && !dto.getName().isBlank())
            userToUpdate.setName(dto.getName());

        if (dto.getEmail() != null && !dto.getEmail().isBlank())
            userToUpdate.setEmail(dto.getEmail());

        if (dto.getPhone() != null && !dto.getPhone().isBlank())
            userToUpdate.setPhone(dto.getPhone());

        if (dto.getAddress() != null && !dto.getAddress().isBlank())
            userToUpdate.setAddress(dto.getAddress());

        // VET license update (only works if entity is Vet)
        if (dto.getLicenseNumber() != null && !dto.getLicenseNumber().isBlank()) {
            if (userToUpdate instanceof Vet vet) {
                vet.setLicenseNumber(dto.getLicenseNumber());
            }
        }

        return userToUpdate;
    }

    // ---------------------------------------------------------------------------
    // ENTITY → DTO (For Viewing Users)
    // Includes licenseNumber only if user is Vet
    // ---------------------------------------------------------------------------
    public static UserDataDTO toDTO(User user)
    {
        String licenseNumber = null;

        if (user instanceof Vet vet) {
            licenseNumber = vet.getLicenseNumber();
        }

        return new UserDataDTO(
                user.getName(),
                user.getEmail(),
                user.getAddress(),
                user.getPhone(),
                user.getId(),
                user.getRole().toString(),   // OWNER, WORKER, VET
                licenseNumber                // null for non-VET users
        );
    }
}
