using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPal.Data;
using PayPal.Repositories;
using PayPal.ViewModels;

namespace PayPal.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public TransactionController(ILogger<HomeController> logger, ApplicationDbContext db, IConfiguration configuration)
        {
            _logger = logger;
            this._context = db;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_context);
            string userName = User.Identity.Name;
            string name = registeredUserRepo.GetUserName(userName);
            ViewBag.UserName = name;

            TransactionRepo transactionRepo = new TransactionRepo(_context);


        

            return View(transactionRepo.GetAll());

        }
    }
}
