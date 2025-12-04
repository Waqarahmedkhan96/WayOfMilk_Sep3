package sep3.entity;

import jakarta.persistence.*;

@Entity
public class Department
{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    private DepartmentType type;

    protected Department() {}

    public Department(DepartmentType type)
    {
        this.type = type;
    }

    public long getId() { return id; }
    public DepartmentType getType() { return type; }
    public void setName(DepartmentType type) { this.type = type; }

}
