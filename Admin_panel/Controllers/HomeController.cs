using Admin_panel.Models;
using Admin_panel.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Admin_panel.Controllers
{
    public class HomeController : Controller
    {
        private readonly Applicationdbcontext _dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(Applicationdbcontext dbcontext, IWebHostEnvironment webHostEnvironment)
        {
            _dbcontext = dbcontext;
            _webHostEnvironment = webHostEnvironment;
        }
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public Task< IActionResult> Index()
        {
            ViewBag.pcount= _dbcontext.Products.Count();
            ViewBag.ucount=_dbcontext.ApplicationUsers.Count();
            return Task.FromResult<IActionResult>(View());
        }
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category cat)
        {
            if (ModelState.IsValid) { 
           await _dbcontext.Categories.AddAsync(cat);
           await _dbcontext.SaveChangesAsync();
            TempData["success"] = "Category is added successfully";
                return RedirectToAction("CategoryList", "Home");
            }
            return View();
            
        }
        public IActionResult CategoryList()
        {
            var list = _dbcontext.Categories.ToList();
            return View(list);
        }
        public async Task<IActionResult> Cat_Edit(int id)
        {
            var list =await _dbcontext.Categories.FindAsync(id);
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> Cat_Edit(Category cat, int id)
        {
            
            if (ModelState.IsValid)
            {
               _dbcontext.Categories.Update(cat);
               await _dbcontext.SaveChangesAsync();
                TempData["success"] = "Changes are applied successfully";
                return RedirectToAction("CategoryList", "Home");   
            }
            return View();
        }
        public async Task<IActionResult> Cat_Delete(int id)
        {
            var list =await _dbcontext.Categories.FindAsync(id);
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> Cat_Delete(Category cat, int id)
        {

            var list =await _dbcontext.Categories.FindAsync(id);
            _dbcontext.Categories.Remove(list);
              await  _dbcontext.SaveChangesAsync();
                TempData["done"] = "Category is deleted successfully";
                return RedirectToAction("CategoryList", "Home");
            
         
        }
        public IActionResult AddSuperMarket()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSuperMarket(SuperMarket sp)
        {
            if (ModelState.IsValid)
            {
               await _dbcontext.SuperMarkets.AddAsync(sp);
               await _dbcontext.SaveChangesAsync();
                TempData["success"] = "Super Market is added successfully";
                return RedirectToAction("SuperMarketList", "Home");
            }
            return View();
        }
        public IActionResult SuperMarketList()
        {
            var list = _dbcontext.SuperMarkets.ToList();
            return View(list);
        }
       
             public IActionResult SPEdit(int id)
        {
            var detail = _dbcontext.SuperMarkets.Find(id);
            return View(detail);
        }
        [HttpPost]
        public async Task<IActionResult> SPEdit(SuperMarket sp, int id)
        {
            if (ModelState.IsValid)
            {
                _dbcontext.SuperMarkets.Update(sp);
                await _dbcontext.SaveChangesAsync();
                TempData["success"] = "Super Market is Updated successfully";
                return RedirectToAction("SuperMarketList", "Home");
            }
            return View();
        }
       
            public async Task<IActionResult> SPDelete(int id)
        {
            var list = await _dbcontext.SuperMarkets.FindAsync(id);
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> SPDelete(SuperMarket sp, int id)
        {

            var list = await _dbcontext.SuperMarkets.FindAsync(id);
            _dbcontext.SuperMarkets.Remove(list);
            await _dbcontext.SaveChangesAsync();
            TempData["done"] = "Super Market is deleted successfully";
            return RedirectToAction("SuperMarketList", "Home");


        }
        public IActionResult AddProduct()
        {
            ViewBag.cat=_dbcontext.Categories.ToList();
            ViewBag.supermart=_dbcontext.SuperMarkets.ToList();
            return View();
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit =15728640)]
        [RequestSizeLimit(15728640)]
        public async Task<IActionResult> AddProduct(Product pd)
        {
            string filename = "";
            if (pd.p_image != null)
            {
                string upload_folder = Path.Combine(_webHostEnvironment.WebRootPath, "media");
                filename = Guid.NewGuid().ToString() + " " + pd.p_image.FileName;
                string filepath=Path.Combine(upload_folder, filename);
                string extension=Path.GetExtension(pd.p_image.FileName);
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jfif" || extension.ToLower() == ".webp")
                {
                   await pd.p_image.CopyToAsync(new FileStream(filepath, FileMode.Create));
                    if(pd.p_image.Length <= 15728640)
                    {
                        var product = new Product
                        {
                            p_name =pd.p_name,
                            p_description =pd.p_description,
                            p_mrp =pd.p_mrp,
                            p_price =pd.p_price,
                            p_category =pd.p_category,
                            p_supermart =pd.p_supermart,
                            p_stock=pd.p_stock,
                            p_img =filename,
                        };
                       await _dbcontext.Products.AddAsync(product);
                        await _dbcontext.SaveChangesAsync();
                        TempData["success"] = "Product is added successfully";
                        return RedirectToAction("ProdList", "Home");
                    }
                    else
                    {
                        TempData["done"] = "Please check file size";
                    }
                }
                else
                {
                    TempData["done"] = "Please check file extension";
                }
            }
               
            
            return View();
        }
        public async Task<IActionResult> ProdList()
        {
            var list= await _dbcontext.Products.ToListAsync();
            return View(list);
        }
        public async Task<IActionResult> PDetails(int id)
        {
            var list = await _dbcontext.Products.Include(x=>x.p_cat).Include(x=>x.p_spmart).FirstOrDefaultAsync(x=>x.p_id==id);
            
            return View(list);
        }
        public async Task<IActionResult> PEdit(int id)
        {
           
            var list = await _dbcontext.Products.FirstOrDefaultAsync(x => x.p_id == id);
            ViewBag.cat = _dbcontext.Categories.ToList();
            ViewBag.supermart = _dbcontext.SuperMarkets.ToList();
            return View(list);
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 15728640)]
        [RequestSizeLimit(15728640)]
        public async Task<IActionResult> PEdit(Product pd, int id)
        {
            var list = await _dbcontext.Products.FirstOrDefaultAsync(x => x.p_id == id);
            ViewBag.cat = _dbcontext.Categories.ToList();
            ViewBag.supermart = _dbcontext.SuperMarkets.ToList();
            string filename = "";
            if (pd.p_image != null)
            {
                string upload_folder = Path.Combine(_webHostEnvironment.WebRootPath, "media");
                filename = Guid.NewGuid().ToString() + " " + pd.p_image.FileName;
                string filepath = Path.Combine(upload_folder, filename);
                string extension = Path.GetExtension(pd.p_image.FileName);
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jfif" || extension.ToLower() == ".webp")
                {
                    await pd.p_image.CopyToAsync(new FileStream(filepath, FileMode.Create));
                    if (pd.p_image.Length <= 15728640)
                    {


                        list.p_name = pd.p_name;
                        list.p_description = pd.p_description;
                        list.p_mrp = pd.p_mrp;
                        list.p_price = pd.p_price;
                        list.p_category = pd.p_category;
                        list.p_supermart = pd.p_supermart;
                        list.p_stock = pd.p_stock;
                        list.p_img = filename;
                        
                         _dbcontext.Products.Update(list);
                        await _dbcontext.SaveChangesAsync();
                        TempData["success"] = "Product is updated successfully";
                        return RedirectToAction("ProdList", "Home");
                    }
                    else
                    {
                        TempData["done"] = "Please check file size";
                    }
                }
                else
                {
                    TempData["done"] = "Please check file extension";
                }
            }

            return RedirectToAction("ProdList", "Home");
            
        }
        public async Task<IActionResult> PDelete(int id)
        {
            var list = await _dbcontext.Products.Include(x => x.p_cat).Include(x => x.p_spmart).FirstOrDefaultAsync(x => x.p_id == id);

            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> PDelete(Product pd,int id)
        {
            var list = await _dbcontext.Products.Include(x => x.p_cat).Include(x => x.p_spmart).FirstOrDefaultAsync(x => x.p_id == id);
            var select_folder = Path.Combine(_webHostEnvironment.WebRootPath, "media");
            var root_path = Path.Combine(Directory.GetCurrentDirectory(), select_folder, list.p_img);
            if (System.IO.File.Exists(root_path))
            {
                System.IO.File.Delete(root_path);
            }
            _dbcontext.Products.Remove(list);
            _dbcontext.SaveChanges();
            TempData["done"] = "Product is deleted successfully";

            return RedirectToAction("ProdList", "Home");
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
            profile.last_name= user.last_name;
            profile.UserName = user.first_name;
            
          
            _dbcontext.ApplicationUsers.Update(profile);
            await _dbcontext.SaveChangesAsync();


            return RedirectToAction("Profile", "Home");
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
          if(user.Email == null)
            {
                TempData["msg"] = "fill both fields ..!";
                return RedirectToAction("ChangeEmail", "Home");
            }
            else if (profile.Email != user.Email)
            {
                TempData["msg"] = "Email is not correct ..!";
                return RedirectToAction("ChangeEmail", "Home");
            }
            var check = await _dbcontext.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == user.new_email);
            if (check != null)
            {
                TempData["msg"] = "This Email is already exist.";
                return RedirectToAction("ChangeEmail", "Home");
            }
            else { 
            profile.Email = user.new_email;
            profile.NormalizedEmail = user.new_email.ToUpper();
            profile.NormalizedUserName = user.new_email.ToUpper();

            _dbcontext.ApplicationUsers.Update(profile);
                await _dbcontext.SaveChangesAsync();
                TempData["msg"] = "Your Email is changed now.";


            return LocalRedirect("/Identity/Account/Login");
            }


        }
        public IActionResult Privacy()
        {
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}