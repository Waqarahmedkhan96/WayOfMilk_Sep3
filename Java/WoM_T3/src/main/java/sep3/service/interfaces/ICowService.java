package sep3.service.interfaces;
import sep3.dto.cowDTO.CowCreationDTO;
import sep3.dto.cowDTO.CowDataDTO;
import java.util.List;

public interface ICowService {
  CowDataDTO addCow(CowCreationDTO cow);
  List<CowDataDTO> getAllCows();
  CowDataDTO updateCow(CowDataDTO changesToCow);
  CowDataDTO updateCowHealth(CowDataDTO changesToCow);
  void deleteCow(long id);
  CowDataDTO getCowById(long cowToFindId);
    //void autoMoveToQuarantine(long cowId, long userId); //Maybe auto - quarantine the cow, when Milk test fails
}