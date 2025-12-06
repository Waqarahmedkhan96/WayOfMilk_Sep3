package sep3.Mapping;

import sep3.dto.transferRecordDTO.TransferRecordDataDTO;
import sep3.entity.TransferRecord;

public class TransferRecordMapper
{
    private TransferRecordMapper() { }

    public static TransferRecordDataDTO convertTransferRecordToDto(TransferRecord entity)
    {
        Long fromDeptId = entity.getFromDept() != null ? entity.getFromDept().getId() : null;
        Long toDeptId = entity.getToDept() != null ? entity.getToDept().getId() : null;
        Long deptId = entity.getDepartment() != null ? entity.getDepartment().getId() : null;
        Long requestedById = entity.getRequestedBy() != null ? entity.getRequestedBy().getId() : null;
        Long approvedByVetId = entity.getApprovedByVet() != null ? entity.getApprovedByVet().getId() : null;
        Long cowId = entity.getCow() != null ? entity.getCow().getId() : null;

        return new TransferRecordDataDTO(
                entity.getId(),
                entity.getMovedAt(),
                fromDeptId,
                toDeptId,
                deptId,
                requestedById,
                approvedByVetId,
                cowId
        );
    }
}
