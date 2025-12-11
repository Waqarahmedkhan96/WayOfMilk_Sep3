package sep3.dto;

/**
 * DTOs for Container CRUD.
 */
public class ContainerDtos {

    // CreateContainer
    public static class CreateContainerDto {
        private double capacityL;

        public CreateContainerDto() {}

        public double getCapacityL() { return capacityL; }
        public void setCapacityL(double capacityL) { this.capacityL = capacityL; }
    }

    // GetContainer
    public static class ContainerDto {
        private long id;
        private double capacityL;
        private double occupiedCapacityL; // how full now

        public ContainerDto() {}

        public long getId() { return id; }
        public void setId(long id) { this.id = id; }

        public double getCapacityL() { return capacityL; }
        public void setCapacityL(double capacityL) { this.capacityL = capacityL; }

        public double getOccupiedCapacityL() { return occupiedCapacityL; }
        public void setOccupiedCapacityL(double occupiedCapacityL) { this.occupiedCapacityL = occupiedCapacityL; }
    }

    // GetContainerList
    public static class ContainerListDto {
        private java.util.List<ContainerDto> containers = new java.util.ArrayList<>();

        public ContainerListDto() {}

        public java.util.List<ContainerDto> getContainers() { return containers; }
        public void setContainers(java.util.List<ContainerDto> containers) { this.containers = containers; }
    }

    // UpdateContainer
    public static class UpdateContainerDto {
        private long id;
        private double capacityL;

        public UpdateContainerDto() {}

        public long getId() { return id; }
        public void setId(long id) { this.id = id; }

        public double getCapacityL() { return capacityL; }
        public void setCapacityL(double capacityL) { this.capacityL = capacityL; }
    }

    // DeleteContainerById
    public static class DeleteContainerDto {
        private long id;

        public DeleteContainerDto() {}

        public long getId() { return id; }
        public void setId(long id) { this.id = id; }
    }
}
