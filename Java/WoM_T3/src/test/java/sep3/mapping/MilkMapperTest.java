package sep3.mapping;

import org.junit.jupiter.api.Test;
import sep3.dto.MilkDtos;
import sep3.entity.Container;
import sep3.entity.Cow;
import sep3.entity.Milk;
import sep3.entity.user.User;

import java.time.LocalDate;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.mock;
import static sep3.entity.MilkTestResult.FAIL;
import static sep3.entity.MilkTestResult.PASS;

class MilkMapperTest {

    @Test
    void fromCreateDto_mapsFieldsCorrectly() {
        MilkDtos.CreateMilkDto dto = new MilkDtos.CreateMilkDto();
        dto.setDate(LocalDate.now());
        dto.setVolumeL(10.0);
        dto.setTestResult(PASS);
        dto.setApprovedForStorage(true);

        Cow cow = mock(Cow.class);
        Container container = mock(Container.class);
        User user = mock(User.class);

        Milk milk = MilkMapper.fromCreateDto(dto, cow, container, user);

        assertEquals(10.0, milk.getVolumeL());
        assertEquals(PASS, milk.getMilkTestResult());
        assertEquals(cow, milk.getCow());
        assertEquals(container, milk.getContainer());
        assertEquals(user, milk.getRegisteredBy());
        assertTrue(milk.isApprovedForStorage());
    }

    @Test
    void toDto_nullMilk_returnsNull() {
        assertNull(MilkMapper.toDto(null));
    }
}
