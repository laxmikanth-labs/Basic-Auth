using System.ComponentModel.DataAnnotations;

namespace BasicAuth.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
    
        [Required]
        [StringLength(50, ErrorMessage ="First Name cannot exceed 50 charecters")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 charecters")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "Email cannot exceed 50 charecters")]
        public string Email { get; set; }
        [Required]
        [MinLength(6,ErrorMessage ="Password must be at least 6 characters long")]
        public string Password { get; set; }
    }
}
