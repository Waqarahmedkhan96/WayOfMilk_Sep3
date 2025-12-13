package sep3.dto.departmentDTO;

import sep3.dto.cowDTO.CowDataDTO;
import sep3.dto.transferRecordDTO.TransferRecordDataDTO;
import sep3.entity.DepartmentType;

import java.util.List;

public class DepartmentDataDTO
{
    private Long id;
    private DepartmentType type;
    private String name;

    private List<CowDataDTO> cows;
    private List<TransferRecordDataDTO> transferRecordsFrom;
    private List<TransferRecordDataDTO> transferRecordsTo;

    public DepartmentDataDTO() { }

    public DepartmentDataDTO(Long id, DepartmentType type, String name)
    {
        this.id = id;
        this.type = type;
        this.name = name;
    }

    public Long getId() { return id; }

    public void setId(Long id) { this.id = id; }

    public DepartmentType getType() { return type; }

    public void setType(DepartmentType type) { this.type = type; }

    public String getName() { return name; }

    public void setName(String name) { this.name = name; }

    public List<CowDataDTO> getCows() {
        return cows;
    }

    public void setCows(List<CowDataDTO> cows) {
        this.cows = cows;
    }

    public List<TransferRecordDataDTO> getTransferRecordsFrom() {
        return transferRecordsFrom;
    }

    public void setTransferRecordsFrom(List<TransferRecordDataDTO> transferRecordsFrom) {
        this.transferRecordsFrom = transferRecordsFrom;
    }

    public List<TransferRecordDataDTO> getTransferRecordsTo() {
        return transferRecordsTo;
    }

    public void setTransferRecordsTo(List<TransferRecordDataDTO> transferRecordsTo) {
        this.transferRecordsTo = transferRecordsTo;
    }


}
