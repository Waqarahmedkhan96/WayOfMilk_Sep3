package sep3.RequestHandlers.CowService;
import sep3.dto.cowDTO.CowCreationDTO;
import sep3.dto.cowDTO.CowDataDTO;
import java.util.List;

public interface ICowDataService {
  CowDataDTO addCow(CowCreationDTO cow);
  List<CowDataDTO> getAllCows();
  CowDataDTO updateCow(CowDataDTO changesToCow);
  CowDataDTO updateCowHealth(CowDataDTO changesToCow);
  void deleteCow(long id);
  CowDataDTO getCowById(long cowToFindId);
}