package sep3.dto;

import java.time.LocalDate;
import sep3.entity.MilkTestResult;

/**
 * DTOs for Milk records (collections).
 */
public class MilkDtos {

    // ------------------- CREATE -------------------
    public static class CreateMilkDto {
        private LocalDate date;
        private double volumeL;
        private MilkTestResult testResult;
        private long cowId;
        private long containerId;
        private long registeredByUserId;

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

    // ------------------- SINGLE MILK DTO -------------------
    public static class MilkDto {
        private long id;
        private LocalDate date;
        private double volumeL;
        private MilkTestResult testResult;
        private long cowId;
        private long containerId;
        private boolean approvedForStorage;

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

        public boolean isApprovedForStorage() { return approvedForStorage; }
        public void setApprovedForStorage(boolean approvedForStorage) { this.approvedForStorage = approvedForStorage; }
    }

    // ------------------- LIST DTO -------------------
    public static class MilkListDto {
        private java.util.List<MilkDto> milkRecords = new java.util.ArrayList<>();
        public java.util.List<MilkDto> getMilkRecords() { return milkRecords; }
        public void setMilkRecords(java.util.List<MilkDto> milkRecords) { this.milkRecords = milkRecords; }
    }

    // ------------------- UPDATE -------------------
    public static class UpdateMilkDto {
        private long id;
        private LocalDate date;
        private Double volumeL;
        private MilkTestResult testResult;
        private Long containerId;

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

    // ------------------- APPROVE STORAGE -------------------
    public static class ApproveMilkStorageDto {
        private long id;
        private long approvedByUserId;
        private boolean approvedForStorage;

        public long getId() { return id; }
        public void setId(long id) { this.id = id; }

        public long getApprovedByUserId() { return approvedByUserId; }
        public void setApprovedByUserId(long approvedByUserId) { this.approvedByUserId = approvedByUserId; }

        public boolean isApprovedForStorage() { return approvedForStorage; }
        public void setApprovedForStorage(boolean approvedForStorage) { this.approvedForStorage = approvedForStorage; }
    }

    // ------------------- QUERY -------------------
    public static class MilkByContainerQuery {
        private long containerId;

        public long getContainerId() { return containerId; }
        public void setContainerId(long containerId) { this.containerId = containerId; }
    }
}
