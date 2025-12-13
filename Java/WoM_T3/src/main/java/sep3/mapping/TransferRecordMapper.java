package sep3.mapping;

import io.grpc.Status;
import sep3.dto.transferRecordDTO.TransferRecordDataDTO;
import sep3.entity.TransferRecord;

import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;

public class TransferRecordMapper
{
    private TransferRecordMapper() { }

    public static TransferRecordDataDTO convertTransferRecordToDto(TransferRecord entity)
    {
        Long fromDeptId = entity.getFromDept() != null ? entity.getFromDept().getId() : null;
        Long toDeptId = entity.getToDept() != null ? entity.getToDept().getId() : null;
        Long requestedById = entity.getRequestedBy() != null ? entity.getRequestedBy().getId() : null;
        Long approvedByVetId = entity.getApprovedByVet() != null ? entity.getApprovedByVet().getId() : null;
        Long cowId = entity.getCow() != null ? entity.getCow().getId() : null;

        return new TransferRecordDataDTO(
                entity.getId(),
                entity.getMovedAt(),
                fromDeptId,
                toDeptId,
                requestedById,
                approvedByVetId,
                cowId
        );
    }

    public static LocalDateTime parseMovedAtOrThrow(String movedAt) {
        if (movedAt == null || movedAt.isBlank()) {
            throw Status.INVALID_ARGUMENT
                    .withDescription("movedAt is required and must be ISO-8601 LocalDateTime, e.g. 2025-01-12T15:33:20")
                    .asRuntimeException();
        }

        try {
            // expects ISO_LOCAL_DATE_TIME: "yyyy-MM-dd'T'HH:mm:ss" (seconds required)
            return LocalDateTime.parse(movedAt);
        } catch (DateTimeParseException ex) {
            throw Status.INVALID_ARGUMENT
                    .withDescription("Invalid movedAt format. Expected ISO LocalDateTime, e.g. 2025-01-12T15:33:20. Got: " + movedAt)
                    .asRuntimeException();
        }
    }
}
