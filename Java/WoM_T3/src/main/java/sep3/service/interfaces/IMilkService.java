package sep3.service.interfaces;

import sep3.dto.MilkDtos;

public interface IMilkService {

    MilkDtos.MilkDto create(MilkDtos.CreateMilkDto dto);
    MilkDtos.MilkDto update(MilkDtos.UpdateMilkDto dto);
    void delete(long id);
    MilkDtos.MilkDto get(long id);
    MilkDtos.MilkListDto getAll();
    MilkDtos.MilkListDto getByContainer(MilkDtos.MilkByContainerQuery dto);

    // NEW
    MilkDtos.MilkDto approveForStorage(MilkDtos.ApproveMilkStorageDto dto);
}
