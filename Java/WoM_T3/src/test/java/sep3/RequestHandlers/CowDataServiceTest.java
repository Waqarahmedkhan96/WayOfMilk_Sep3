package sep3.RequestHandlers;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import sep3.RequestHandlers.CowService.CowDataService;
import sep3.dao.CowDAO;
import sep3.dao.DepartmentDAO;
import sep3.dao.UserDAO;
import sep3.dto.cowDTO.CowCreationDTO;
import sep3.dto.cowDTO.CowDataDTO;
import sep3.entity.*;
import sep3.entity.user.Owner;
import sep3.entity.user.User;

import java.time.LocalDate;
import java.util.List;
import java.util.Optional;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class CowDataServiceTest {

  @Mock
  private CowDAO mockCowDAO;
  @Mock
  private DepartmentDAO mockDepartmentDAO;
  @Mock
  private UserDAO mockUserDAO;

  @InjectMocks
  private CowDataService cowDataService;

  @Test
  void testAddCow_Success() {
    // 1. Arrange
    long userId = 99L;
    CowCreationDTO newCowDto = new CowCreationDTO("Reg123", LocalDate.now(), userId);

    // Prepare dummy dependencies
    Department mockQuarantine = new Department(DepartmentType.QUARANTINE);
    User mockUser = new Owner("Owie", "owie@email.com", "25648454", "Street 123", "password");

    mockUser.setId(userId);

    // Define Mock Behavior
    when(mockDepartmentDAO.findByType(DepartmentType.QUARANTINE))
        .thenReturn(Optional.of(mockQuarantine));

    when(mockUserDAO.findById(userId))
        .thenReturn(Optional.of(mockUser));

    // Simulate the "Saved" cow returned by the DB
    // Note: We use the constructor that accepts Department and User
    Cow savedCow = new Cow("Reg123", LocalDate.now(), mockQuarantine, mockUser);
    savedCow.setId(1L); // DB assigns ID

    when(mockCowDAO.save(any(Cow.class))).thenReturn(savedCow);

    // 2. Act
    CowDataDTO resultDto = cowDataService.addCow(newCowDto);

    // 3. Assert
    verify(mockDepartmentDAO, times(1)).findByType(DepartmentType.QUARANTINE);
    verify(mockUserDAO, times(1)).findById(userId);
    verify(mockCowDAO, times(1)).save(any(Cow.class));

    Assertions.assertNotNull(resultDto);
    Assertions.assertEquals(1L, resultDto.getId());
    Assertions.assertEquals("Reg123", resultDto.getRegNo());
    // Verify defaults
    Assertions.assertFalse(resultDto.isHealthy());
  }

  @Test
  void testGetCowById_Success() {
    // 1. Arrange
    Department dept = new Department(DepartmentType.QUARANTINE);
    User user = new Owner("Owie", "owie@email.com", "25648454", "Street 123", "password");

    Cow foundCow = new Cow("Reg123", LocalDate.now(), dept, user);
    foundCow.setId(50L);

    when(mockCowDAO.findById(50L)).thenReturn(Optional.of(foundCow));

    // 2. Act
    CowDataDTO resultDto = cowDataService.getCowById(50L);

    // 3. Assert
    Assertions.assertNotNull(resultDto);
    Assertions.assertEquals(50L, resultDto.getId());
    Assertions.assertEquals("Reg123", resultDto.getRegNo());
    verify(mockCowDAO, times(1)).findById(50L);
  }

  @Test
  void testGetCowById_NotFound() {
    // 1. Arrange
    when(mockCowDAO.findById(999L)).thenReturn(Optional.empty());

    // 2. Act & Assert
    RuntimeException thrown = Assertions.assertThrows(RuntimeException.class, () -> {
      cowDataService.getCowById(999L);
    });

    Assertions.assertTrue(thrown.getMessage().contains("Cow not found"));
  }

  @Test
  void testGetAllCows() {
    // 1. Arrange
    Department dept = new Department(DepartmentType.QUARANTINE);
    User user = new Owner("Owie", "owie@email.com", "25648454", "Street 123", "password");

    Cow cow1 = new Cow("Reg1", LocalDate.now(), dept, user);
    cow1.setId(1L);
    Cow cow2 = new Cow("Reg2", LocalDate.now(), dept, user);
    cow2.setId(2L);

    when(mockCowDAO.findAll()).thenReturn(List.of(cow1, cow2));

    // 2. Act
    List<CowDataDTO> resultDtos = cowDataService.getAllCows();

    // 3. Assert
    Assertions.assertEquals(2, resultDtos.size());
    Assertions.assertEquals(1L, resultDtos.get(0).getId());
    Assertions.assertEquals("Reg2", resultDtos.get(1).getRegNo());
    verify(mockCowDAO, times(1)).findAll();
  }

  @Test
  void testDeleteCow() {
    // 1. Arrange (Delete usually returns void, so we just call it)
    long cowId = 10L;

    // 2. Act
    cowDataService.deleteCow(cowId);

    // 3. Assert
    verify(mockCowDAO, times(1)).deleteById(cowId);
  }
}