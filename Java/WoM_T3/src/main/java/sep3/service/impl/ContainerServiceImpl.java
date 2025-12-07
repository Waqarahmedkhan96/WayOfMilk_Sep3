package sep3.service.impl;

import jakarta.persistence.EntityNotFoundException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import sep3.repository.ContainerRepository;
import sep3.dto.ContainerDtos;
import sep3.entity.Container;
import sep3.mapping.ContainerMapper;
import sep3.service.interfaces.IContainerService;

import java.util.List;

@Service
@Transactional
public class ContainerServiceImpl implements IContainerService {

    private final ContainerRepository containerRepository;
    private final ContainerMapper containerMapper;

    public ContainerServiceImpl(ContainerRepository containerRepository, ContainerMapper containerMapper) {
        this.containerRepository = containerRepository;
        this.containerMapper = containerMapper;
    }

    @Override
    public ContainerDtos.ContainerDto create(ContainerDtos.CreateContainerDto dto) {
        // map DTO -> Entity
        Container container = containerMapper.fromCreateDto(dto);

        // save entity
        Container saved = containerRepository.save(container);

        // map back to DTO
        return containerMapper.toDto(saved);
    }

    @Override
    public ContainerDtos.ContainerDto update(ContainerDtos.UpdateContainerDto dto) {
        // find existing entity or throw
        Container existing = containerRepository.findById(dto.getId())
                .orElseThrow(() -> new EntityNotFoundException(
                        "Container not found with id: " + dto.getId()));

        // apply changes from DTO
        containerMapper.updateEntity(existing, dto);

        // save updated entity
        Container saved = containerRepository.save(existing);

        // return DTO
        return containerMapper.toDto(saved);
    }

    @Override
    public void delete(long id) {
        if (!containerRepository.existsById(id)) {
            throw new EntityNotFoundException("Container not found with id: " + id);
        }
        containerRepository.deleteById(id);
    }

    @Override
    public ContainerDtos.ContainerDto get(long id) {
        Container container = containerRepository.findById(id)
                .orElseThrow(() -> new EntityNotFoundException(
                        "Container not found with id: " + id));

        return containerMapper.toDto(container);
    }

    @Override
    public ContainerDtos.ContainerListDto getAll() {
        List<Container> containers = containerRepository.findAll();
        return containerMapper.toListDto(containers);
    }
}
