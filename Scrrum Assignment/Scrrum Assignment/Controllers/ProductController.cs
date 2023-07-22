using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Scrrum_Assignment.Data;
using Scrrum_Assignment.Models;
using System.Web;

namespace Scrrum_Assignment.Controllers
{
    public class ProductController : Controller
    {
        //Using Class ApprlicationDbContext for doing databse operations
        private ApplicationDbContext _db;
        // Using Interface Iwebhostenvironment to findout the wwwroot
        private readonly IWebHostEnvironment _webHostEnvironment;
        //Creating Connstructor for dependency injection
        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        
        //Action result for Index view
        public IActionResult Index()
        {
            //Fetching the list of all product from DB
            List<Product> objProduct = _db.Products.ToList();
            
            return View(objProduct);
        }
        //Action Result for Edit/Create, (if id is null then it will open create view else update view)
        public IActionResult Upsert(int? id)
        {

            Product? product = new()
            {

            };
            if (id == null || id == 0)
            {
                return View(product);
            }
            else
            {
                 product = _db.Products.Find(id);
                
                return View(product);

            }
        }
        //Action Result for delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? Productfromdb = _db.Products.Find(id);
            if (Productfromdb == null)
            {
                return NotFound();
            }
            return View(Productfromdb);
        }
        [HttpPost]
        //Post request for updating and creating entries in database
        //The request will have an object and a static file which needs to inserted in folder and DB
        public IActionResult Upsert(Product obj, IFormFile? file)
        {
            
            {
                //Finding path directory
                string wwwRootpath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    //Renaming the Image file
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootpath, @"images\product");
                    if (!string.IsNullOrEmpty(obj.ImageURL))
                    {
                        //delete old image if exist in DB (for updating)
                        var oldImagePath = Path.Combine(wwwRootpath, obj.ImageURL.Trim('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    //create new file
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    //Assigning the path value to ImageURL attribute
                    obj.ImageURL = @"\images\product\" + fileName;
                }
                if (obj.Id == 0)
                {
                    //Performing Add opertaion
                    _db.Add(obj);
                }
                else
                {
                    //Performing update operation
                    _db.Update(obj);
                }
                TempData["Success"] = "Product created successfully";
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();


        }
        //Post request for deleting an entry from db
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            //Finding the entry on the basis of id and the deleting it
            Product obj = _db.Products.Find(id);
            if (obj == null)
            {
                return View();
            }
            TempData["Success"] = "Category deleted successfully";
            _db.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");



        }

    }
}
