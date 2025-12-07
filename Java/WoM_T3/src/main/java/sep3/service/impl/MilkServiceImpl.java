package sep3.service.impl;

import org.springframework.stereotype.Service;
import sep3.dto.MilkDtos;
import sep3.entity.Container;
import sep3.entity.Cow;
import sep3.entity.Milk;
import sep3.entity.MilkTestResult;
import sep3.entity.user.User;
import sep3.mapping.MilkMapper;
import sep3.dao.ContainerDAO;
import sep3.dao.CowDAO;
import sep3.dao.MilkDAO;
import sep3.dao.UserDAO;
import sep3.service.interfaces.ICowService;
import sep3.service.interfaces.IMilkService;

import java.time.LocalDate;
import java.util.List;

@Service
public class MilkServiceImpl implements IMilkService {

    private final MilkDAO milkDao;
    private final CowDAO cowDao;
    private final ContainerDAO containerDao;
    private final UserDAO userDao;
    private final MilkMapper mapper;
    private final ICowService cowService; // interface DI

    public MilkServiceImpl(MilkDAO milkDao, CowDAO cowDao, ContainerDAO containerDao, UserDAO userDao, MilkMapper mapper, ICowService cowService) {
        this.milkDao = milkDao;
        this.cowDao = cowDao;
        this.containerDao = containerDao;
        this.userDao = userDao;
        this.mapper = mapper;
        this.cowService = cowService;
    }

    // CREATE MILK RECORD
    @Override
    public MilkDtos.MilkDto create(MilkDtos.CreateMilkDto dto) {

        Cow cow = cowDao.findById(dto.getCowId()).orElseThrow();
        Container container = containerDao.findById(dto.getContainerId()).orElseThrow();
        User registeredBy = userDao.findById(dto.getRegisteredByUserId()).orElseThrow();

        if (dto.getDate() == null) {
            dto.setDate(LocalDate.now());
        }

        Milk milk = mapper.fromCreateDto(dto, cow, container, registeredBy);
        milkDao.save(milk);

//        // AUTO-QUARANTINE for BAD test result
//        if (dto.getTestResult() == MilkTestResult.FAIL) {
//            cowService.autoMoveToQuarantine(cow.getId(), registeredBy.getId());
//        }

        return mapper.toDto(milk);
    }

    // UPDATE
    @Override
    public MilkDtos.MilkDto update(MilkDtos.UpdateMilkDto dto) {
        Milk milk = milkDao.findById(dto.getId()).orElseThrow();

        Container container = null;
        if (dto.getContainerId() != null) {
            container = containerDao.findById(dto.getContainerId()).orElseThrow();
        }

        mapper.updateEntity(milk, dto, container);
        milkDao.save(milk);

        return mapper.toDto(milk);
    }

    // DELETE
    @Override
    public void delete(long id) {
        milkDao.deleteById(id);
    }

    // GET ONE
    @Override
    public MilkDtos.MilkDto get(long id) {
        return mapper.toDto(milkDao.findById(id).orElseThrow());
    }

    // GET ALL
    @Override
    public MilkDtos.MilkListDto getAll() {
        return mapper.toListDto(milkDao.findAll());
    }

    // GET BY CONTAINER (using Container entity)
    @Override
    public MilkDtos.MilkListDto getByContainer(MilkDtos.MilkByContainerQuery dto) {
        Container container = containerDao.findById(dto.getContainerId()).orElseThrow();
        List<Milk> list = milkDao.findByContainer(container);
        return mapper.toListDto(list);
    }
}
