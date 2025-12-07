package sep3.service.interfaces;

import sep3.dto.ContainerDtos;

public interface IContainerService {

    // Create new container
    ContainerDtos.ContainerDto create(ContainerDtos.CreateContainerDto dto);

    // Update existing container
    ContainerDtos.ContainerDto update(ContainerDtos.UpdateContainerDto dto);

    // Delete by id
    void delete(long id);

    // Get single container by id
    ContainerDtos.ContainerDto get(long id);

    // Get all containers
    ContainerDtos.ContainerListDto getAll();
}
