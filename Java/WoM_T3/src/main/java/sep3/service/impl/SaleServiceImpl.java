package sep3.service.impl;

import org.springframework.stereotype.Service;
import sep3.mapping.SaleMapper;
import sep3.repository.ContainerRepository;
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
    private final sep3.dao.SaleRepository saleRepository;
    private final sep3.dao.CustomerRepository customerRepository;
    private final ContainerRepository containerRepository;
    private final UserRepository userRepository;

    public SaleServiceImpl(sep3.dao.SaleRepository saleRepository,
                           sep3.dao.CustomerRepository customerRepository,
                           ContainerRepository containerRepository,
                           UserRepository userRepository)
    {
        this.saleRepository = saleRepository;
        this.customerRepository = customerRepository;
        this.containerRepository = containerRepository;
        this.userRepository = userRepository;
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
            throw new IllegalArgumentException("customerId, containerId, quantityL, price and createdByUserId are required.");
        }

        Customer customer = customerRepository.findById(dto.getCustomerId())
                .orElseThrow(() -> new RuntimeException("Customer not found: " + dto.getCustomerId()));

        Container container = containerRepository.findById(dto.getContainerId())
                .orElseThrow(() -> new RuntimeException("Container not found: " + dto.getContainerId()));

        User createdBy = userRepository.findById(dto.getCreatedByUserId())
                .orElseThrow(() -> new RuntimeException("User not found: " + dto.getCreatedByUserId()));

        LocalDateTime dateTime = dto.getDateTime() != null
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

        Sale saved = saleRepository.save(sale);

        return SaleMapper.convertSaleToDto(saved);
    }

    // ========== READ ==========
    @Override
    public SaleDataDTO getSaleById(long id)
    {
        Sale sale = saleRepository.findById(id)
                .orElseThrow(() -> new RuntimeException("Sale not found: " + id));

        return SaleMapper.convertSaleToDto(sale);
    }

    @Override
    public List<SaleDataDTO> getAllSales()
    {
        return saleRepository.findAll()
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

        Sale sale = saleRepository.findById(dto.getId())
                .orElseThrow(() -> new RuntimeException("Sale not found: " + dto.getId()));

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

        Sale updated = saleRepository.save(sale);
        return SaleMapper.convertSaleToDto(updated);
    }

    // ========== DELETE ==========
    @Override
    public void deleteSale(long id)
    {
        if (!saleRepository.existsById(id))
        {
            throw new RuntimeException("Sale not found: " + id);
        }
        saleRepository.deleteById(id);
    }
}
