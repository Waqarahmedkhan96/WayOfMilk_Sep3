package sep3.dto.saleDTO;

import java.time.LocalDateTime;

public class SaleCreationDTO {

    private Long customerId;
    private Long containerId;
    private Double quantityL;
    private Double price;
    private Long createdByUserId;
    private LocalDateTime dateTime;
    private Boolean recallCase;

    // Empty constructor FOR gRPC / Jackson
    public SaleCreationDTO() {
    }

    // Full constructor
    public SaleCreationDTO(Long customerId,
                           Long containerId,
                           Double quantityL,
                           Double price,
                           Long createdByUserId,
                           LocalDateTime dateTime) {
        this.customerId = customerId;
        this.containerId = containerId;
        this.quantityL = quantityL;
        this.price = price;
        this.createdByUserId = createdByUserId;
        this.dateTime = dateTime;
    }

    // Getters + Setters
    public Long getCustomerId() {
        return customerId;
    }

    public void setCustomerId(Long customerId) {
        this.customerId = customerId;
    }

    public Long getContainerId() {
        return containerId;
    }

    public void setContainerId(Long containerId) {
        this.containerId = containerId;
    }

    public Double getQuantityL() {
        return quantityL;
    }

    public void setQuantityL(Double quantityL) {
        this.quantityL = quantityL;
    }

    public Double getPrice() {
        return price;
    }

    public void setPrice(Double price) {
        this.price = price;
    }

    public Long getCreatedByUserId() {
        return createdByUserId;
    }

    public void setCreatedByUserId(Long createdByUserId) {
        this.createdByUserId = createdByUserId;
    }

    public LocalDateTime getDateTime() {
        return dateTime;
    }

    public void setDateTime(LocalDateTime dateTime) {
        this.dateTime = dateTime;
    }

    public Boolean getRecallCase() {
        return recallCase;
    }

    public void setRecallCase(Boolean recallCase) {
        this.recallCase = recallCase;
    }
}
