using System.ComponentModel.DataAnnotations;

namespace PayPal.ViewModels
{
    public class TransactionViewModel
    {

        [Display(Name = "Payment ID")]
        [Key] // Define primary key.
        public string? paymentID { get; set; }

        [Display(Name = "Created")]
        public string? create_time { get; set; }

        [Display(Name = "Name")]
        public string? payerName { get; set; }

        [Display(Name = "Email")]
        public string? payerEmail { get; set; }

        [Display(Name = "Amount")]
        public string? amount { get; set; }
        public string? currency { get; set; }

        [Display(Name = "MOP")]
        public string? paymentMethod { get; set; }



    }
}
