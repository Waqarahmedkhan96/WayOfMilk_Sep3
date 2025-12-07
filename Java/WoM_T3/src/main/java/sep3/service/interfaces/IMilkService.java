package sep3.service.interfaces;

import sep3.dto.MilkDtos;

public interface IMilkService {

    MilkDtos.MilkDto create(MilkDtos.CreateMilkDto dto);   // create milk

    MilkDtos.MilkDto update(MilkDtos.UpdateMilkDto dto);   // update milk

    void delete(long id);                                  // delete milk

    MilkDtos.MilkDto get(long id);                         // get milk

    MilkDtos.MilkListDto getAll();                         // list all

    MilkDtos.MilkListDto getByContainer(MilkDtos.MilkByContainerQuery dto); // by container
}
