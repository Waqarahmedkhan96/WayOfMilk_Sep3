package sep3.Mapping;
import sep3.dto.cowDTO.*;
import sep3.dto.userDTO.*;
import sep3.wayofmilk.grpc.CowCreationRequest; // Import your generated classes
import sep3.wayofmilk.grpc.CowData; // Import your generated classes
import sep3.wayofmilk.grpc.UserCreationRequest;
import sep3.wayofmilk.grpc.UserData;
import sep3.wayofmilk.grpc.AuthenticationRequest;

import java.time.LocalDate;
import java.time.format.DateTimeParseException;

public class GrpcMapper {

  //COW mappers

  //OUT
  /* Convert internal CowDataDTO (with nulls)
   * into the gRPC CowData message (with 'optional' fields).
   */
  public static CowData convertCowDtoToProto(CowDataDTO dto)
  {
    CowData.Builder builder = CowData.newBuilder();

    builder.setId(dto.getId()); // ID is primitive long, always safe if not null

    // Null Checks for Strings
    if (dto.getRegNo() != null)
      builder.setRegNo(dto.getRegNo());
    if (dto.getBirthDate() != null)
      builder.setBirthDate(dto.getBirthDate().toString());

    // Safe check for Boolean
    if (dto.isHealthy() != null)
    {
      builder.setIsHealthy(dto.isHealthy());
    }

    // Safe check for Long (Department ID)
    if (dto.getDepartmentId() != null)
    {
      builder.setDepartmentId(dto.getDepartmentId());
    }

    return builder.build();
  }

  //  Converts the gRPC CowData message (with 'optional' fields)
   // into internal CowDataDTO (with nulls).
   // This is for handling an incoming partial UPDATE request.

  public static CowDataDTO convertCowProtoToDto(CowData proto) {
    CowDataDTO dto = new CowDataDTO();

    // ID is always present
    dto.setId(proto.getId());

    // Use has...() to check if a value was ACTUALLY sent
    if (proto.hasRegNo()) {
      dto.setRegNo(proto.getRegNo());
    }
    if (proto.hasBirthDate()) {
      if (proto.getBirthDate().isEmpty()) {
        dto.setBirthDate(null);
      } else {
        dto.setBirthDate(LocalDate.parse(proto.getBirthDate()));
      }
    }
    if (proto.hasIsHealthy()) {
      dto.setHealthy(proto.getIsHealthy());
    }
    if (proto.hasDepartmentId()) {
      dto.setDepartmentId(proto.getDepartmentId());
    }

    return dto;
  }

  // Converts an incoming gRPC CowCreationRequest
   // into internal CowCreationDTO

  public static CowCreationDTO convertCowProtoCreationToDto(CowCreationRequest proto) {
    // Assumes fields are required for creation
    try {
      return new CowCreationDTO(
          proto.getRegNo(),
          LocalDate.parse(proto.getBirthDate()), // Expects YYYY-MM-DD
          proto.getRegisteredByUserId()
      );
    } catch (DateTimeParseException e) {
      throw new IllegalArgumentException("Date must be in YYYY-MM-DD format");
    }
  }

  //USER mappers

  // 1. Creation Request (Proto -> DTO)
  public static UserCreationDTO userCreationProtoToDto(UserCreationRequest proto)
  {
    UserCreationDTO dto = new UserCreationDTO();
    dto.setEmail(proto.getEmail());
    dto.setName(proto.getName());
    dto.setAddress(proto.getAddress());
    dto.setPhone(proto.getPhone());
    dto.setPassword(proto.getPassword());
    dto.setRole(proto.getRole());
    dto.setLicenseNumber(proto.getLicenseNumber());
    return dto;
  }

  // 2. User Data (DTO -> Proto) - For sending user info back to T2/Client
  public static UserData convertUserDtoToProto(UserDataDTO dto) {
    UserData.Builder builder = UserData.newBuilder();

    // ID is primitive long
    builder.setId(dto.getId());

    // Null checks for Strings to prevent NullPointerExceptions
    if (dto.getName() != null) builder.setName(dto.getName());
    if (dto.getEmail() != null) builder.setEmail(dto.getEmail());
    if (dto.getAddress() != null) builder.setAddress(dto.getAddress());
    if (dto.getPhone() != null) builder.setPhone(dto.getPhone());
    if (dto.getRole() != null) builder.setRole(dto.getRole());
    if (dto.getLicenseNumber() != null) builder.setLicenseNumber(dto.getLicenseNumber());

    return builder.build();
  }

  // 3. User Data (Proto -> DTO) - For receiving updates from T2
  public static UserDataDTO convertUserProtoToDto(UserData proto) {
    UserDataDTO dto = new UserDataDTO();

    dto.setId(proto.getId());

    // Only set fields that were actually sent (Optional check)
    if (proto.hasName()) dto.setName(proto.getName());
    if (proto.hasEmail()) dto.setEmail(proto.getEmail());
    if (proto.hasAddress()) dto.setAddress(proto.getAddress());
    if (proto.hasPhone()) dto.setPhone(proto.getPhone());
    if (proto.hasRole()) dto.setRole(proto.getRole());
    if (proto.hasLicenseNumber()) dto.setLicenseNumber(proto.getLicenseNumber());

    return dto;
  }

  // 4. Login Request (Proto -> DTO)
  public static UserLoginDTO convertLoginProtoToDto(AuthenticationRequest proto) {
    UserLoginDTO dto = new UserLoginDTO();
    dto.setEmail(proto.getEmail());
    dto.setPassword(proto.getPassword());
    return dto;
  }
}
