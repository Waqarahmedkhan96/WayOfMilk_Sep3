package sep3.service;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import sep3.repository.UserRepository;
import sep3.service.impl.TransferRecordServiceImpl;
import sep3.repository.CowRepository;
import sep3.repository.DepartmentRepository;
import sep3.repository.TransferRepository;

import sep3.dto.transferRecordDTO.TransferRecordCreationDTO;
import sep3.dto.transferRecordDTO.TransferRecordDataDTO;

import sep3.entity.*;
import sep3.entity.user.Owner;
import sep3.entity.user.User;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Optional;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class TransferRecordDataServiceTest {

    @Mock private TransferRepository mockTransferDAO;
    @Mock private CowRepository mockCowRepository;
    @Mock private DepartmentRepository mockDepartmentRepository;
    @Mock private UserRepository mockUserRepository;

    @InjectMocks
    private TransferRecordServiceImpl transferRecordDataService;

    // ----------- ADD TRANSFER -----------
    @Test
    void testAddTransferRecord_Success() {
        // Arrange
        long cowId = 1L;
        long fromDeptId = 10L;
        long toDeptId = 20L;
        long userId = 5L;

        TransferRecordCreationDTO dto = new TransferRecordCreationDTO(
                cowId, fromDeptId, toDeptId, userId, LocalDateTime.now()
        );

        Cow cow = new Cow("COW1", LocalDate.now(), true, new Owner("Owie", "owie@email.com", "25648454", "Street 123", "password"));
        cow.setId(cowId);

        Department fromDept = new Department(DepartmentType.MILKING); fromDept.setId(fromDeptId);
        Department toDept = new Department(DepartmentType.RESTING); toDept.setId(toDeptId);

        User requester = new Owner("A", "a@a.com", "999", "Addr", "pw");
        requester.setId(userId);

        TransferRecord saved = new TransferRecord(
                dto.getMovedAt(), fromDept, toDept, toDept, requester, null, cow
        );
        saved.setId(999L);

        when(mockCowRepository.findById(cowId)).thenReturn(Optional.of(cow));
        when(mockDepartmentRepository.findById(fromDeptId)).thenReturn(Optional.of(fromDept));
        when(mockDepartmentRepository.findById(toDeptId)).thenReturn(Optional.of(toDept));
        when(mockUserRepository.findById(userId)).thenReturn(Optional.of(requester));
        when(mockTransferDAO.save(any(TransferRecord.class))).thenReturn(saved);

        // Act
        TransferRecordDataDTO result = transferRecordDataService.addTransferRecord(dto);

        // Assert
        Assertions.assertNotNull(result);
        Assertions.assertEquals(999L, result.getId());
        Assertions.assertEquals(cowId, result.getCowId());
        Assertions.assertEquals(fromDeptId, result.getFromDepartmentId());
        Assertions.assertEquals(toDeptId, result.getToDepartmentId());

        verify(mockTransferDAO, times(1)).save(any(TransferRecord.class));
        verify(mockCowRepository, times(1)).save(any(Cow.class)); // updated cow department
    }

    // ----------- GET ALL -----------
    @Test
    void testGetAllTransferRecords() {
        TransferRecord t1 = mockTransferRecord(1L);
        TransferRecord t2 = mockTransferRecord(2L);

        when(mockTransferDAO.findAll()).thenReturn(List.of(t1, t2));

        List<TransferRecordDataDTO> result = transferRecordDataService.getAllTransferRecords();

        Assertions.assertEquals(2, result.size());
        Assertions.assertEquals(1L, result.get(0).getId());
    }

    // ----------- GET FOR COW ----------
    @Test
    void testGetTransferRecordsForCow() {
        when(mockTransferDAO.findByCowId(10L))
                .thenReturn(List.of(mockTransferRecord(1L)));

        List<TransferRecordDataDTO> result =
                transferRecordDataService.getTransferRecordsForCow(10L);

        Assertions.assertEquals(1, result.size());
        Assertions.assertEquals(1L, result.get(0).getId());
    }

    // ----------- GET BY ID ----------
    @Test
    void testGetTransferRecordById_Success() {
        TransferRecord rec = mockTransferRecord(44L);

        when(mockTransferDAO.findById(44L)).thenReturn(Optional.of(rec));

        TransferRecordDataDTO dto =
                transferRecordDataService.getTransferRecordById(44L);

        Assertions.assertEquals(44L, dto.getId());
        verify(mockTransferDAO, times(1)).findById(44L);
    }

    @Test
    void testGetTransferRecordById_NotFound() {
        when(mockTransferDAO.findById(111L)).thenReturn(Optional.empty());

        RuntimeException ex =
                Assertions.assertThrows(RuntimeException.class,
                        () -> transferRecordDataService.getTransferRecordById(111L));

        Assertions.assertTrue(ex.getMessage().contains("not found"));
    }

    // ----------- APPROVE ----------
    @Test
    void testApproveTransfer_Success() {
        TransferRecord rec = mockTransferRecord(5L);

        User vet = new Owner("Vet", "v@v.com", "123", "A", "pw");
        vet.setId(99L);

        when(mockTransferDAO.findById(5L)).thenReturn(Optional.of(rec));
        when(mockUserRepository.findById(99L)).thenReturn(Optional.of(vet));
        when(mockTransferDAO.save(any(TransferRecord.class))).thenReturn(rec);

        TransferRecordDataDTO result =
                transferRecordDataService.approveTransfer(5L, 99L);

        Assertions.assertEquals(5L, result.getId());
        Assertions.assertEquals(99L, result.getApprovedByVetUserId());
    }

    // ---------- Helper ----------
    private TransferRecord mockTransferRecord(long id) {
        Department dept = new Department(DepartmentType.MILKING); dept.setId(10L);
        Cow cow = new Cow("A", LocalDate.now(), true, new Owner("Owie", "owie@email.com", "25648454", "Street 123", "password")); cow.setId(99L);

        TransferRecord r = new TransferRecord(
                LocalDateTime.now(), dept, dept, dept,
                new Owner("Owie", "owie@email.com", "25648454", "Street 123", "password"), null, cow
        );
        r.setId(id);
        return r;
    }
}
