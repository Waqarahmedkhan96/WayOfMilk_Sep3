package sep3.service.impl;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import sep3.dto.cowDTO.CowCreationDTO;
import sep3.dto.cowDTO.CowDataDTO;
import sep3.entity.Cow;
import sep3.entity.Department;
import sep3.entity.DepartmentType;
import sep3.entity.user.*;
import sep3.repository.CowRepository;
import sep3.repository.DepartmentRepository;
import sep3.repository.UserRepository;

import java.time.LocalDate;
import java.util.Optional;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class CowServiceImplTest {

  @Mock
  private CowRepository mockCowRepository;
  @Mock
  private DepartmentRepository mockDepartmentRepository;
  @Mock
  private UserRepository mockUserRepository;

  @InjectMocks
  private CowServiceImpl cowDataService;

  @Test
  void testAddCow_Success() {
    // 1. ARRANGE (Setup data)
    long deptId = 1L;
    long userId = 100L;
    String regNo = "12345";
    LocalDate birthDate = LocalDate.now();

    // Create the input DTO
    CowCreationDTO newCowDto = new CowCreationDTO();
    newCowDto.setDepartmentId(deptId);
    newCowDto.setRegisteredByUserId(userId);
    newCowDto.setRegNo(regNo);
    newCowDto.setBirthDate(birthDate);

    // Create the mocked entities that the repositories will return
    Department mockDepartment = new Department( DepartmentType.QUARANTINE, "Quarantine Dept");
    mockDepartment.setId(deptId);
    mockDepartment.setType(DepartmentType.QUARANTINE); // Critical: Must be Quarantine to pass logic

    User mockUser = new Owner();
    mockUser.setId(userId);

    // Create the cow that acts as the "saved" result
    Cow savedCow = new Cow(regNo, birthDate, mockDepartment, mockUser);
    savedCow.setId(50L); // simulate DB assigning an ID

    // TELLING MOCKITO WHAT TO DO:
    // When the service asks for a Department, give it the mockDepartment
    when(mockDepartmentRepository.findById(deptId)).thenReturn(Optional.of(mockDepartment));

    // When the service asks for a User, give it the mockUser
    when(mockUserRepository.findById(userId)).thenReturn(Optional.of(mockUser));

    // When the service saves ANY cow, return the savedCow we created above
    when(mockCowRepository.save(any(Cow.class))).thenReturn(savedCow);

    // 2. ACT (Run the method)
    CowDataDTO result = cowDataService.addCow(newCowDto);

    // 3. ASSERT (Verify results)
    assertNotNull(result);
    assertEquals(regNo, result.getRegNo());
    // Verify the repository was actually called
    verify(mockCowRepository, times(1)).save(any(Cow.class));
  }



  @Test
  void testGetCowById_Success() {
    // 1. Arrange
    Department dept = new Department(DepartmentType.QUARANTINE, "Quarantine Dept");
    User user = new Owner("Owie", "owie@email.com", "25648454", "Street 123", "password");

    Cow foundCow = new Cow("Reg123", LocalDate.now(), dept, user);
    foundCow.setId(50L);

    when(mockCowRepository.findById(50L)).thenReturn(Optional.of(foundCow));

    // 2. Act
    CowDataDTO resultDto = cowDataService.getCowById(50L);

    // 3. Assert
    Assertions.assertNotNull(resultDto);
    Assertions.assertEquals(50L, resultDto.getId());
    Assertions.assertEquals("Reg123", resultDto.getRegNo());
    verify(mockCowRepository, times(1)).findById(50L);
  }

  @Test
  void testGetCowById_NotFound() {
    // 1. Arrange
    when(mockCowRepository.findById(999L)).thenReturn(Optional.empty());

    // 2. Act & Assert
    RuntimeException thrown = Assertions.assertThrows(RuntimeException.class, () -> {
      cowDataService.getCowById(999L);
    });

    Assertions.assertTrue(thrown.getMessage().contains("Cow not found"));
  }

  @Test
  void testGetAllCows() {
    // 1. Arrange
    Department dept = new Department(DepartmentType.QUARANTINE, "Quarantine Dept");
    User user = new Owner("Owie", "owie@email.com", "25648454", "Street 123", "password");

    Cow cow1 = new Cow("Reg1", LocalDate.now(), dept, user);
    cow1.setId(1L);
    Cow cow2 = new Cow("Reg2", LocalDate.now(), dept, user);
    cow2.setId(2L);

    when(mockCowRepository.findAll()).thenReturn(List.of(cow1, cow2));

    // 2. Act
    List<CowDataDTO> resultDtos = cowDataService.getAllCows();

    // 3. Assert
    Assertions.assertEquals(2, resultDtos.size());
    Assertions.assertEquals(1L, resultDtos.get(0).getId());
    Assertions.assertEquals("Reg2", resultDtos.get(1).getRegNo());
    verify(mockCowRepository, times(1)).findAll();
  }

  @Test
  void testDeleteCow() {
    // 1. Arrange
    Department dept = new Department(DepartmentType.QUARANTINE, "Quarantine Dept");
    User user = new Owner("Owie", "owie@email.com", "25648454", "Street 123", "password");

    Cow cow1 = new Cow("Reg1", LocalDate.now(), dept, user);
    cow1.setId(1L);
    long cowId = 1L;

    // stub repository existence check so service won't throw
    when(mockCowRepository.existsById(cowId)).thenReturn(true);

    // 2. Act
    cowDataService.deleteCow(cowId);

    // 3. Assert
    verify(mockCowRepository, times(1)).deleteById(cowId);
  }
}