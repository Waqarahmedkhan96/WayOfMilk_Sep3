package sep3.RequestHandlers.TransferRecordService;

import org.springframework.stereotype.Service;
import sep3.Mapping.TransferRecordMapper;
import sep3.dao.CowDAO;
import sep3.dao.DepartmentDAO;
import sep3.dao.TransferRecordDAO;
import sep3.dao.UserDAO;
import sep3.dto.transferRecordDTO.TransferRecordCreationDTO;
import sep3.dto.transferRecordDTO.TransferRecordDataDTO;
import sep3.entity.Cow;
import sep3.entity.Department;
import sep3.entity.TransferRecord;
import sep3.entity.user.User;

import java.time.LocalDateTime;
import java.util.List;
import java.util.stream.Collectors;

@Service
public class TransferRecordDataService implements ITransferRecordDataService
{
    private final TransferRecordDAO transferRecordDAO;
    private final CowDAO cowDAO;
    private final DepartmentDAO departmentDAO;
    private final UserDAO userDAO;

    public TransferRecordDataService(TransferRecordDAO transferRecordDAO,
                                     CowDAO cowDAO,
                                     DepartmentDAO departmentDAO,
                                     UserDAO userDAO)
    {
        this.transferRecordDAO = transferRecordDAO;
        this.cowDAO = cowDAO;
        this.departmentDAO = departmentDAO;
        this.userDAO = userDAO;
    }

    @Override
    public TransferRecordDataDTO addTransferRecord(TransferRecordCreationDTO dto)
    {
        if (dto.getCowId() == null ||
                dto.getFromDepartmentId() == null ||
                dto.getToDepartmentId() == null ||
                dto.getRequestedByUserId() == null)
        {
            throw new IllegalArgumentException("cowId, fromDepartmentId, toDepartmentId i requestedByUserId są wymagane.");
        }

        Cow cow = cowDAO.findById(dto.getCowId())
                .orElseThrow(() -> new RuntimeException("Cow not found: " + dto.getCowId()));

        Department fromDept = departmentDAO.findById(dto.getFromDepartmentId())
                .orElseThrow(() -> new RuntimeException("From Department not found: " + dto.getFromDepartmentId()));

        Department toDept = departmentDAO.findById(dto.getToDepartmentId())
                .orElseThrow(() -> new RuntimeException("To Department not found: " + dto.getToDepartmentId()));

        User requestedBy = userDAO.findById(dto.getRequestedByUserId())
                .orElseThrow(() -> new RuntimeException("User not found: " + dto.getRequestedByUserId()));

        LocalDateTime movedAt = dto.getMovedAt() != null ? dto.getMovedAt() : LocalDateTime.now();

        // Uproszczenie: we treat department as the target department
        TransferRecord entity = new TransferRecord(
                movedAt,
                fromDept,
                toDept,
                toDept,
                requestedBy,
                null,
                cow
        );

        TransferRecord saved = transferRecordDAO.save(entity);

        // update cow's department
        cow.setDepartment(toDept);
        cowDAO.save(cow);

        return TransferRecordMapper.convertTransferRecordToDto(saved);
    }

    @Override
    public List<TransferRecordDataDTO> getAllTransferRecords()
    {
        return transferRecordDAO.findAll()
                .stream()
                .map(TransferRecordMapper::convertTransferRecordToDto)
                .collect(Collectors.toList());
    }

    @Override
    public List<TransferRecordDataDTO> getTransferRecordsForCow(long cowId)
    {
        return transferRecordDAO.findByCowId(cowId)
                .stream()
                .map(TransferRecordMapper::convertTransferRecordToDto)
                .collect(Collectors.toList());
    }

    @Override
    public TransferRecordDataDTO getTransferRecordById(long id)
    {
        TransferRecord entity = transferRecordDAO.findById(id)
                .orElseThrow(() -> new RuntimeException("TransferRecord not found: " + id));

        return TransferRecordMapper.convertTransferRecordToDto(entity);
    }

    @Override
    public TransferRecordDataDTO approveTransfer(long transferId, long vetUserId)
    {
        TransferRecord entity = transferRecordDAO.findById(transferId)
                .orElseThrow(() -> new RuntimeException("TransferRecord not found: " + transferId));

        User vet = userDAO.findById(vetUserId)
                .orElseThrow(() -> new RuntimeException("User not found: " + vetUserId));

        entity.setApprovedByVet(vet);

        // if movedAt was null (e.g., "approve and set actual time"), set it now
        if (entity.getMovedAt() == null)
        {
            entity.setMovedAt(LocalDateTime.now());
        }

        TransferRecord saved = transferRecordDAO.save(entity);
        return TransferRecordMapper.convertTransferRecordToDto(saved);
    }
}
