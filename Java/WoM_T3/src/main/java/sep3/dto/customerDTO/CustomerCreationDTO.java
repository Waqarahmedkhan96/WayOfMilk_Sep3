package sep3.dto.customerDTO;

public class CustomerCreationDTO
{
    private String companyName;
    private String phoneNo;
    private String email;
    private String companyCVR;

    public CustomerCreationDTO() {}

    public CustomerCreationDTO(String companyName,
                               String phoneNo,
                               String email,
                               String companyCVR,
                               Long registeredByUserId)
    {
        this.companyName = companyName;
        this.phoneNo = phoneNo;
        this.email = email;
        this.companyCVR = companyCVR;

    }

    public String getCompanyName() { return companyName; }
    public void setCompanyName(String companyName) { this.companyName = companyName; }

    public String getPhoneNo() { return phoneNo; }
    public void setPhoneNo(String phoneNo) { this.phoneNo = phoneNo; }

    public String getEmail() { return email; }
    public void setEmail(String email) { this.email = email; }

    public String getCompanyCVR() { return companyCVR; }
    public void setCompanyCVR(String companyCVR) { this.companyCVR = companyCVR; }

    public Long getRegisteredByUserId() { return registeredByUserId; }
    public void setRegisteredByUserId(Long registeredByUserId) { this.registeredByUserId = registeredByUserId; }
}
