using HRP.Models;
using ProjectX.Models;

namespace ProjectX.Models;

public class User
{
    // user properties
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public string PhoneNumber { get; set; }

    public bool IsActive { get; set; } = true;

    //user navigation properties

    public Resume Resume { get; set; }

    public List<UserSkill> UserSkills { get; set; }

    public List<Application> Applications { get; set; }

    public List<Notification> Notifications { get; set; }

    public List<Review> Reviews { get; set; }
 

}