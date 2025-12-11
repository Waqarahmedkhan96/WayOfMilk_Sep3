package sep3.entity;

import jakarta.persistence.*;
import java.util.ArrayList;
import java.util.List;

@Entity
public class Container {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    @Column(name = "capacity_l")
    private double capacityL;

    // how full now
    @Column(name = "occupied_capacity_l")
    private double occupiedCapacityL;

    // 1 Container  * Milk
    @OneToMany(mappedBy = "container",
            cascade = CascadeType.ALL,
            orphanRemoval = true)
    private List<Milk> milkList = new ArrayList<>();

    protected Container() {
    }

    public Container(double capacityL) {
        this.capacityL = capacityL;
        this.occupiedCapacityL = 0.0; // start empty
    }

    // ---------- getters / setters ----------

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

    public double getOccupiedCapacityL() {
        return occupiedCapacityL;
    }

    public void setOccupiedCapacityL(double occupiedCapacityL) {
        this.occupiedCapacityL = occupiedCapacityL;
    }

    public List<Milk> getMilkList() {
        return milkList;
    }

    public void setMilkList(List<Milk> milkList) {
        this.milkList = milkList;
    }

    // ---------- domain logic ----------

    // check free space
    public boolean hasSpaceFor(double volumeL) {
        return occupiedCapacityL + volumeL <= capacityL;
    }

    // add milk safely
    public void addMilk(Milk milk) {
        if (milk == null) return;

        double volume = milk.getVolumeL();
        if (!hasSpaceFor(volume)) {
            throw new IllegalStateException(
                    "Container " + id + " does not have enough free capacity.");
        }

        // keep both sides in sync
        milkList.add(milk);
        occupiedCapacityL += volume;
        milk.setContainer(this);
    }

    // remove quantity
    public void removeMilk(double volumeL) {
        if (volumeL <= 0) return;

        if (volumeL > occupiedCapacityL) {
            throw new IllegalStateException(
                    "Container " + id + " does not have enough milk to remove.");
        }

        occupiedCapacityL -= volumeL;
    }
}
