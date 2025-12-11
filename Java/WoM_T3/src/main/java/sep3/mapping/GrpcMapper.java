package sep3.mapping;
import sep3.dto.MilkDtos.*;
import sep3.dto.cowDTO.*;
import sep3.dto.departmentDTO.DepartmentCreationDTO;
import sep3.dto.departmentDTO.DepartmentDataDTO;
import sep3.dto.transferRecordDTO.TransferRecordCreationDTO;
import sep3.dto.transferRecordDTO.TransferRecordDataDTO;
import sep3.dto.userDTO.*;
import sep3.dto.customerDTO.CustomerCreationDTO;
import sep3.dto.customerDTO.CustomerDataDTO;
import sep3.dto.saleDTO.SaleCreationDTO;
import sep3.dto.saleDTO.SaleDataDTO;

import sep3.entity.DepartmentType;
import sep3.entity.MilkTestResult;
import sep3.wayofmilk.grpc.*;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;

public final class GrpcMapper
{

  //no constructor because it's a utility class with static methods only (also why final)
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

  public static CowDataDTO convertCowProtoToDto(CowData proto)
  {
    CowDataDTO dto = new CowDataDTO();

    // ID is always present
    dto.setId(proto.getId());

    // Use has...() to check if a value was ACTUALLY sent
    if (proto.hasRegNo())
    {
      dto.setRegNo(proto.getRegNo());
    }
    if (proto.hasBirthDate())
    {
      if (proto.getBirthDate().isEmpty())
      {
        dto.setBirthDate(null);
      }
      else
      {
        dto.setBirthDate(LocalDate.parse(proto.getBirthDate()));
      }
    }
    if (proto.hasIsHealthy())
    {
      dto.setHealthy(proto.getIsHealthy());
    }
    if (proto.hasDepartmentId())
    {
      dto.setDepartmentId(proto.getDepartmentId());
    }

    return dto;
  }

  // Converts an incoming gRPC CowCreationRequest
  // into internal CowCreationDTO

