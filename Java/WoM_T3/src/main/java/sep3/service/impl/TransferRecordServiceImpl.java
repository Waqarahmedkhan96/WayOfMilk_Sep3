package sep3.service.impl;

import org.springframework.stereotype.Service;
import sep3.entity.DepartmentType;
import sep3.mapping.TransferRecordMapper;
import sep3.repository.CowRepository;
import sep3.repository.DepartmentRepository;
import sep3.repository.TransferRecordRepository;
import sep3.repository.UserRepository;
import sep3.dto.transferRecordDTO.TransferRecordCreationDTO;
import sep3.dto.transferRecordDTO.TransferRecordDataDTO;
import sep3.entity.Cow;
import sep3.entity.Department;
import sep3.entity.TransferRecord;
import sep3.entity.user.User;
import sep3.service.interfaces.ITransferRecordService;

import java.time.LocalDateTime;
import java.util.List;
import java.util.stream.Collectors;

@Service
public class TransferRecordServiceImpl implements ITransferRecordService
{
    private final TransferRecordRepository transferRecordRepository;
    private final CowRepository cowRepository;
    private final DepartmentRepository departmentRepository;
    private final UserRepository userRepository;

    public TransferRecordServiceImpl(TransferRecordRepository transferRecordRepository,
                                     CowRepository cowRepository,
                                     DepartmentRepository departmentRepository,
                                     UserRepository userRepository)
    {
        this.transferRecordRepository = transferRecordRepository;
        this.cowRepository = cowRepository;
        this.departmentRepository = departmentRepository;
        this.userRepository = userRepository;
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

        Cow cow = cowRepository.findById(dto.getCowId())
                .orElseThrow(() -> new RuntimeException("Cow not found: " + dto.getCowId()));

        Department fromDept = departmentRepository.findById(dto.getFromDepartmentId())
                .orElseThrow(() -> new RuntimeException("From Department not found: " + dto.getFromDepartmentId()));

        Department toDept = departmentRepository.findById(dto.getToDepartmentId())
                .orElseThrow(() -> new RuntimeException("To Department not found: " + dto.getToDepartmentId()));
    //if a cow is in quarantine, it can only be released if it is healthy
        if(fromDept.getType() == DepartmentType.QUARANTINE && !cow.isHealthy())
        {
          throw new IllegalArgumentException("Cow is not healthy and cannot be released from quarantine.");
        }

        User requestedBy = userRepository.findById(dto.getRequestedByUserId())
                .orElseThrow(() -> new RuntimeException("User not found: " + dto.getRequestedByUserId()));

        LocalDateTime movedAt = dto.getMovedAt() != null ? dto.getMovedAt() : LocalDateTime.now();

        // Uproszczenie: we treat department as the target department
        TransferRecord entity = new TransferRecord(
                movedAt,
                fromDept,
                toDept,
                requestedBy,
                null,
                cow
        );

        TransferRecord saved = transferRecordRepository.save(entity);

        // update cow's department
        cow.setDepartment(toDept);
        cowRepository.save(cow);

        return TransferRecordMapper.convertTransferRecordToDto(saved);
    }

    @Override
    public List<TransferRecordDataDTO> getAllTransferRecords()
    {
        return transferRecordRepository.findAll()
                .stream()
                .map(TransferRecordMapper::convertTransferRecordToDto)
                .collect(Collectors.toList());
    }

    @Override
    public List<TransferRecordDataDTO> getTransferRecordsForCow(long cowId)
    {
        return transferRecordRepository.findByCowId(cowId)
                .stream()
                .map(TransferRecordMapper::convertTransferRecordToDto)
                .collect(Collectors.toList());
    }

    @Override
    public TransferRecordDataDTO getTransferRecordById(long id)
    {
        TransferRecord entity = transferRecordRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("TransferRecord not found: " + id));

        return TransferRecordMapper.convertTransferRecordToDto(entity);
    }

    @Override
    public TransferRecordDataDTO updateTransferRecord(TransferRecordDataDTO dto)
    {
        TransferRecord entity = transferRecordRepository.findById(dto.getId())
                .orElseThrow(() -> new RuntimeException("TransferRecord not found: " + dto.getId()));

        if (dto.getMovedAt() != null)
            entity.setMovedAt(dto.getMovedAt());

        if (dto.getFromDepartmentId() != null)
        {
            Department from = departmentRepository.findById(dto.getFromDepartmentId())
                    .orElseThrow(() -> new RuntimeException("Department not found"));
            entity.setFromDept(from);
        }

        if (dto.getToDepartmentId() != null)
        {
            Department to = departmentRepository.findById(dto.getToDepartmentId())
                    .orElseThrow(() -> new RuntimeException("Department not found"));
            entity.setToDept(to);
        }

        if (dto.getApprovedByVetUserId() != null)
        {
            User vet = userRepository.findById(dto.getApprovedByVetUserId())
                    .orElseThrow(() -> new RuntimeException("User not found"));
            entity.setApprovedByVet(vet);
        }

        TransferRecord saved = transferRecordRepository.save(entity);
        return TransferRecordMapper.convertTransferRecordToDto(saved);
    }

    @Override
    public void deleteTransferRecord(long id)
    {
        transferRecordRepository.deleteById(id);
    }

    @Override
    public TransferRecordDataDTO approveTransfer(long transferId, long vetUserId)
    {
        TransferRecord entity = transferRecordRepository.findById(transferId)
                .orElseThrow(() -> new RuntimeException("TransferRecord not found: " + transferId));

        User vet = userRepository.findById(vetUserId)
                .orElseThrow(() -> new RuntimeException("User not found: " + vetUserId));

        entity.setApprovedByVet(vet);

        // if movedAt was null (e.g., "approve and set actual time"), set it now
        if (entity.getMovedAt() == null)
        {
            entity.setMovedAt(LocalDateTime.now());
        }

        TransferRecord saved = transferRecordRepository.save(entity);
        return TransferRecordMapper.convertTransferRecordToDto(saved);
    }
}
