package sep3.service.impl;

import jakarta.persistence.EntityNotFoundException;
import org.springframework.stereotype.Service;
import sep3.mapping.SaleMapper;
import sep3.repository.ContainerRepository;
import sep3.repository.CustomerRepository;
import sep3.repository.SaleRepository;
import sep3.repository.UserRepository;
import sep3.dto.saleDTO.SaleCreationDTO;
import sep3.dto.saleDTO.SaleDataDTO;
import sep3.entity.Customer;
import sep3.entity.Container;
import sep3.entity.Sale;
import sep3.entity.user.User;
import sep3.service.interfaces.ISaleService;

import java.time.LocalDateTime;
import java.util.List;
import java.util.stream.Collectors;

@Service
public class SaleServiceImpl implements ISaleService
{
    private final SaleRepository saleDAO;
    private final CustomerRepository customerDAO;
    private final ContainerRepository containerDAO;
    private final UserRepository userDAO;

    public SaleServiceImpl(SaleRepository saleDAO,
                           CustomerRepository customerDAO,
                           ContainerRepository containerDAO,
                           UserRepository userDAO)
    {
        this.saleDAO = saleDAO;
        this.customerDAO = customerDAO;
        this.containerDAO = containerDAO;
        this.userDAO = userDAO;
    }

    // ========== CREATE ==========
    @Override
    public SaleDataDTO addSale(SaleCreationDTO dto)
    {
        if (dto.getCustomerId() == null ||
                dto.getContainerId() == null ||
                dto.getQuantityL() == null ||
                dto.getPrice() == null ||
                dto.getCreatedByUserId() == null)
        {
            throw new IllegalArgumentException(
                    "customerId, containerId, quantityL, price and createdByUserId are required.");
        }

        // validate quantity
        if (dto.getQuantityL() <= 0) {
            throw new IllegalArgumentException("quantityL must be positive.");
        }

        Customer customer = customerDAO.findById(dto.getCustomerId())
                .orElseThrow(() ->
                        new EntityNotFoundException("Customer not found with id: " + dto.getCustomerId()));

        Container container = containerDAO.findById(dto.getContainerId())
                .orElseThrow(() ->
                        new EntityNotFoundException("Container not found with id: " + dto.getContainerId()));

        User createdBy = userDAO.findById(dto.getCreatedByUserId())
                .orElseThrow(() ->
                        new EntityNotFoundException("User not found with id: " + dto.getCreatedByUserId()));

        // check container stock
        if (container.getOccupiedCapacityL() < dto.getQuantityL()) {
            throw new IllegalArgumentException(
                    "Container " + container.getId() + " does not have enough milk for this sale.");
        }

        LocalDateTime dateTime = (dto.getDateTime() != null)
                ? dto.getDateTime()
                : LocalDateTime.now();

        boolean recallCase = dto.getRecallCase() != null && dto.getRecallCase();

        Sale sale = new Sale();
        sale.setCustomer(customer);
        sale.setContainer(container);
        sale.setQuantityL(dto.getQuantityL());
        sale.setPrice(dto.getPrice());
        sale.setDateTime(dateTime);
        sale.setRecallCase(recallCase);
        sale.setCreatedBy(createdBy);

        container.removeMilk(dto.getQuantityL()); // decrease container stock
        containerDAO.save(container);             // save container change

        Sale saved = saleDAO.save(sale);

        return SaleMapper.convertSaleToDto(saved);
    }

    // ========== READ ==========
    @Override
    public SaleDataDTO getSaleById(long id)
    {
        Sale sale = saleDAO.findById(id)
                .orElseThrow(() ->
                        new EntityNotFoundException("Sale not found with id: " + id));

        return SaleMapper.convertSaleToDto(sale);
    }

    @Override
    public List<SaleDataDTO> getAllSales()
    {
        return saleDAO.findAll()
                .stream()
                .map(SaleMapper::convertSaleToDto)
                .collect(Collectors.toList());
    }

    // ========== UPDATE (partial) ==========
    @Override
    public SaleDataDTO updateSale(SaleDataDTO dto)
    {
        if (dto.getId() == null)
        {
            throw new IllegalArgumentException("Sale ID must be provided for update.");
        }

        Sale sale = saleDAO.findById(dto.getId())
                .orElseThrow(() ->
                        new EntityNotFoundException("Sale not found with id: " + dto.getId()));

        // Just basic updatable fields (we usually don't change relations here)
        if (dto.getQuantityL() != null) {
            sale.setQuantityL(dto.getQuantityL());
        }
        if (dto.getPrice() != null) {
            sale.setPrice(dto.getPrice());
        }
        if (dto.getDateTime() != null) {
            sale.setDateTime(dto.getDateTime());
        }
        if (dto.getRecallCase() != null) {
            sale.setRecallCase(dto.getRecallCase());
        }

        Sale updated = saleDAO.save(sale);
        return SaleMapper.convertSaleToDto(updated);
    }

    // ========== DELETE ==========
    @Override
    public void deleteSale(long id)
    {
        if (!saleDAO.existsById(id))
        {
            throw new EntityNotFoundException("Sale not found with id: " + id);
        }
        saleDAO.deleteById(id);
    }
}
