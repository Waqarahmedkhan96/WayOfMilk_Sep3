package sep3.dto.saleDTO;

import java.time.LocalDateTime;

public class SaleDataDTO {

    private Long id;
    private Long customerId;
    private Long containerId;
    private Double quantityL;
    private Double price;
    private LocalDateTime dateTime;
    private Boolean recallCase;
    private Long createdByUserId;

    public SaleDataDTO() {
    }

    public SaleDataDTO(Long id, Long customerId, Long containerId,
                       Double quantityL, Double price,
                       LocalDateTime dateTime, Boolean recallCase,
                       Long createdByUserId) {
        this.id = id;
        this.customerId = customerId;
        this.containerId = containerId;
        this.quantityL = quantityL;
        this.price = price;
        this.dateTime = dateTime;
        this.recallCase = recallCase;
        this.createdByUserId = createdByUserId;
    }

    // Getters

    public Long getId() {
        return id;
    }

    public Long getCustomerId() {
        return customerId;
    }

    public Long getContainerId() {
        return containerId;
    }

    public Double getQuantityL() {
        return quantityL;
    }

    public Double getPrice() {
        return price;
    }

    public LocalDateTime getDateTime() {
        return dateTime;
    }

    public Boolean getRecallCase() {
        return recallCase;
    }

    public Long getCreatedByUserId() {
        return createdByUserId;
    }

    // Setters

    public void setId(Long id) {
        this.id = id;
    }

    public void setCustomerId(Long customerId) {
        this.customerId = customerId;
    }

    public void setContainerId(Long containerId) {
        this.containerId = containerId;
    }

    public void setQuantityL(Double quantityL) {
        this.quantityL = quantityL;
    }

    public void setPrice(Double price) {
        this.price = price;
    }

    public void setDateTime(LocalDateTime dateTime) {
        this.dateTime = dateTime;
    }

    public void setRecallCase(Boolean recallCase) {
        this.recallCase = recallCase;
    }

    public void setCreatedByUserId(Long createdByUserId) {
        this.createdByUserId = createdByUserId;
    }
}
