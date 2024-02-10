using Microsoft.AspNetCore.Mvc;
using PayPal.Data;
using PayPal.Repositories;
using PayPal.ViewModels;
using PayPal.Models;


namespace PayPal.Controllers
{
    public class ShopController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public ShopController(ILogger<HomeController> logger, ApplicationDbContext db, IConfiguration configuration)
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

            ShopRepo shopRepo = new ShopRepo(_context);
            return View(shopRepo.GetAll());
                    
        }

    }
}
