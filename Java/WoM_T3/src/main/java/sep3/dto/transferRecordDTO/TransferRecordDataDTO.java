package sep3.dto.transferRecordDTO;

import java.time.LocalDateTime;

public class TransferRecordDataDTO
{
    private Long id;
    private LocalDateTime movedAt;

    private Long fromDepartmentId;
    private Long toDepartmentId;

    private Long requestedByUserId;
    private Long approvedByVetUserId;

    private Long cowId;

    public TransferRecordDataDTO() { }

    public TransferRecordDataDTO(Long id, LocalDateTime movedAt,
                                 Long fromDepartmentId, Long toDepartmentId,
                                 Long requestedByUserId, Long approvedByVetUserId,
                                 Long cowId)
    {
        this.id = id;
        this.movedAt = movedAt;
        this.fromDepartmentId = fromDepartmentId;
        this.toDepartmentId = toDepartmentId;
        this.requestedByUserId = requestedByUserId;
        this.approvedByVetUserId = approvedByVetUserId;
        this.cowId = cowId;
    }

    public Long getId()
    {
        return id;
    }

    public void setId(Long id)
    {
        this.id = id;
    }

    public LocalDateTime getMovedAt()
    {
        return movedAt;
    }

    public void setMovedAt(LocalDateTime movedAt)
    {
        this.movedAt = movedAt;
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

    public Long getApprovedByVetUserId()
    {
        return approvedByVetUserId;
    }

    public void setApprovedByVetUserId(Long approvedByVetUserId)
    {
        this.approvedByVetUserId = approvedByVetUserId;
    }

    public Long getCowId()
    {
        return cowId;
    }

    public void setCowId(Long cowId)
    {
        this.cowId = cowId;
    }
}
