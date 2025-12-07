package sep3.service.impl;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import sep3.repository.DepartmentRepository;
import sep3.dto.departmentDTO.DepartmentCreationDTO;
import sep3.dto.departmentDTO.DepartmentDataDTO;
import sep3.entity.Department;
import sep3.entity.DepartmentType;

import java.util.List;
import java.util.Optional;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class DepartmentServiceImplTest {

    @Mock
    private DepartmentRepository mockDepartmentRepository;

    @InjectMocks
    private DepartmentServiceImpl departmentDataService;

    // ---------- ADD ----------
    @Test
    void testAddDepartment_Success() {
        // Arrange
        DepartmentCreationDTO creationDTO = new DepartmentCreationDTO(DepartmentType.MILKING);

        Department saved = new Department(DepartmentType.MILKING);
        saved.setId(1L);

        when(mockDepartmentRepository.save(any(Department.class))).thenReturn(saved);

        // Act
        DepartmentDataDTO result = departmentDataService.addDepartment(creationDTO);

        // Assert
        Assertions.assertNotNull(result);
        Assertions.assertEquals(1L, result.getId());
        Assertions.assertEquals(DepartmentType.MILKING, result.getType());
        verify(mockDepartmentRepository, times(1)).save(any(Department.class));
    }

    // ---------- GET BY ID ----------
    @Test
    void testGetDepartmentById_Success() {
        Department dept = new Department(DepartmentType.RESTING);
        dept.setId(5L);

        when(mockDepartmentRepository.findById(5L)).thenReturn(Optional.of(dept));

        DepartmentDataDTO result = departmentDataService.getDepartmentById(5L);

        Assertions.assertNotNull(result);
        Assertions.assertEquals(5L, result.getId());
        Assertions.assertEquals(DepartmentType.RESTING, result.getType());
        verify(mockDepartmentRepository, times(1)).findById(5L);
    }

    @Test
    void testGetDepartmentById_NotFound() {
        when(mockDepartmentRepository.findById(123L)).thenReturn(Optional.empty());

        RuntimeException ex = Assertions.assertThrows(RuntimeException.class, () ->
                departmentDataService.getDepartmentById(123L));

        Assertions.assertTrue(ex.getMessage().contains("not found"));
    }

    // ---------- GET BY TYPE ----------
    @Test
    void testGetDepartmentByType_Success() {
        Department dept = new Department(DepartmentType.QUARANTINE);
        dept.setId(10L);

        when(mockDepartmentRepository.findByType(DepartmentType.QUARANTINE))
                .thenReturn(Optional.of(dept));

        DepartmentDataDTO result =
                departmentDataService.getDepartmentByType(DepartmentType.QUARANTINE);

        Assertions.assertEquals(10L, result.getId());
        Assertions.assertEquals(DepartmentType.QUARANTINE, result.getType());
    }

    // ---------- GET ALL ----------
    @Test
    void testGetAllDepartments() {
        Department d1 = new Department(DepartmentType.MILKING); d1.setId(1L);
        Department d2 = new Department(DepartmentType.RESTING); d2.setId(2L);

        when(mockDepartmentRepository.findAll()).thenReturn(List.of(d1, d2));

        List<DepartmentDataDTO> dtos = departmentDataService.getAllDepartments();

        Assertions.assertEquals(2, dtos.size());
        Assertions.assertEquals(1L, dtos.get(0).getId());
        Assertions.assertEquals(DepartmentType.RESTING, dtos.get(1).getType());
        verify(mockDepartmentRepository, times(1)).findAll();
    }

    // ---------- DELETE ----------
    @Test
    void testDeleteDepartment() {
        long id = 7L;

        departmentDataService.deleteDepartment(id);

        verify(mockDepartmentRepository, times(1)).deleteById(id);
    }
}
