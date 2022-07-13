using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
        return View(objProductList);
    }

    //EDIT
    public IActionResult Upsert(int? id)
    {
        Product product = new();
        IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
            u=> new SelectListItem { 
                Text = u.Name,
                Value = u.Id.ToString()
            });
        IEnumerable<SelectListItem> CoverTypeList = _unitOfWork.CoverType.GetAll().Select(
            u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });

        if (id == null || id == 0)
        {
            //일부 데이터를 불러올 때 사용 ViewBag
            //자동으로 변수형을 유추
            ViewBag.CategoryList = CategoryList;
            ViewBag.CoverTypeList = CoverTypeList;
            return View(product);
        }
        else
        { 
        }
        

        return View(product);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken] //XSRf/CSRF 공격 방지용
    public IActionResult Upsert(Product obj)
    {
        //정확한 데이터가 들어왔는 지 체크 
        if (ModelState.IsValid)
        {
            _unitOfWork.Product.Update(obj);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        return View(obj);
    }


    //Delete
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        //var categoryFromDb = _db.Find(id);
        var productFromDbFirst = _unitOfWork.Product.GetFirstOfrDefault(u => u.Id == id);
        //var categoryFromDbSingle = _db.Categories.FirstOrDefault(u => u.Id == id);

        if (productFromDbFirst == null)
            return NotFound();

        return View(productFromDbFirst);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken] //XSRf/CSRF 공격 방지용
    public IActionResult DeletePOST(int? id)
    {
        //var obj = _db.Categories.Find(id);
        var obj = _unitOfWork.Product.GetFirstOfrDefault(u => u.Id == id);
        if (obj == null)
            return NotFound();

        //정확한 데이터가 들어왔는 지 체크 
        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "삭제가 완료되었습니다.";
        return RedirectToAction("Index");
    }
}
