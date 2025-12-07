package sep3.dao;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import sep3.entity.TransferRecord;

import java.util.List;

@Repository
public interface TransferRecordDAO extends JpaRepository<TransferRecord, Long>
{
    // history of transfers for a specific cow
    List<TransferRecord> findByCowId(long cowId);
}
