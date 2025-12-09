package sep3.service.impl;
import sep3.service.interfaces.IMilkService;


import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import sep3.dto.MilkDtos;
import sep3.entity.*;
import sep3.entity.user.User;
import sep3.entity.user.UserRole;
import sep3.mapping.MilkMapper;
import sep3.repository.*;

import java.time.LocalDate;
import java.util.List;

@Service
@Transactional
public class MilkServiceImpl implements IMilkService {

    private final MilkRepository milkRepo;
    private final CowRepository cowRepo;
    private final ContainerRepository containerRepo;
    private final UserRepository userRepo;
    private final MilkMapper mapper;

    public MilkServiceImpl(MilkRepository milkRepo,
                           CowRepository cowRepo,
                           ContainerRepository containerRepo,
                           UserRepository userRepo,
                           MilkMapper mapper) {

        this.milkRepo = milkRepo;
        this.cowRepo = cowRepo;
        this.containerRepo = containerRepo;
        this.userRepo = userRepo;
        this.mapper = mapper;
    }

    // ---------- CREATE ----------
    @Override
    public MilkDtos.MilkDto create(MilkDtos.CreateMilkDto dto) {

        Cow cow = cowRepo.findById(dto.getCowId()).orElseThrow();
        Container container = containerRepo.findById(dto.getContainerId()).orElseThrow();
        User registeredBy = userRepo.findById(dto.getRegisteredByUserId()).orElseThrow();

        // Only Worker & Owner can create milk entries
        if (registeredBy.getRole() != UserRole.WORKER &&
                registeredBy.getRole() != UserRole.OWNER) {
            throw new IllegalStateException("Only WORKER or OWNER can register milk.");
        }

        if (dto.getDate() == null) dto.setDate(LocalDate.now());

        Milk milk = mapper.fromCreateDto(dto, cow, container, registeredBy);

        // capacity check
        if (!container.hasSpaceFor(milk.getVolumeL())) {
            throw new IllegalStateException("Container has insufficient free capacity.");
        }

        // default approval = false
        milk.setApprovedForStorage(false);

        container.addMilk(milk);
        containerRepo.save(container);
        milkRepo.save(milk);

        return mapper.toDto(milk);
    }

    // ---------- UPDATE ----------
    @Override
    public MilkDtos.MilkDto update(MilkDtos.UpdateMilkDto dto) {
        Milk milk = milkRepo.findById(dto.getId()).orElseThrow();

        Container newContainer = null;
        if (dto.getContainerId() != null) {
            newContainer = containerRepo.findById(dto.getContainerId()).orElseThrow();
        }

        mapper.updateEntity(milk, dto, newContainer);
        milkRepo.save(milk);

        return mapper.toDto(milk);
    }

    // ---------- APPROVE STORAGE ----------
    @Override
    public MilkDtos.MilkDto approveForStorage(MilkDtos.ApproveMilkStorageDto dto) {

        Milk milk = milkRepo.findById(dto.getId()).orElseThrow();
        User user = userRepo.findById(dto.getApprovedByUserId()).orElseThrow();

        if (user.getRole() != UserRole.WORKER &&
                user.getRole() != UserRole.OWNER) {
            throw new IllegalStateException("Only WORKER or OWNER may approve storage.");
        }

        // Business rule: only PASSable milk may be approved
        if (milk.getMilkTestResult() != MilkTestResult.PASS) {
            throw new IllegalStateException("Only PASS test milk can be approved.");
        }

        milk.setApprovedForStorage(dto.isApprovedForStorage());
        milkRepo.save(milk);

        return mapper.toDto(milk);
    }

    // ---------- DELETE ----------
    @Override
    public void delete(long id) {
        milkRepo.deleteById(id);
    }

    // ---------- GET ----------
    @Override
    public MilkDtos.MilkDto get(long id) {
        return mapper.toDto(milkRepo.findById(id).orElseThrow());
    }

    // ---------- GET ALL ----------
    @Override
    public MilkDtos.MilkListDto getAll() {
        return mapper.toListDto(milkRepo.findAll());
    }

    // ---------- GET BY CONTAINER ----------
    @Override
    public MilkDtos.MilkListDto getByContainer(MilkDtos.MilkByContainerQuery dto) {
        Container container = containerRepo.findById(dto.getContainerId()).orElseThrow();
        List<Milk> list = milkRepo.findByContainer(container);
        return mapper.toListDto(list);
    }
}
