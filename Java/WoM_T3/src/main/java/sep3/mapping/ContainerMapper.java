package sep3.mapping;

import org.springframework.stereotype.Component;
import sep3.dto.ContainerDtos;
import sep3.entity.Container;

import java.util.List;
import java.util.stream.Collectors;

@Component
public class ContainerMapper {

    // CreateDto -> Entity
    public Container fromCreateDto(ContainerDtos.CreateContainerDto dto) {
        return new Container(dto.getCapacityL());
    }

    // UpdateDto -> Entity
    public void updateEntity(Container c, ContainerDtos.UpdateContainerDto dto) {
        c.setCapacityL(dto.getCapacityL());
    }

    // Entity -> ContainerDto
    public ContainerDtos.ContainerDto toDto(Container c) {
        if (c == null) return null;
        ContainerDtos.ContainerDto dto = new ContainerDtos.ContainerDto();
        dto.setId(c.getId());
        dto.setCapacityL(c.getCapacityL());
        dto.setOccupiedCapacityL(c.getOccupiedCapacityL()); // map occupied
        return dto;
    }

    // Entities -> ListDto
    public ContainerDtos.ContainerListDto toListDto(List<Container> containers) {
        ContainerDtos.ContainerListDto listDto = new ContainerDtos.ContainerListDto();
        listDto.setContainers(
                containers.stream()
                        .map(this::toDto)
                        .collect(Collectors.toList())
        );
        return listDto;
    }
}
