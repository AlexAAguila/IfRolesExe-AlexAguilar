using PayPal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using PayPal.Data;
using PayPal.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PayPal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IConfiguration configuration)
        {
            _logger = logger;
            this._context = db;
            _configuration = configuration;
        }

        // Home page shows list of items.
        // Item price is set through the ViewBag.
        public IActionResult Index()
        {
            var adminUserName = _configuration["adminLogin:Username"];
            var adminPassword = _configuration["AdminLogin:Password"];
            var connectionString = _configuration["ConnectionStrings:DefaultConnection"];

            if (User.Identity.IsAuthenticated)
            {
                MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_context);
                string userName = User.Identity.Name;
            //    HttpContext.Session.SetString("UserName", userName);
                string name = registeredUserRepo.GetUserName(userName);
                ViewBag.UserName = name;
            }
        
            return View("Index", "3.55|CAD");

        }

        // Home page shows list of items.
        // Item price is set through the ViewBag.
        public IActionResult Transactions()
        {
            DbSet<IPN> items = _context.IPNs;

            return View(items);
        }

        // This method receives and stores
        // the Paypal transaction details.
        [HttpPost]
        public JsonResult PaySuccess([FromBody] IPN ipn)
        {
            try
            {
                _context.IPNs.Add(ipn);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            return Json(ipn);
        }
        // Home page shows list of items.
        // Item price is set through the ViewBag.
        public IActionResult Confirmation(string confirmationId)
        {
            IPN transaction =
            _context.IPNs.FirstOrDefault(t => t.paymentID == confirmationId);

            return View("Confirmation", transaction);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




        [Authorize]
        public IActionResult SecureArea()
        {
            MyRegisteredUserRepo registeredUserRepo = new MyRegisteredUserRepo(_context);
            string userName = User.Identity.Name;
            string name = registeredUserRepo.GetUserName(userName);
            ViewBag.UserName = name;    
            var registeredUser = _context.MyRegisteredUsers.FirstOrDefault(ru => ru.Email == userName);

            return View(registeredUser);
        }

        public IActionResult PayPalConfirmation(PayPalConfirmationModel payPalConfirmationModel)
        {
            TransactionRepo transactionRepo = new TransactionRepo(_context);
            transactionRepo.Add(payPalConfirmationModel);
            

            return View(payPalConfirmationModel);
        }


    }
}
