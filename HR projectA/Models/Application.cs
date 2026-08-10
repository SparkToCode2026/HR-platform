
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AutoMapper.Configuration.Annotations;
using ProjectX.Models;

namespace HRP.Models;

public class Application
{
    [Key]
    [JsonIgnore]
    public int ApplicationID { get; set; }
    public DateTime AppliedAt { get; set; }
    public string ApplicationStatus { get; set; } = string.Empty;
   
    [ForeignKey("UserA")]
    public int UserId { get; set; }

    [JsonIgnore] 
    public User? UserA { get; set; }

    [ForeignKey("JobPosting")]
    public int JobPostingID { get; set; }
    [JsonIgnore]
    public JobPosting? JobPosting { get; set; }
    [JsonIgnore]
    public List<Interview>? Interviews { get; set; }
    [JsonIgnore]
    public Offer? Offer { get; set; }
    [JsonIgnore]
    public List<Review>? _reviews { get; set; }
     
}
