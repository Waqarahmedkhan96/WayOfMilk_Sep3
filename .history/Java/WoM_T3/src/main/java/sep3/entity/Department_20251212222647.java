package sep3.entity;

import jakarta.persistence.*;

@Entity
@Table(name = "departments") // FIX: Plural name for consistency with "users" table
public class Department
{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    @Enumerated(EnumType.STRING) // FIX: Stores "QUARANTINE" instead of "0" in the DB
    private DepartmentType type;

    @Column(nullable = false, unique = true)
    private String name;

    protected Department() {}

    public Department(DepartmentType type, String name)
    {
        this.type = type;
        this.name = name;
    }

    public String getName() { return name; }
    public void setName(String name) { this.name = name; }

    public long getId() { return id; }
    public void setId(long id) { this.id = id; }
    public DepartmentType getType() { return type; }
    //public void setName(DepartmentType type) { this.type = type; }

    // made a new from setName to setType for clarity
    public void setType(DepartmentType type) { this.type = type; }

}
