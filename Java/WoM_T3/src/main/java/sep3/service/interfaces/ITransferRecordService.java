package sep3.service.interfaces;

import sep3.dto.transferRecordDTO.TransferRecordCreationDTO;
import sep3.dto.transferRecordDTO.TransferRecordDataDTO;

import java.util.List;

public interface ITransferRecordService
{
    TransferRecordDataDTO addTransferRecord(TransferRecordCreationDTO transfer);
    List<TransferRecordDataDTO> getAllTransferRecords();
    List<TransferRecordDataDTO> getTransferRecordsForCow(long cowId);
    TransferRecordDataDTO getTransferRecordById(long id);
    TransferRecordDataDTO approveTransfer(long transferId, long vetUserId);

    TransferRecordDataDTO updateTransferRecord(TransferRecordDataDTO dto);
    void deleteTransferRecord(long id);
}
