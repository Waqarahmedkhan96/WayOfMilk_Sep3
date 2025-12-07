package sep3.dto;

import java.time.LocalDate;
import sep3.entity.MilkTestResult;

/**
 * DTOs for Milk records (collections).
 */
public class MilkDtos {

    // CreateMilk
    public static class CreateMilkDto {
        private LocalDate date;
        private double volumeL;
        private MilkTestResult testResult;
        private long cowId;
        private long containerId;
        private long registeredByUserId;

        public CreateMilkDto() {}

        public LocalDate getDate() { return date; }
        public void setDate(LocalDate date) { this.date = date; }

        public double getVolumeL() { return volumeL; }
        public void setVolumeL(double volumeL) { this.volumeL = volumeL; }

        public MilkTestResult getTestResult() { return testResult; }
        public void setTestResult(MilkTestResult testResult) { this.testResult = testResult; }

        public long getCowId() { return cowId; }
        public void setCowId(long cowId) { this.cowId = cowId; }

        public long getContainerId() { return containerId; }
        public void setContainerId(long containerId) { this.containerId = containerId; }

        public long getRegisteredByUserId() { return registeredByUserId; }
        public void setRegisteredByUserId(long registeredByUserId) { this.registeredByUserId = registeredByUserId; }
    }

    // GetMilk
    public static class MilkDto {
        private long id;
        private LocalDate date;
        private double volumeL;
        private MilkTestResult testResult;
        private long cowId;
        private long containerId;

        public MilkDto() {}

        public long getId() { return id; }
        public void setId(long id) { this.id = id; }

        public LocalDate getDate() { return date; }
        public void setDate(LocalDate date) { this.date = date; }

        public double getVolumeL() { return volumeL; }
        public void setVolumeL(double volumeL) { this.volumeL = volumeL; }

        public MilkTestResult getTestResult() { return testResult; }
        public void setTestResult(MilkTestResult testResult) { this.testResult = testResult; }

        public long getCowId() { return cowId; }
        public void setCowId(long cowId) { this.cowId = cowId; }

        public long getContainerId() { return containerId; }
        public void setContainerId(long containerId) { this.containerId = containerId; }
    }

    // GetMilkList
    public static class MilkListDto {
        private java.util.List<MilkDto> milkRecords = new java.util.ArrayList<>();

        public MilkListDto() {}

        public java.util.List<MilkDto> getMilkRecords() { return milkRecords; }
        public void setMilkRecords(java.util.List<MilkDto> milkRecords) { this.milkRecords = milkRecords; }
    }

    // UpdateMilk
    public static class UpdateMilkDto {
        private long id;
        private LocalDate date;
        private Double volumeL;
        private MilkTestResult testResult;
        private Long containerId;

        public UpdateMilkDto() {}

        public long getId() { return id; }
        public void setId(long id) { this.id = id; }

        public LocalDate getDate() { return date; }
        public void setDate(LocalDate date) { this.date = date; }

        public Double getVolumeL() { return volumeL; }
        public void setVolumeL(Double volumeL) { this.volumeL = volumeL; }

        public MilkTestResult getTestResult() { return testResult; }
        public void setTestResult(MilkTestResult testResult) { this.testResult = testResult; }

        public Long getContainerId() { return containerId; }
        public void setContainerId(Long containerId) { this.containerId = containerId; }
    }

    // DeleteMilkById
    public static class DeleteMilkDto {
        private long id;

        public DeleteMilkDto() {}

        public long getId() { return id; }
        public void setId(long id) { this.id = id; }
    }

    // GetMilkByContainer
    public static class MilkByContainerQuery {
        private long containerId;

        public MilkByContainerQuery() {}

        public long getContainerId() { return containerId; }
        public void setContainerId(long containerId) { this.containerId = containerId; }
    }
}
