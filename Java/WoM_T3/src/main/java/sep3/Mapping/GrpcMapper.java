package sep3.Mapping;
import sep3.dto.cowDTO.CowCreationDTO;
import sep3.dto.cowDTO.CowDataDTO;
import sep3.wayofmilk.grpc.CowCreationRequest; // Import your generated classes
import sep3.wayofmilk.grpc.CowData; // Import your generated classes
import java.time.LocalDate;
import java.time.format.DateTimeParseException;

public class GrpcMapper {

  //OUT
  /* Convert internal CowDataDTO (with nulls)
   * into the gRPC CowData message (with 'optional' fields).
   */
  public static CowData convertCowDtoToProto(CowDataDTO dto) {
    CowData.Builder builder = CowData.newBuilder();

    builder.setId(dto.getId()); // ID is primitive long, always safe if not null

    // Null Checks for Strings
    if (dto.getRegNo() != null) builder.setRegNo(dto.getRegNo());
    if (dto.getBirthDate() != null) builder.setBirthDate(dto.getBirthDate().toString());

    // Safe check for Boolean
    if (dto.isHealthy() != null) {
      builder.setIsHealthy(dto.isHealthy());
    }

    // Safe check for Long (Department ID)
    if (dto.getDepartmentId() != null) {
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
}
