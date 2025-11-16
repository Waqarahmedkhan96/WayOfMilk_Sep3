
namespace Entities;

public class User
{
    public User() { }
    public User(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    public int Id { get; set; }
    public string UserName { get; set; } = "";
    public string Password { get; set; } = ""; // additional and need to discuss with the group
    public UserRole Role { get; set; } // Owner, Worker, Vet
}
