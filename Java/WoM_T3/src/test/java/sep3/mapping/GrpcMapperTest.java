package sep3.mapping;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import sep3.dto.cowDTO.CowDataDTO;
import sep3.wayofmilk.grpc.CowData;

import java.time.LocalDate;

class GrpcMapperTest {

  @Test
  void convertCowDtoToProto_shouldHandleNullsSafely() {
    // Arrange
    // Create a DTO with NULL Boolean and Long fields
    // We simulate a partial update or a cow with missing info
    CowDataDTO partialDto = new CowDataDTO();
    partialDto.setId(100L);
    partialDto.setRegNo("COW-999");
    partialDto.setBirthDate(LocalDate.of(2023, 1, 1));

    // EXPLICITLY setting these to null to trigger the crash
    partialDto.setHealthy(null);
    partialDto.setDepartmentId(null);

    // Act
    // This line will throw NullPointerException if the mapper isn't fixed
    CowData resultProto = GrpcMapper.convertCowDtoToProto(partialDto);

    // Assert
    // 1. Verify basic fields are present
    Assertions.assertEquals(100L, resultProto.getId());
    Assertions.assertEquals("COW-999", resultProto.getRegNo());

    // 2. Verify that the "Optional" fields are missing (NOT false, NOT 0, but missing)
    Assertions.assertFalse(resultProto.hasIsHealthy(),
        "Proto should not have isHealthy set if DTO value was null");

    Assertions.assertFalse(resultProto.hasDepartmentId(),
        "Proto should not have departmentId set if DTO value was null");
  }

  @Test
  void convertCowDtoToProto_shouldMapValuesWhenPresent() {
    // Arrange
    CowDataDTO fullDto = new CowDataDTO(
        50L,
        "COW-001",
        LocalDate.of(2022, 5, 5),
        true, // isHealthy
        10L   // departmentId
    );

    // Act
    CowData resultProto = GrpcMapper.convertCowDtoToProto(fullDto);

    // Assert
    Assertions.assertTrue(resultProto.hasIsHealthy());
    Assertions.assertTrue(resultProto.getIsHealthy());

    Assertions.assertTrue(resultProto.hasDepartmentId());
    Assertions.assertEquals(10L, resultProto.getDepartmentId());
  }
}