package sep3.RequestHandlers;
import sep3.dto.CowCreationDTO;
import sep3.dto.CowDataDTO;
import java.util.List;

public interface ICowDataService {
  CowDataDTO addCow(CowCreationDTO cow);
  List<CowDataDTO> getAllCows();
  CowDataDTO updateCow(CowDataDTO changesToCow);
  CowDataDTO updateCowHealth(CowDataDTO changesToCow);
  void deleteCow(long id);
  CowDataDTO getCowById(long cowToFindId);
}