package sep3.service.impl;

import jakarta.persistence.EntityNotFoundException;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import sep3.dto.saleDTO.SaleCreationDTO;
import sep3.repository.ContainerRepository;
import sep3.repository.CustomerRepository;
import sep3.repository.SaleRepository;
import sep3.repository.UserRepository;

import java.util.Optional;

import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.mockito.Mockito.when;

@ExtendWith(MockitoExtension.class)
class SaleServiceImplTest {

    @Mock
    private SaleRepository saleRepo;
    @Mock private CustomerRepository customerRepo;
    @Mock private ContainerRepository containerRepo;
    @Mock private UserRepository userRepo;

    @InjectMocks
    private SaleServiceImpl service;

    @Test
    void addSale_missingRequiredField_throwsException() {
        SaleCreationDTO dto = new SaleCreationDTO();
        dto.setCustomerId(1L); // reszta null

        assertThrows(IllegalArgumentException.class,
                () -> service.addSale(dto));
    }

    @Test
    void getSaleById_notFound_throwsException() {
        when(saleRepo.findById(1L)).thenReturn(Optional.empty());

        assertThrows(EntityNotFoundException.class,
                () -> service.getSaleById(1L));
    }
}
