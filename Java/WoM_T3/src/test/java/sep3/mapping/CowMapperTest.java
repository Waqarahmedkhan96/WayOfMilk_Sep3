package sep3.mapping;

import org.junit.jupiter.api.Test;

import sep3.dto.cowDTO.CowDataDTO;
import sep3.entity.Cow;
import sep3.entity.Department;


import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.Mockito.*;

class CowMapperTest {

    @Test
    void convertCowToDto_withDepartment_mapsCorrectly() {
        Department dept = mock(Department.class);
        when(dept.getId()).thenReturn(5L);
        when(dept.getName()).thenReturn("Milking");

        Cow cow = mock(Cow.class);
        when(cow.getId()).thenReturn(1L);
        when(cow.getRegNo()).thenReturn("COW-1");
        when(cow.getDepartment()).thenReturn(dept);
        when(cow.isHealthy()).thenReturn(true);

        CowDataDTO dto = CowMapper.convertCowToDto(cow);

        assertEquals(1L, dto.getId());
        assertEquals("COW-1", dto.getRegNo());
        assertEquals(5L, dto.getDepartmentId());
        assertEquals("Milking", dto.getDepartmentName());
        assertTrue(dto.isHealthy());
    }

    @Test
    void updateCowFromDto_updatesOnlyProvidedFields() {
        Cow cow = mock(Cow.class);
        CowDataDTO dto = new CowDataDTO();
        Department dept = mock(Department.class);
        dto.setRegNo("NEW");

        CowMapper.updateCowFromDto(cow, dto, dept);

        verify(cow).setRegNo("NEW");
    }
}
