using System.ComponentModel.DataAnnotations;

namespace PayPal.ViewModels
{
    public class UserRoleVM
    {
        [Key]
        [Display(Name = "ID")]
        public int? Id { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "RoleName Name")]
        public string RoleName { get; set; }
    }
}
