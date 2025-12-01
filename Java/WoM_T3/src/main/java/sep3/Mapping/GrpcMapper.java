package sep3.Mapping;
import sep3.DTOs.CowCreationDTO;
import sep3.DTOs.CowDataDTO;
import sep3.wayofmilk.grpc.CowCreationRequest; // Import your generated classes
import sep3.wayofmilk.grpc.CowData; // Import your generated classes
import java.time.LocalDate;

public class GrpcMapper {

  //OUT
  /* Convert internal CowDataDTO (with nulls)
   * into the gRPC CowData message (with 'optional' fields).
   */
  public static CowData convertCowDtoToProto(CowDataDTO dto) {
    CowData.Builder builder = CowData.newBuilder();

    // The DTO from the service will be complete, so we
    // can safely unbox and convert.
    builder.setId(dto.getId());
    builder.setRegNo(dto.getRegNo());
    builder.setBirthDate(dto.getBirthDate().toString());
    builder.setIsHealthy(dto.isHealthy());
    builder.setDepartmentId(dto.getDepartmentId());

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
    return new CowCreationDTO(
        proto.getRegNo(),
        LocalDate.parse(proto.getBirthDate())
    );
  }
}
