using Admin_panel.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Admin_panel.Controllers
{
    public class PlaceOrderController : Controller
    {
        private readonly Applicationdbcontext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PlaceOrderController(Applicationdbcontext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _dbcontext = dbcontext;
            _webHostEnvironment = webHostEnvironment;
        }
        public double sumcart(double quantity, double price)
        {
            return price * quantity;
        }
        [BindProperty]
        public SummaryVM SummaryVM { get; set; }
        public Order Order { get; set; }
        public async Task<IActionResult> Checkout()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            SummaryVM = new SummaryVM
            {
                listCart = await _dbcontext.CartProducts.Include(x => x.Productid).Where(x => x.users_id == claims.Value).ToListAsync(),
                Order = new()
            };
            foreach (var item in SummaryVM.listCart)
            {
                item.cart_price = sumcart(item.Quantity, item.Productid.p_price);
                item.cart_total = item.cart_total + item.cart_price;
            }
            SummaryVM.Order.app_user = await _dbcontext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == claims.Value);

            SummaryVM.Order.first_name = SummaryVM.Order.app_user.first_name.ToUpper();
            SummaryVM.Order.last_name = SummaryVM.Order.app_user.last_name.ToUpper();
            SummaryVM.Order.Contact_No = SummaryVM.Order.app_user.PhoneNumber;
            SummaryVM.Order.City = SummaryVM.Order.app_user.City;
            SummaryVM.Order.Town = SummaryVM.Order.app_user.Town;
            SummaryVM.Order.Country = SummaryVM.Order.app_user.Country;
            SummaryVM.Order.st_rate = 150;
            return View(SummaryVM);
        }
        //[AutoValidateAntiforgeryToken]
        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> OrderPlace()
        //{
        //    var claimidentity = (ClaimsIdentity)User.Identity;
        //    var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
        //    SummaryVM = new SummaryVM
        //    {
        //        listCart = await _dbcontext.CartProducts.Include(x => x.Productid).Where(x => x.users_id == claims.Value).ToListAsync(),
        //        Order = new()
        //    };
        //    foreach (var item in SummaryVM.listCart)
        //    {
        //        item.cart_price = sumcart(item.Quantity, item.Productid.p_price);
        //        item.cart_total = item.cart_total + item.cart_price;
        //    }
        //    SummaryVM.Order.first_name = SummaryVM.Order.first_name;
        //    SummaryVM.Order.last_name = SummaryVM.Order.last_name.ToUpper();
        //    SummaryVM.Order.Contact_No = SummaryVM.Order.Contact_No;
        //    SummaryVM.Order.City = SummaryVM.Order.City;
        //    SummaryVM.Order.Town = SummaryVM.Order.Town;
        //    SummaryVM.Order.Country = SummaryVM.Order.Country;
        //    SummaryVM.Order.Order_total = SummaryVM.Order.Order_total;
        //    SummaryVM.Order.payment_status=StaticDetails.Payment_Status_Pending;
        //    SummaryVM.Order.Order_status=StaticDetails.Order_Status_Pending;
        //    SummaryVM.Order.Payment_date=System.DateTime.Now;



        //}
    }
}
