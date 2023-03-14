using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using BulkyBookWeb.DataAccess;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers;

[Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitofWork _unitofWork;
    private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitofWork unitofWork, IWebHostEnvironment hostEnvironment)
        {
            _unitofWork = unitofWork;
        _hostEnvironment = hostEnvironment;
        }


        public IActionResult Index()
        {
          
            return View();
        }
      
        //GET
        public IActionResult Upsert(int? id)
        {
        ProductVM productVM = new()
        {
            Product = new(),

            CategoryList = _unitofWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverTypeList = _unitofWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
        };
        if (id == null || id == 0)
            {
            //Create Product
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CoverTypeList"] = CoverTypeList;
            return View(productVM);

        }
        else
        {
            //update
        }
            return View(productVM);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile file)
        {
            if (ModelState.IsValid)
            {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if(file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\products");
                var extension = Path.GetExtension(file.FileName);
                using(var fileStream = new FileStream(Path.Combine(uploads,fileName+extension),FileMode.Create))
                {
                    file.CopyTo(fileStream);

                }
                    obj.Product.ImageUrl= @"\images\products\"+fileName+extension;
            }
              _unitofWork.Product.Add(obj.Product);
                _unitofWork.Save();
                TempData["success"] = " Product created sccessfully";
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
            //  var CoverTypeFrontDb = _db.categories.Find(id);
            var CoverTypeFrontDbFirst = _unitofWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            //  var CoverTypeFrontDbSingle = _db.categories.SingleOrDefault(u => u.Id == id);
            if (CoverTypeFrontDbFirst == null)
            {
                return NotFound();
            }

            return View(CoverTypeFrontDbFirst);
        }

        //POST
        [HttpPost, ActionName("DeletePOST")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitofWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }

            _unitofWork.CoverType.Remove(obj);
            _unitofWork.Save();
            TempData["success"] = "CoverType deleted sccessfully";
            return RedirectToAction("Index");
        }
    #region API CALLS
    [HttpGet]
    public IActionResult GetAll() {
    var productList = _unitofWork.Product.GetAll(includeProperties:"Category,CoverType");
        return Json(new {data  = productList});
    }
    #endregion


}
