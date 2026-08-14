using System.ComponentModel.DataAnnotations;

namespace ProjectX.DTOs;

    public class CreateCompanyDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

// Used ONLY by Admins to update verification status
    public class UpdateCompanyVerificationDto
    {
        public bool IsVerified { get; set; }
    }
