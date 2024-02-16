using Admin_panel.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

namespace Admin_panel.Controllers
{
    public class WebController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        
        private readonly Applicationdbcontext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public WebController(Applicationdbcontext dbcontext, IWebHostEnvironment webHostEnvironment,SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _dbcontext = dbcontext;
            _webHostEnvironment = webHostEnvironment;
           
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Profile()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var profile = await _dbcontext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == claims.Value);

            return View(profile);
        }
        [HttpPost]
        [ActionName("Profile")]
        public async Task<IActionResult> Profile2(ApplicationUser user)
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var profile = await _dbcontext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == claims.Value);
            profile.first_name = user.first_name;
            profile.last_name = user.last_name;
            profile.UserName = user.first_name;
            profile.PhoneNumber = user.PhoneNumber;
            profile.City= user.City;
            profile.Country= user.Country;
            profile.Town= user.Town;
            TempData["msg2"] = "Profile Updated..!!";

            
            _dbcontext.ApplicationUsers.Update(profile);
            await _dbcontext.SaveChangesAsync();


            return RedirectToAction("Profile", "Web");
        }
        public async Task<IActionResult> ChangeEmail()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var profile = await _dbcontext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == claims.Value);

            return View();

        }
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ApplicationUser user)
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var profile = await _dbcontext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == claims.Value);
            if (user.Email == null)
            {
                TempData["msg"] = "fill both fields ..!";
                return RedirectToAction("ChangeEmail", "Web");
            }
            else if (profile.Email != user.Email)
            {
                TempData["msg"] = "Email is not correct ..!";
                return RedirectToAction("ChangeEmail", "Web");
            }
            var check = await _dbcontext.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == user.new_email);
            if (check != null)
            {
                TempData["msg"] = "This Email is already exist.";
                    return RedirectToAction("ChangeEmail", "Web");
            }
            else {
                profile.NormalizedUserName = user.new_email.ToUpper();
                profile.Email = user.new_email;
                profile.NormalizedEmail = user.new_email.ToUpper();

                _dbcontext.ApplicationUsers.Update(profile);
                var result = await _dbcontext.SaveChangesAsync();
           
              
            TempData["msg"] = "Your Email is changed now.";
            await _signInManager.SignOutAsync();

          return LocalRedirect("/Identity/Account/Login2");
            }


        }
        public async Task<IActionResult> Shop(int id)
        {
            if (id == 0) { 
            Product pr = new Product()
            {
                products =await _dbcontext.Products.Include(x=>x.p_cat).Include(s=>s.p_spmart).ToListAsync(),
                categories=await _dbcontext.Categories.ToListAsync(),

            };
                foreach (var item in pr.categories)
                {
                    item.p_count = _dbcontext.Products.Where(x => x.p_category == item.cat_id).Count();
                }
                return View(pr);
            }
            else
            {
                Product pr = new Product()
                {
                    products = await _dbcontext.Products.Include(x => x.p_cat).Include(s=>s.p_spmart).Where(a => a.p_category == id).ToListAsync(),
                    categories = await _dbcontext.Categories.ToListAsync(),
                   
                };
                foreach (var item in pr.categories)
                {
                    item.p_count = _dbcontext.Products.Where(x=>x.p_category==item.cat_id).Count();
                }
                return View(pr);
            }
          
            
          

        }
       
        public async Task<IActionResult> PDetail(int id)
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claims = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims == null) {
                Product pr = new Product()
                {
                    p_product = await _dbcontext.Products.Include(x => x.p_cat).Include(s => s.p_spmart).FirstOrDefaultAsync(a => a.p_id == id),
                    
                };
                
                return View(pr);
                }
            else { 
            Product pr = new Product() {
                p_product = await _dbcontext.Products.Include(x => x.p_cat).Include(s => s.p_spmart).FirstOrDefaultAsync(a => a.p_id == id),
               p_cart=await _dbcontext.CartProducts.Where(x=>x.User.Id==claims.Value).FirstOrDefaultAsync(s=>s.Productid.p_id==id)
        };
            return View(pr);
            }
        }

    }
}
