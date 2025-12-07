package sep3.dto.transferRecordDTO;

import java.time.LocalDateTime;

public class TransferRecordCreationDTO
{
    private Long cowId;
    private Long fromDepartmentId;
    private Long toDepartmentId;
    private Long requestedByUserId;
    // optional – if null, the service will set LocalDateTime.now()
    private LocalDateTime movedAt;

    public TransferRecordCreationDTO() { }

    public TransferRecordCreationDTO(Long cowId, Long fromDepartmentId, Long toDepartmentId,
                                     Long requestedByUserId, LocalDateTime movedAt)
    {
        this.cowId = cowId;
        this.fromDepartmentId = fromDepartmentId;
        this.toDepartmentId = toDepartmentId;
        this.requestedByUserId = requestedByUserId;
        this.movedAt = movedAt;
    }

    public Long getCowId()
    {
        return cowId;
    }

    public void setCowId(Long cowId)
    {
        this.cowId = cowId;
    }

    public Long getFromDepartmentId()
    {
        return fromDepartmentId;
    }

    public void setFromDepartmentId(Long fromDepartmentId)
    {
        this.fromDepartmentId = fromDepartmentId;
    }

    public Long getToDepartmentId()
    {
        return toDepartmentId;
    }

    public void setToDepartmentId(Long toDepartmentId)
    {
        this.toDepartmentId = toDepartmentId;
    }

    public Long getRequestedByUserId()
    {
        return requestedByUserId;
    }

    public void setRequestedByUserId(Long requestedByUserId)
    {
        this.requestedByUserId = requestedByUserId;
    }

    public LocalDateTime getMovedAt()
    {
        return movedAt;
    }

    public void setMovedAt(LocalDateTime movedAt)
    {
        this.movedAt = movedAt;
    }
}
