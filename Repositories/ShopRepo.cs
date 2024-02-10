using PayPal.Data;
using PayPal.Models;

namespace PayPal.Repositories
{
    public class ShopRepo
    {

        private readonly ApplicationDbContext _context;

        public ShopRepo(ApplicationDbContext context)
        {
            this._context = context;
        }


        public IEnumerable<Product> GetAll()
        {
            return _context.Products;
        }

    }
}
