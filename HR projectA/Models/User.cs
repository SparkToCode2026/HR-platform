using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using HRP.Models;
using ProjectX.Models;

namespace ProjectX.Models;

public class User
{
    [Key]
    [JsonIgnore]
    // user properties
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    public int PhoneNumber { get; set; }

    public bool IsActive { get; set; } 
    public int? CompanyId { get; set; }
    [JsonIgnore]
    public company? Company { get; set; }


    //user navigation properties
[JsonIgnore]
    public Resume? Resume { get; set; }
[JsonIgnore]
    public List<UserSkill>? UserSkills { get; set; }
[JsonIgnore]
    public List<Application>? Applications { get; set; }
[JsonIgnore]
    public List<Notification>? Notifications { get; set; }
[JsonIgnore]
    public List<Review>? Reviews { get; set; }
 

}