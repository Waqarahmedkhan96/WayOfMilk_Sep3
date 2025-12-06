package sep3.RequestHandlers.SaleService;

import org.springframework.stereotype.Service;
import sep3.Mapping.SaleMapper;
import sep3.dao.CustomerDAO;
import sep3.dao.SaleDAO;
import sep3.dao.ContainerDAO;
import sep3.dao.UserDAO;
import sep3.dto.saleDTO.SaleCreationDTO;
import sep3.dto.saleDTO.SaleDataDTO;
import sep3.entity.Customer;
import sep3.entity.Container;
import sep3.entity.Sale;
import sep3.entity.user.User;

import java.time.LocalDateTime;
import java.util.List;
import java.util.stream.Collectors;

@Service
public class SaleDataService implements ISaleDataService
{
    private final SaleDAO saleDAO;
    private final CustomerDAO customerDAO;
    private final ContainerDAO containerDAO;
    private final UserDAO userDAO;

    public SaleDataService(SaleDAO saleDAO,
                           CustomerDAO customerDAO,
                           ContainerDAO containerDAO,
                           UserDAO userDAO)
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
            throw new IllegalArgumentException("customerId, containerId, quantityL, price and createdByUserId are required.");
        }

        Customer customer = customerDAO.findById(dto.getCustomerId())
                .orElseThrow(() -> new RuntimeException("Customer not found: " + dto.getCustomerId()));

        Container container = containerDAO.findById(dto.getContainerId())
                .orElseThrow(() -> new RuntimeException("Container not found: " + dto.getContainerId()));

        User createdBy = userDAO.findById(dto.getCreatedByUserId())
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

        Sale saved = saleDAO.save(sale);

        return SaleMapper.convertSaleToDto(saved);
    }

    // ========== READ ==========
    @Override
    public SaleDataDTO getSaleById(long id)
    {
        Sale sale = saleDAO.findById(id)
                .orElseThrow(() -> new RuntimeException("Sale not found: " + id));

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
                .orElseThrow(() -> new RuntimeException("Sale not found: " + dto.getId()));

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
            throw new RuntimeException("Sale not found: " + id);
        }
        saleDAO.deleteById(id);
    }
}
