package sep3.service.interfaces;

import sep3.dto.transferRecordDTO.TransferRecordCreationDTO;
import sep3.dto.transferRecordDTO.TransferRecordDataDTO;

import java.util.List;

public interface ITransferRecordDataService
{
    TransferRecordDataDTO addTransferRecord(TransferRecordCreationDTO transfer);

    List<TransferRecordDataDTO> getAllTransferRecords();

    List<TransferRecordDataDTO> getTransferRecordsForCow(long cowId);

    TransferRecordDataDTO getTransferRecordById(long id);

    // e.g. approval by vet
    TransferRecordDataDTO approveTransfer(long transferId, long vetUserId);
}
