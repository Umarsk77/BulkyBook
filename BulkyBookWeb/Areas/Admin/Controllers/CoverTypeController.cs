using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBookWeb.DataAccess;

using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;

[Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public CoverTypeController(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }


        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitofWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _unitofWork.CoverType.Add(obj);
                _unitofWork.Save();
                TempData["success"] = "CoverType crated sccessfully";
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
            var CoverTypeFrontDbFirst = _unitofWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (CoverTypeFrontDbFirst == null)
            {
                return NotFound();
            }

            return View(CoverTypeFrontDbFirst);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _unitofWork.CoverType.Update(obj);
                _unitofWork.Save();
                TempData["success"] = "CoverType updated sccessfully";
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

            return View(obj);
        }
    
}
