package sep3.mapping;

import org.springframework.stereotype.Component;
import sep3.dto.MilkDtos;
import sep3.entity.Container;
import sep3.entity.Cow;
import sep3.entity.Milk;
import sep3.entity.user.User;

import java.util.List;
import java.util.stream.Collectors;

@Component
public class MilkMapper {
    public MilkMapper() {
    }

    // CreateDto -> Entity
    public Milk fromCreateDto(MilkDtos.CreateMilkDto dto, Cow cow, Container container, User registeredBy) {
        Milk milk = new Milk();
        milk.setDate(dto.getDate());   // service can set now() if null
        milk.setVolumeL(dto.getVolumeL());
        milk.setMilkTestResult(dto.getTestResult());
        milk.setCow(cow);
        milk.setContainer(container);
        milk.setRegisteredBy(registeredBy);
        // NOTE: we do NOT set approvedForStorage here.
        // Creation always starts as "not yet approved";
        // approval is handled centrally in approveForStorage(...)
        return milk;
    }

    // UpdateDto -> Entity
    public void updateEntity(Milk milk, MilkDtos.UpdateMilkDto dto, Container container) {
        if (dto.getDate() != null) milk.setDate(dto.getDate());
        if (dto.getVolumeL() != null) milk.setVolumeL(dto.getVolumeL());
        if (dto.getTestResult() != null) milk.setMilkTestResult(dto.getTestResult());
        if (container != null) milk.setContainer(container);
    }

    // Entity -> MilkDto
    public MilkDtos.MilkDto toDto(Milk milk) {
        if (milk == null) return null;
        MilkDtos.MilkDto dto = new MilkDtos.MilkDto();
        dto.setId(milk.getId());
        dto.setDate(milk.getDate());
        dto.setVolumeL(milk.getVolumeL());
        dto.setTestResult(milk.getMilkTestResult());
        if (milk.getCow() != null) dto.setCowId(milk.getCow().getId());
        if (milk.getContainer() != null) dto.setContainerId(milk.getContainer().getId());
        if (milk.getRegisteredBy() != null) dto.setRegisteredByUserId(milk.getRegisteredBy().getId());
        dto.setApprovedForStorage(milk.isApprovedForStorage());
        return dto;
    }

    // Entities -> ListDto
    public MilkDtos.MilkListDto toListDto(List<Milk> milkList) {
        MilkDtos.MilkListDto listDto = new MilkDtos.MilkListDto();
        listDto.setMilkRecords(
                milkList.stream()
                        .map(this::toDto)
                        .collect(Collectors.toList())
        );
        return listDto;
    }
}
