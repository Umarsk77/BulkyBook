using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBookWeb.DataAccess;

using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;

[Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public CategoryController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
    

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitofWork.Category.GetAll();
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
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order Dose not Match the Name");
            }
            if (ModelState.IsValid)
            {
                _unitofWork.Category.Add(obj);
                _unitofWork.Save();
                TempData["success"] = "category crated sccessfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }



        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //  var categoryFrontDb = _db.categories.Find(id);
            var categoryFrontDbFirst = _unitofWork.Category.GetFirstOrDefault(u => u.Id == id);
            //  var categoryFrontDbSingle = _db.categories.SingleOrDefault(u => u.Id == id);
            if (categoryFrontDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryFrontDbFirst);
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
                _unitofWork.Category.Update(obj);
                _unitofWork.Save();
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
            //  var categoryFrontDb = _db.categories.Find(id);
            var categoryFrontDbFirst = _unitofWork.Category.GetFirstOrDefault(u => u.Id == id);
            //  var categoryFrontDbSingle = _db.categories.SingleOrDefault(u => u.Id == id);
            if (categoryFrontDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryFrontDbFirst);
        }

        //POST
        [HttpPost, ActionName("DeletePOST")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitofWork.Category.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitofWork.Category.Remove(obj);
            _unitofWork.Save();
            TempData["success"] = "category deleted sccessfully";
            return RedirectToAction("Index");

            return View(obj);
        }
    
}
