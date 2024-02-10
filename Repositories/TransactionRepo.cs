using NuGet.Protocol.Plugins;
using PayPal.Data;
using PayPal.Models;
using PayPal.ViewModels;

namespace PayPal.Repositories
{
    public class TransactionRepo
    {

        private readonly ApplicationDbContext _context;

        public TransactionRepo(ApplicationDbContext context)
        {
            this._context = context;
        }


        public IEnumerable<TransactionViewModel> GetAll()
        {
            return _context.TransactionVMs;
        }


        public string Add(PayPalConfirmationModel payPalConfirmationModel)
        {
            var transaction = new TransactionViewModel()
            {
                paymentID = payPalConfirmationModel.TransactionId,
                create_time = DateTime.Now.ToString("dd-MM-yyyy, HH:mm"),
                payerName = payPalConfirmationModel.PayerName,
                payerEmail = payPalConfirmationModel.PayerEmail,
                amount = payPalConfirmationModel.Amount,
                currency = "CAD",
                paymentMethod = payPalConfirmationModel.PaymentMethod
            };
            _context.TransactionVMs.Add(transaction);
            _context.SaveChanges();
            return "Transaction added successfully";
        }


    }
}