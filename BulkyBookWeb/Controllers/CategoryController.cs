using BulkyBook.Models;

using BulkyBookWeb.DataAccess;

using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{

    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;  
        }


        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.categories.ToList();
            return View(objCategoryList);
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if(obj.Name== obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order Dose not Match the Name");
            }
            if (ModelState.IsValid)
            {
                _db.categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "category crated sccessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }



        //GET
        public IActionResult Edit(int? id)
        {
            if(id ==null || id == 0)
            {
                return NotFound();
            }
            var categoryFrontDb = _db.categories.Find(id);
            ///    var categoryFrontDbFirst = _db.categories.FirstOrDefault(u=>u.Id == id);
          //  var categoryFrontDbSingle = _db.categories.SingleOrDefault(u => u.Id == id);
          if(categoryFrontDb == null)
            {
                return NotFound();
            }

            return View(categoryFrontDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order Dose not Match the Name");
            }
            if (ModelState.IsValid)
            {
                _db.categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "category updated sccessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }




        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFrontDb = _db.categories.Find(id);
            ///    var categoryFrontDbFirst = _db.categories.FirstOrDefault(u=>u.Id == id);
            //  var categoryFrontDbSingle = _db.categories.SingleOrDefault(u => u.Id == id);
            if (categoryFrontDb == null)
            {
                return NotFound();
            }

            return View(categoryFrontDb);
        }

        //POST1a
        [HttpPost,ActionName("DeletePOST")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
        var obj = _db.categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

                _db.categories.Remove(obj);
                _db.SaveChanges();
            TempData["success"] = "category deleted sccessfully";
            return RedirectToAction("Index");
            
            return View(obj);
        }
    }
}
