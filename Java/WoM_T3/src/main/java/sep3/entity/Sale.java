package sep3.entity;

import jakarta.persistence.*;
import sep3.entity.user.User;

import java.time.LocalDateTime;

@Entity
public class Sale
{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    @ManyToOne
    private User createdBy;

    private LocalDateTime dateTime;

    @ManyToOne
    private Container container;

    private double quantityL;
    private double price;
    private boolean recallCase;

    @ManyToOne
    private Customer customer;

    public Sale() {}

    public Sale(User createdBy, LocalDateTime dateTime, Container container,
                double quantityL, double price, boolean recallCase, Customer customer)
    {
        this.createdBy = createdBy;
        this.dateTime = dateTime;
        this.container = container;
        this.quantityL = quantityL;
        this.price = price;
        this.recallCase = recallCase;
        this.customer = customer;
    }

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public User getCreatedBy() {
        return createdBy;
    }

    public void setCreatedBy(User createdBy) {
        this.createdBy = createdBy;
    }

    public LocalDateTime getDateTime() {
        return dateTime;
    }

    public void setDateTime(LocalDateTime dateTime) {
        this.dateTime = dateTime;
    }

    public Container getContainer() {
        return container;
    }

    public void setContainer(Container container) {
        this.container = container;
    }

    public double getQuantityL() {
        return quantityL;
    }

    public void setQuantityL(double quantityL) {
        this.quantityL = quantityL;
    }

    public double getPrice() {
        return price;
    }

    public void setPrice(double price) {
        this.price = price;
    }

    public boolean isRecallCase() {
        return recallCase;
    }

    public void setRecallCase(boolean recallCase) {
        this.recallCase = recallCase;
    }

    public Customer getCustomer() {
        return customer;
    }

    public void setCustomer(Customer customer) {
        this.customer = customer;
    }
}