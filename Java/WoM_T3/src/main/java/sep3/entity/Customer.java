package sep3.entity;

import jakarta.persistence.*;
import sep3.entity.user.User;

import java.util.ArrayList;
import java.util.List;

@Entity
public class Customer
{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private long id;

    private String companyName;
    private String phoneNo;
    private String email;
    private String companyCVR;

    // --- who registered the customer (User 1..* Customer) ---
    @ManyToOne
    @JoinColumn(name = "registered_by_id")   // FK -> users.id
    private User registeredBy;

    // --- 1 Customer .. * Sales ---
    @OneToMany(mappedBy = "customer", cascade = CascadeType.ALL)
    private List<Sale> sales = new ArrayList<>();

    protected Customer() {}

    public Customer(String companyName, String phoneNo, String email, String companyCVR)
    {
        this.companyName = companyName;
        this.phoneNo = phoneNo;
        this.email = email;
        this.companyCVR = companyCVR;
    }

    public Customer(String companyName,
                    String phoneNo,
                    String email,
                    String companyCVR,
                    User registeredBy)
    {
        this.companyName = companyName;
        this.phoneNo = phoneNo;
        this.email = email;
        this.companyCVR = companyCVR;
        this.registeredBy = registeredBy;
    }

    // ---------- getters / setters ----------

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public String getCompanyName() {
        return companyName;
    }

    public void setCompanyName(String companyName) {
        this.companyName = companyName;
    }

    public String getPhoneNo() {
        return phoneNo;
    }

    public void setPhoneNo(String phoneNo) {
        this.phoneNo = phoneNo;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getCompanyCVR() {
        return companyCVR;
    }

    public void setCompanyCVR(String companyCVR) {
        this.companyCVR = companyCVR;
    }

    public User getRegisteredBy() {
        return registeredBy;
    }

    public void setRegisteredBy(User registeredBy) {
        this.registeredBy = registeredBy;
    }

    public List<Sale> getSales() {
        return sales;
    }

    // helper methods (optional but nice)
    public void addSale(Sale sale) {
        sales.add(sale);
        sale.setCustomer(this);
    }

    public void removeSale(Sale sale) {
        sales.remove(sale);
        sale.setCustomer(null);
    }
}
