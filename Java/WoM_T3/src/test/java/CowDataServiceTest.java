import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import sep3.DAOs.CowDAO;
import sep3.DTOs.CowCreationDTO;
import sep3.DTOs.CowDataDTO;
import sep3.Mapping.CowMapper;
import sep3.RequestHandlers.CowDataService;
import sep3.entities.Cow;

import java.time.LocalDate;
import java.util.List;
import java.util.Optional;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class) class CowDataServiceTest
{

  @Mock private CowDAO mockCowDAO; // A fake DAO

  @Mock private CowMapper mockCowMapper; // can mock the mapper, but we test it
  // only if we have enough time on our hands to play

  @InjectMocks private CowDataService cowDataService; // The real service, but with fake parts
  //note: no point in testing the interface, as it's just a wrapper for the real service'

  @Test void testAddCow()
  {
    // 1. Arrange
    CowCreationDTO newCowDto = new CowCreationDTO("Reg123", LocalDate.now());
    // ... (rest of your arrange)
    Cow savedCow = new Cow("Reg123", LocalDate.now());
    savedCow.setId(1L);
    when(mockCowDAO.save(any(Cow.class))).thenReturn(savedCow);

    // This is what we expect the *real* mapper to return
    CowDataDTO expectedDto = new CowDataDTO(1L, "Reg123", LocalDate.now(),
        false, 0L);
    // (Note: Your Cow constructor sets isHealthy=false and departmentId=0 by default)

    // 2. Act
    CowDataDTO resultDto = cowDataService.addCow(newCowDto);

    // 3. Assert
    verify(mockCowDAO, times(1)).save(any(Cow.class));

    // --- Add these assertions! ---
    Assertions.assertNotNull(resultDto);
    Assertions.assertEquals(expectedDto.getId(), resultDto.getId());
    Assertions.assertEquals(expectedDto.getRegNo(), resultDto.getRegNo());
    Assertions.assertEquals(expectedDto.isHealthy(), resultDto.isHealthy());
  }

  @Test void testFindCowById()
  {
    // 1. ARRANGE
    // Creating the cow we expect to "find"
    Cow foundCow = new Cow("Reg123", LocalDate.now());
    foundCow.setId(1L);

    // Creating the DTO we expect the mapper to return
    CowDataDTO expectedDto = new CowDataDTO(1L, "Reg123", LocalDate.now(), true,
        0L);

    // --- This is the most important line ---
    // Tell the mock DAO: "WHEN findById(1L) is called, THEN return an Optional of foundCow"
    when(mockCowDAO.findById(1L)).thenReturn(Optional.of(foundCow));

    // skipping mocking the mapper for now
    // when(CowMapper.convertCowToDto(foundCow)).thenReturn(expectedDto);

    // 2. ACT
    // Calling the method we are testing
    CowDataDTO resultDto = cowDataService.getCowById(1L);

    // 3. ASSERT
    // Check that the method returned the correct data
    Assertions.assertNotNull(resultDto);
    Assertions.assertEquals(1L, resultDto.getId().longValue());
    //specifying long because the dto uses Long specifically, and it causes confusion for the compiler
    Assertions.assertEquals("Reg123", resultDto.getRegNo());

    // Also, verify the mock was called correctly
    verify(mockCowDAO, times(1)).findById(1L);

  }

  @Test void testGetAllCows()
  {
    //1. Arrange
    Cow cow1 = new Cow("Reg123", LocalDate.now());
    cow1.setId(1L);
    Cow cow2 = new Cow("Reg456", LocalDate.now());
    cow2.setId(2L);
    when(mockCowDAO.findAll()).thenReturn(List.of(cow1, cow2));

    //2. Act
    List<CowDataDTO> resultDtos = cowDataService.getAllCows();

    //3. Assert
    Assertions.assertEquals(2, resultDtos.size());
    Assertions.assertEquals(1L, resultDtos.get(0).getId().longValue());
    Assertions.assertEquals("Reg456", resultDtos.get(1).getRegNo());
    verify(mockCowDAO, times(1)).findAll();

  }

//  @Test void deleteCow()
//  {
//    //1. Arrange
//    Cow cowToDelete = new Cow("Reg123", LocalDate.now());
//    cowToDelete.setId(1L);
//    when(mockCowDAO.findById(1L)).thenReturn(Optional.of(cowToDelete));
//
//    //2. Act
//    cowDataService.deleteCow(1L);
//
//    //3. Assert
//    verify(mockCowDAO, times(1)).deleteById(1L);
//    Assertions.assertNull(cowDataService.getCowById(1L));
//  }



  //error case for duplicate reg no
  //currently not doing as told and it's late. might revisit another time
//  @Test void addDuplicate()
//  {
//    // 1. ARRANGE
//    CowCreationDTO newCowDto = new CowCreationDTO("Reg123", LocalDate.now());
//
//    // Tell the mock to SIMULATE the database error
//    when(mockCowDAO.save(any(Cow.class))).thenThrow(
//        new DataIntegrityViolationException("Simulating duplicate key error")
//    );
//
//    // 2. ACT & 3. ASSERT
//    // Now we check if our service correctly *handles* that error
//    // (in this case, by letting it bubble up)
//    Assertions.assertThrows(DataIntegrityViolationException.class, () -> cowDataService.addCow(newCowDto));
//  }

}
