using HRP.Models;

namespace ProjectX.Models
{
    public class company
    {
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
        public List<Department> Departments { get; set; }

        public List<JobPosting> JobPostings { get; set; }

        public List<Notification> Notifications { get; set; }

         
    } }
