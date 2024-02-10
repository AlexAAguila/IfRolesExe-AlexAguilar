using System.ComponentModel.DataAnnotations;

namespace PayPal.ViewModels
{
    public class UserVM
    {
        [Key]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        //[Required]
        //[Display(Name = "RoleName Name")]
        //public string RoleName { get; set; }
    }
}
