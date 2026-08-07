using System.Text.Json.Serialization;
using HRP.Models;

namespace ProjectX.Models
{
    public class company
    {[JsonIgnore]
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string CompanyDescription { get; set; }

        public string Industry { get; set; }

        public string CompanyWebsite { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string LocationStreet { get; set; }
        
        
        public bool IsVerified  { get; set; }


        //nevegation property
        [JsonIgnore]
        public List<Department> Departments { get; set; }
        [JsonIgnore]
        public List<JobPosting> JobPostings { get; set; }
        [JsonIgnore]
        public List<Notification> Notifications { get; set; }

         
    } }
