package sep3.service.interfaces;
import sep3.dto.MilkDtos;
import sep3.dto.cowDTO.CowCreationDTO;
import sep3.dto.cowDTO.CowDataDTO;
import java.util.List;

public interface ICowService {
  CowDataDTO addCow(CowCreationDTO cow);
  CowDataDTO getCowById(long cowToFindId);
  CowDataDTO getCowByRegNo(String regNo);
  List<CowDataDTO> getCowsByDepartmentId(long departmentId);
  List<MilkDtos.MilkDto> getCowMilk(long cowId);
  List<CowDataDTO> getAllCows();
  CowDataDTO updateCow(CowDataDTO changesToCow, long userId);
  void updateManyCowsHealth(List<Long> cowsIds, boolean healthUpdate, long userId);
  void deleteCow(long id);
}