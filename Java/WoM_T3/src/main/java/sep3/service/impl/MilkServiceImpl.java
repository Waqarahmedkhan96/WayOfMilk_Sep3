package sep3.service.impl;

import org.springframework.stereotype.Service;
import sep3.dto.MilkDtos;
import sep3.entity.Container;
import sep3.entity.Cow;
import sep3.entity.Milk;
import sep3.entity.MilkTestResult;
import sep3.entity.user.User;
import sep3.mapping.MilkMapper;
import sep3.repository.ContainerRepository;
import sep3.repository.CowRepository;
import sep3.repository.MilkRepository;
import sep3.repository.UserRepository;
import sep3.service.interfaces.ICowService;
import sep3.service.interfaces.IMilkService;

import java.time.LocalDate;
import java.util.List;

@Service
public class MilkServiceImpl implements IMilkService {

    private final MilkRepository milkRepo;
    private final CowRepository cowRepo;
    private final ContainerRepository containerRepo;
    private final UserRepository userRepo;
    private final MilkMapper mapper;
    private final ICowService cowService; // interface DI

    public MilkServiceImpl(MilkRepository milkRepo,
                           CowRepository cowRepo,
                           ContainerRepository containerRepo,
                           UserRepository userRepo,
                           MilkMapper mapper,
                           ICowService cowService) { // inject via interface
        this.milkRepo = milkRepo;
        this.cowRepo = cowRepo;
        this.containerRepo = containerRepo;
        this.userRepo = userRepo;
        this.mapper = mapper;
        this.cowService = cowService;
    }

    // CREATE MILK RECORD
    @Override
    public MilkDtos.MilkDto create(MilkDtos.CreateMilkDto dto) {

        Cow cow = cowRepo.findById(dto.getCowId()).orElseThrow();
        Container container = containerRepo.findById(dto.getContainerId()).orElseThrow();
        User registeredBy = userRepo.findById(dto.getRegisteredByUserId()).orElseThrow();

        if (dto.getDate() == null) {
            dto.setDate(LocalDate.now());
        }

        Milk milk = mapper.fromCreateDto(dto, cow, container, registeredBy);
        milkRepo.save(milk);
        return mapper.toDto(milk);
    }

    // UPDATE
    @Override
    public MilkDtos.MilkDto update(MilkDtos.UpdateMilkDto dto) {
        Milk milk = milkRepo.findById(dto.getId()).orElseThrow();

        Container container = null;
        if (dto.getContainerId() != null) {
            container = containerRepo.findById(dto.getContainerId()).orElseThrow();
        }

        mapper.updateEntity(milk, dto, container);
        milkRepo.save(milk);

        return mapper.toDto(milk);
    }

    // DELETE
    @Override
    public void delete(long id) {
        milkRepo.deleteById(id);
    }

    // GET ONE
    @Override
    public MilkDtos.MilkDto get(long id) {
        return mapper.toDto(milkRepo.findById(id).orElseThrow());
    }

    // GET ALL
    @Override
    public MilkDtos.MilkListDto getAll() {
        return mapper.toListDto(milkRepo.findAll());
    }

    // GET BY CONTAINER (using Container entity)
    @Override
    public MilkDtos.MilkListDto getByContainer(MilkDtos.MilkByContainerQuery dto) {
        Container container = containerRepo.findById(dto.getContainerId()).orElseThrow();
        List<Milk> list = milkRepo.findByContainer(container);
        return mapper.toListDto(list);
    }
}
