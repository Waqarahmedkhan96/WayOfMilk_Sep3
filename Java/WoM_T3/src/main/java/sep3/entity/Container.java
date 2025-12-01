package sep3.entity;

import jakarta.persistence.*;

@Entity
public class Container
{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    private double capacityL;

    protected Container() {}

    public Container(double capacityL)
    {
        this.capacityL = capacityL;
    }

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public double getCapacityL() {
        return capacityL;
    }

    public void setCapacityL(double capacityL) {
        this.capacityL = capacityL;
    }
}