  public static CowCreationDTO convertCowProtoCreationToDto(CowCreationRequest proto)
  {
    // Assumes fields are required for creation
    try
    {
      return new CowCreationDTO(proto.getRegNo(),
          LocalDate.parse(proto.getBirthDate()), // Expects YYYY-MM-DD
          proto.getRegisteredByUserId(), proto.getQuarantineDepartmentId());
    }
    catch (DateTimeParseException e)
    {
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
  public static UserData convertUserDtoToProto(UserDataDTO dto)
  {
    UserData.Builder builder = UserData.newBuilder();

    // ID is primitive long
    builder.setId(dto.getId());

    // Null checks for Strings to prevent NullPointerExceptions
    if (dto.getName() != null)
      builder.setName(dto.getName());
    if (dto.getEmail() != null)
      builder.setEmail(dto.getEmail());
    if (dto.getAddress() != null)
      builder.setAddress(dto.getAddress());
    if (dto.getPhone() != null)
      builder.setPhone(dto.getPhone());
    if (dto.getRole() != null)
      builder.setRole(dto.getRole());
    if (dto.getLicenseNumber() != null)
      builder.setLicenseNumber(dto.getLicenseNumber());

    return builder.build();
  }

  // 3. User Data (Proto -> DTO) - For receiving updates from T2
  public static UserDataDTO convertUserProtoToDto(UserData proto)
  {
    UserDataDTO dto = new UserDataDTO();

    dto.setId(proto.getId());

    // Only set fields that were actually sent (Optional check)
    if (proto.hasName())
      dto.setName(proto.getName());
    if (proto.hasEmail())
      dto.setEmail(proto.getEmail());
    if (proto.hasAddress())
      dto.setAddress(proto.getAddress());
    if (proto.hasPhone())
      dto.setPhone(proto.getPhone());
    if (proto.hasRole())
      dto.setRole(proto.getRole());
    if (proto.hasLicenseNumber())
      dto.setLicenseNumber(proto.getLicenseNumber());

    return dto;
  }

  // 4. Login Request (Proto -> DTO)
  public static UserLoginDTO convertLoginProtoToDto(AuthenticationRequest proto)
  {
    UserLoginDTO dto = new UserLoginDTO();
    dto.setEmail(proto.getEmail());
    dto.setPassword(proto.getPassword());
    return dto;
  }

    // Department mappers


    public static DepartmentData convertDepartmentDtoToProto(DepartmentDataDTO dto) {

        DepartmentData.Builder b = DepartmentData.newBuilder();

        if (dto.getId() != null) {
            b.setId(dto.getId());
        }

        if (dto.getType() != null) {
            // enum -> STRING dla proto
            b.setType(dto.getType().name());
        }

        // UWAGA: proto NIE ma cows ani transferów – nic więcej nie mapujemy

        return b.build();
    }


    public static DepartmentDataDTO convertDepartmentProtoToDto(DepartmentData proto)
    {
        DepartmentDataDTO dto = new DepartmentDataDTO();
        dto.setId(proto.getId());

        // tutaj typ jest OPCJONALNY (np. przy update tylko ID)
        DepartmentType type = convertDepartmentTypeStringToEnum(proto.getType());
        dto.setType(type);

        return dto;
    }

    public static DepartmentCreationDTO convertDepartmentProtoCreationToDto(
            DepartmentCreationRequest proto)
    {
        DepartmentType type = convertDepartmentTypeStringToEnum(proto.getType());
        if (type == null) {
            throw new IllegalArgumentException("Department type is required for creation.");
        }
        return new DepartmentCreationDTO(type);
    }

    /**
     * Bezpieczna konwersja String -> DepartmentType.
     * Zwraca null, jeśli string jest pusty – dzięki temu update bez typu nie wywala serwera.
     */
    public static DepartmentType convertDepartmentTypeStringToEnum(String type) {
        if (type == null || type.isBlank()) {
            return null; // 👈 przy update/getById nie rzucamy, tylko zostawiamy null
        }

        try {
            return DepartmentType.valueOf(type.toUpperCase());
        } catch (Exception e) {
            throw new IllegalArgumentException("Invalid department type: " + type);
        }
    }


    // Transfer Record mapping
    public static TransferRecordData convertTransferRecordDtoToProto(TransferRecordDataDTO dto)
    {
        TransferRecordData.Builder builder = TransferRecordData.newBuilder();

        if (dto.getId() != null) builder.setId(dto.getId());
        if (dto.getMovedAt() != null) builder.setMovedAt(dto.getMovedAt().toString());

        if (dto.getFromDepartmentId() != null) builder.setFromDepartmentId(dto.getFromDepartmentId());
        if (dto.getToDepartmentId() != null) builder.setToDepartmentId(dto.getToDepartmentId());

        if (dto.getRequestedByUserId() != null) builder.setRequestedByUserId(dto.getRequestedByUserId());
        if (dto.getApprovedByVetUserId() != null) builder.setApprovedByVetUserId(dto.getApprovedByVetUserId());

        if (dto.getCowId() != null) builder.setCowId(dto.getCowId());

        return builder.build();
    }

  public static TransferRecordData convertTransferDtoToProto(
      TransferRecordDataDTO dto)
  {
    return convertTransferRecordDtoToProto(dto);
  }

  public static TransferRecordCreationDTO convertTransferRecordProtoCreationToDto(
      TransferRecordCreationRequest proto)
  {
    LocalDateTime movedAt = proto.getMovedAt().isBlank() ? null : LocalDateTime.parse(proto.getMovedAt());

        return new TransferRecordCreationDTO(
                proto.getCowId(),
                proto.getFromDepartmentId(),
                proto.getToDepartmentId(),
                proto.getRequestedByUserId(),
                movedAt
        );
    }

    public static TransferRecordCreationDTO convertTransferProtoCreationToDto(
            TransferRecordCreationRequest proto)
    {
        return convertTransferRecordProtoCreationToDto(proto);
    }


    public static TransferRecordDataDTO convertTransferRecordProtoToDto(TransferRecordData proto)
    {
        TransferRecordDataDTO dto = new TransferRecordDataDTO();

        dto.setId(proto.getId());

    if (proto.getMovedAt() != null && !proto.getMovedAt().isBlank())
      dto.setMovedAt(LocalDateTime.parse(proto.getMovedAt()));

        dto.setFromDepartmentId(proto.getFromDepartmentId());
        dto.setToDepartmentId(proto.getToDepartmentId());

        dto.setRequestedByUserId(proto.getRequestedByUserId());
        dto.setApprovedByVetUserId(proto.getApprovedByVetUserId());

        dto.setCowId(proto.getCowId());

        return dto;
    }

    public static TransferRecordDataDTO convertTransferProtoToDto(TransferRecordData proto)
    {
        return convertTransferRecordProtoToDto(proto);
    }


    // =================== CUSTOMER MAPPERS ===================

    // DTO -> Proto (for sending CustomerData back to T2)
    public static CustomerData convertCustomerDtoToProto(CustomerDataDTO dto) {
        CustomerData.Builder builder = CustomerData.newBuilder();

        if (dto.getId() != null) {
            builder.setId(dto.getId());
        }
        if (dto.getCompanyName() != null) {
            builder.setCompanyName(dto.getCompanyName());
        }
        if (dto.getPhoneNo() != null) {
            builder.setPhoneNo(dto.getPhoneNo());
        }
        if (dto.getEmail() != null) {
            builder.setEmail(dto.getEmail());
        }
        if (dto.getCompanyCVR() != null) {
            builder.setCompanyCVR(dto.getCompanyCVR());
        }

        return builder.build();
    }

    // Proto -> DTO (for incoming updates, if ever used)
    public static CustomerDataDTO convertCustomerProtoToDto(CustomerData proto) {
        CustomerDataDTO dto = new CustomerDataDTO();

        dto.setId(proto.getId());

        if (!proto.getCompanyName().isBlank()) {
            dto.setCompanyName(proto.getCompanyName());
        }
        if (!proto.getPhoneNo().isBlank()) {
            dto.setPhoneNo(proto.getPhoneNo());
        }
        if (!proto.getEmail().isBlank()) {
            dto.setEmail(proto.getEmail());
        }
        if (!proto.getCompanyCVR().isBlank()) {
            dto.setCompanyCVR(proto.getCompanyCVR());
        }

        return dto;
    }

    // Creation: Proto -> DTO
    public static CustomerCreationDTO convertCustomerProtoCreationToDto(CustomerCreationRequest proto) {
        CustomerCreationDTO dto = new CustomerCreationDTO();
        dto.setCompanyName(proto.getCompanyName());
        dto.setPhoneNo(proto.getPhoneNo());
        dto.setEmail(proto.getEmail());
        dto.setCompanyCVR(proto.getCompanyCVR());
        return dto;
    }

// =================== SALE MAPPERS ===================

    // DTO -> Proto (for sending sale info back to T2)
    public static SaleData convertSaleDtoToProto(SaleDataDTO dto) {
        SaleData.Builder builder = SaleData.newBuilder();

        if (dto.getId() != null) {
            builder.setId(dto.getId());
        }
        if (dto.getCustomerId() != null) {
            builder.setCustomerId(dto.getCustomerId());
        }
        if (dto.getContainerId() != null) {
            builder.setContainerId(dto.getContainerId());
        }
        if (dto.getQuantityL() != null) {
            builder.setQuantityL(dto.getQuantityL());
        }
        if (dto.getPrice() != null) {
            builder.setPrice(dto.getPrice());
        }
        if (dto.getDateTime() != null) {
            builder.setDateTime(dto.getDateTime().toString());
        }
        if (dto.getRecallCase() != null) {
            builder.setRecallCase(dto.getRecallCase());
        }
        if (dto.getCreatedByUserId() != null) {
            builder.setCreatedByUserId(dto.getCreatedByUserId());
        }

        return builder.build();
    }

    // Proto -> DTO (for incoming updates, if needed)
    public static SaleDataDTO convertSaleProtoToDto(SaleData proto) {
        SaleDataDTO dto = new SaleDataDTO();

        dto.setId(proto.getId());
        dto.setCustomerId(proto.getCustomerId());
        dto.setContainerId(proto.getContainerId());
        dto.setQuantityL(proto.getQuantityL());
        dto.setPrice(proto.getPrice());

        if (!proto.getDateTime().isBlank()) {
            dto.setDateTime(java.time.LocalDateTime.parse(proto.getDateTime()));
        }

        dto.setRecallCase(proto.getRecallCase());
        dto.setCreatedByUserId(proto.getCreatedByUserId());

        return dto;
    }

    // Creation: Proto -> DTO
    public static SaleCreationDTO convertSaleProtoCreationToDto(SaleCreationRequest proto) {
        java.time.LocalDateTime dateTime = null;
        if (!proto.getDateTime().isBlank()) {
            dateTime = java.time.LocalDateTime.parse(proto.getDateTime());
        }

        SaleCreationDTO dto = new SaleCreationDTO(
                proto.getCustomerId(),
                proto.getContainerId(),
                proto.getQuantityL(),
                proto.getPrice(),
                proto.getCreatedByUserId(),
                dateTime
        );
        dto.setRecallCase(proto.getRecallCase()); // NEW
        return dto;
    }

  //Milk mappers

  public static MilkDto convertMilkProtoToDto(MilkMessage proto)
  {

    MilkDto dto = new MilkDto();
    dto.setId(proto.getId());
    dto.setVolumeL(proto.getVolumeL());
    dto.setDate(LocalDate.parse(proto.getDate()));
    dto.setContainerId(proto.getContainerId());
    dto.setCowId(proto.getCowId());
    //no registeredBy field in this dto, although the proto has it
    dto.setTestResult(convertProtoMilkTestResult(proto.getTestResult()));
    return dto;
  }

  public static MilkMessage convertMilkDtoToProto(MilkDto dto)
  {
    MilkMessage.Builder builder = MilkMessage.newBuilder();
    builder.setId(dto.getId());
    builder.setVolumeL(dto.getVolumeL());
    builder.setDate(dto.getDate().toString());
    builder.setContainerId(dto.getContainerId());
    builder.setCowId(dto.getCowId());
    builder.setTestResult(convertDtoMilkTestResult(dto.getTestResult()));

    //no registeredBy field in this dto, so remember to set it up manually in the grpc implementation
    return builder.build();
  }

  //helper method to parse one enum to the other

  public static MilkTestResult convertProtoMilkTestResult(MilkTestResultEnum proto)
  {
    if (proto == null)
      return null;
    try
    {
      // map by enum name; falls back to null if no matching value exists
      return MilkTestResult.valueOf(proto.name());
    }
    catch (IllegalArgumentException e)
    {
      return null;
    }
  }

  public static MilkTestResultEnum convertDtoMilkTestResult(MilkTestResult dto)
  {
    if (dto == null)
      return null;
    try
    {
      return MilkTestResultEnum.valueOf(dto.toString());
    }
    catch (IllegalArgumentException e)
    {
      return null;
    }
  }

}
