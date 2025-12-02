package sep3.entity;


import jakarta.persistence.*;

import java.time.LocalDate;
import java.time.LocalDateTime;

@Entity
public class TransferRecord
{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    private LocalDateTime movedAt;

    @ManyToOne
    private Department fromDept;

    @ManyToOne
    private Department toDept;

    @ManyToOne
    private Department department;

    @ManyToOne
    private User requestedBy;

    @ManyToOne
    private User approvedByVet;

    @ManyToOne
    private Cow cow;

    protected TransferRecord() {}

    public TransferRecord(LocalDateTime movedAt, Department fromDept, Department toDept,
                          Department department, User requestedBy, User approvedByVet, Cow cow)
    {
        this.movedAt = movedAt;
        this.fromDept = fromDept;
        this.toDept = toDept;
        this.department = department;
        this.requestedBy = requestedBy;
        this.approvedByVet = approvedByVet;
        this.cow = cow;
    }

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public LocalDateTime getMovedAt() {
        return movedAt;
    }

    public void setMovedAt(LocalDateTime movedAt) {
        this.movedAt = movedAt;
    }

    public Department getFromDept() {
        return fromDept;
    }

    public void setFromDept(Department fromDept) {
        this.fromDept = fromDept;
    }

    public Department getToDept() {
        return toDept;
    }

    public void setToDept(Department toDept) {
        this.toDept = toDept;
    }

    public User getRequestedBy() {
        return requestedBy;
    }

    public void setRequestedBy(User requestedBy) {
        this.requestedBy = requestedBy;
    }

    public User getApprovedByVet() {
        return approvedByVet;
    }

    public void setApprovedByVet(User approvedByVet) {
        this.approvedByVet = approvedByVet;
    }

    public Department getDepartment() {
        return department;
    }

    public void setDepartment(Department department) {
        this.department = department;
    }

    public Cow getCow() {
        return cow;
    }

    public void setCow(Cow cow) {
        this.cow = cow;
    }

    
}
