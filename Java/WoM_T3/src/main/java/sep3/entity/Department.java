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

    protected Department() {}

    public Department(DepartmentType type)
    {
        this.type = type;
    }

    public long getId() { return id; }
    public DepartmentType getType() { return type; }
    public void setName(DepartmentType type) { this.type = type; }

    // made a new from setName to setType for clarity
    public void setType(DepartmentType type) { this.type = type; }

}
