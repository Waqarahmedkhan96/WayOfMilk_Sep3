package sep3.dto.customerDTO;

public class CustomerDataDTO {

    private Long id;
    private String companyName;
    private String phoneNo;
    private String email;
    private String companyCVR;

    public CustomerDataDTO() {
    }

    public CustomerDataDTO(Long id, String companyName, String phoneNo,
                           String email, String companyCVR) {
        this.id = id;
        this.companyName = companyName;
        this.phoneNo = phoneNo;
        this.email = email;
        this.companyCVR = companyCVR;
    }

    // Getters

    public Long getId() {
        return id;
    }

    public String getCompanyName() {
        return companyName;
    }

    public String getPhoneNo() {
        return phoneNo;
    }

    public String getEmail() {
        return email;
    }

    public String getCompanyCVR() {
        return companyCVR;
    }

    // Setters

    public void setId(Long id) {
        this.id = id;
    }

    public void setCompanyName(String companyName) {
        this.companyName = companyName;
    }

    public void setPhoneNo(String phoneNo) {
        this.phoneNo = phoneNo;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public void setCompanyCVR(String companyCVR) {
        this.companyCVR = companyCVR;
    }
}
