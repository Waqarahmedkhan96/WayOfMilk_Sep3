package sep3.entity;

import jakarta.persistence.*;

import java.util.ArrayList;
import java.util.List;

@Entity
@Table(name = "departments") // FIX: Plural name for consistency with "users" table
public class Department
{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    @Enumerated(EnumType.STRING) // FIX: Stores "QUARANTINE" instead of "0" in the DB
    private DepartmentType type;

    @Column(nullable = false, unique = true, name = "name")
    private String name;

    @OneToMany(mappedBy = "department")
    private List<Cow> cows = new ArrayList<>();

    @OneToMany(mappedBy = "fromDept")
    private List<TransferRecord> transferRecordsFrom = new ArrayList<>();

    @OneToMany(mappedBy = "toDept")
    private List<TransferRecord> transferRecordsTo = new ArrayList<>();

    protected Department() {}

    public Department(DepartmentType type, String name)
    {
        this.type = type;
        this.name = name;
    }


    public long getId() { return id; }
    public void setId(long id) { this.id = id; }
    public DepartmentType getType() { return type; }
    public void setType(DepartmentType type) { this.type = type; }
    public String getName() { return name; }
    public void setName(String name) { this.name = name; }

    public List<Cow> getCows() { return cows; }
    public List<TransferRecord> getTransferRecordsFrom() { return transferRecordsFrom; }
    public List<TransferRecord> getTransferRecordsTo() { return transferRecordsTo; }
}
