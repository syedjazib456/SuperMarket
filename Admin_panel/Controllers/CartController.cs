using Admin_panel.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Admin_panel.Controllers
{
    public class CartController : Controller
    {
        private readonly Applicationdbcontext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CartController(Applicationdbcontext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _dbcontext = dbcontext;
            _webHostEnvironment = webHostEnvironment;
        }
        //public Product products { get; set; }
        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCart(Product pr, int id)
        {

            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var cart = await _dbcontext.CartProducts.Where(a => a.Product_id == id).FirstOrDefaultAsync(f => f.users_id == claims.Value);
            if (cart == null)
            {

                CartProduct carts = new CartProduct()
                {
                    Product_id = id,
                    users_id = claims.Value,
                    Quantity = pr.p_cart.Quantity
                };
                await _dbcontext.CartProducts.AddAsync(carts);
                await _dbcontext.SaveChangesAsync();
                TempData["msg2"] = "Your product is added in cart basket";
                return LocalRedirect("~/Web/PDetail/" + id);
            }
            else
            {
                cart.Quantity = pr.p_cart.Quantity;
                _dbcontext.CartProducts.Update(cart);
                await _dbcontext.SaveChangesAsync();
                TempData["msg2"] = "Your product is added in cart basket";
                return LocalRedirect("~/Web/PDetail/" + id);
            }
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCart2(int id)
        {

            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var pr = await _dbcontext.Products.Include(x => x.p_cat).FirstOrDefaultAsync(s => s.p_id == id);
            var cart = await _dbcontext.CartProducts.Include(x=>x.Productid).Where(a => a.Product_id == id).FirstOrDefaultAsync(f => f.users_id == claims.Value);
            if (cart == null)
            {

                CartProduct carts = new CartProduct()
                {
                    Product_id = id,
                    users_id = claims.Value,
                    Quantity = 1
                };
                await _dbcontext.CartProducts.AddAsync(carts);
                await _dbcontext.SaveChangesAsync();
                TempData["msg2"] = "Your product is added in cart basket";
                return LocalRedirect("~/Web/Shop/" + pr.p_cat.cat_id);
            }
            else
            {
                
                cart.Quantity = cart.Quantity + 1;
                _dbcontext.CartProducts.Update(cart);
                await _dbcontext.SaveChangesAsync();
                TempData["msg2"] = "Your product is added in cart basket";
                return LocalRedirect("~/Web/Shop/"+cart.Productid.p_category);
            }
        }
        public double sumcart(double quantity, double price)
        {
            return price * quantity;
        }
        [AutoValidateAntiforgeryToken]
        [Authorize]
        public async Task<IActionResult> CartList()
        {
            var claimidentity=(ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var cart= await _dbcontext.CartProducts.Include(s=>s.Productid).Where(x=>x.users_id==claims.Value).ToListAsync();
            foreach (var item in cart)
            {
                item.cart_price = sumcart(item.Quantity, item.Productid.p_price);
                item.cart_total = item.cart_total + item.cart_price;
                
            }
            return View(cart);
        }
        public async Task<IActionResult> Sub(int id)
        {
            var cart = await _dbcontext.CartProducts.FirstOrDefaultAsync(x=>x.Cart_id==id);
           
         
          
                cart.Quantity = cart.Quantity-1;
            if (cart.Quantity < 1)
            {
                TempData["msg2"] = "Range between 1 to 50";
                return RedirectToAction("CartList", "Cart");
            }
            else
            {
                _dbcontext.CartProducts.Update(cart);
                await _dbcontext.SaveChangesAsync();
               return RedirectToAction("CartList", "Cart");
            }
        }
        public async Task<IActionResult> Add(int id)
        {
            var cart = await _dbcontext.CartProducts.FirstOrDefaultAsync(x=>x.Cart_id==id);



            cart.Quantity = cart.Quantity + 1;
            if (cart.Quantity > 50)
            {
                TempData["msg2"] = "Range between 1 to 50";
                return RedirectToAction("CartList", "Cart");
            }
            else
            {
                _dbcontext.CartProducts.Update(cart);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction("CartList", "Cart");
            }
        }
        public async Task<IActionResult> CartRemove(int id)
        {
            var cart = await _dbcontext.CartProducts.FirstOrDefaultAsync(x => x.Cart_id == id);
                _dbcontext.CartProducts.Remove(cart);
                await _dbcontext.SaveChangesAsync();
                return RedirectToAction("CartList", "Cart");
            
        }
      
    }
}
