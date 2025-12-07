package sep3.entity;

import jakarta.persistence.*;
import sep3.entity.user.User;
import java.time.LocalDate;

@Entity
public class Milk
{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    private LocalDate date;
    private double volumeL;

    @Enumerated(EnumType.STRING)
    private MilkTestResult milkTestResult;

    private boolean approvedForStorage;

    @ManyToOne
    private Cow cow;

    @ManyToOne
    private Container container;

    @ManyToOne
    private User registeredBy;

    public Milk() {}

    public Milk(LocalDate date, double volumeL, MilkTestResult result,
                boolean approvedForStorage, Cow cow, Container container, User registeredBy)
    {
        this.date = date;
        this.volumeL = volumeL;
        this.milkTestResult = result;
        this.approvedForStorage = approvedForStorage;
        this.cow = cow;
        this.container = container;
        this.registeredBy = registeredBy;
    }

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public LocalDate getDate() {
        return date;
    }

    public void setDate(LocalDate date) {
        this.date = date;
    }

    public double getVolumeL() {
        return volumeL;
    }

    public void setVolumeL(double volumeL) {
        this.volumeL = volumeL;
    }

    public MilkTestResult getMilkTestResult() {
        return milkTestResult;
    }

    public void setMilkTestResult(MilkTestResult milkTestResult) {
        this.milkTestResult = milkTestResult;
    }

    public boolean isApprovedForStorage() {
        return approvedForStorage;
    }

    public void setApprovedForStorage(boolean approvedForStorage) {
        this.approvedForStorage = approvedForStorage;
    }

    public Cow getCow() {
        return cow;
    }

    public void setCow(Cow cow) {
        this.cow = cow;
    }

    public Container getContainer() {
        return container;
    }

    public void setContainer(Container container) {
        this.container = container;
    }

    public User getRegisteredBy() {
        return registeredBy;
    }

    public void setRegisteredBy(User registeredBy) {
        this.registeredBy = registeredBy;
    }


}
