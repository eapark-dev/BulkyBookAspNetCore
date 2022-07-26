using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(IUnitOfWork unitOfWork , IWebHostEnvironment hostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll();
        return View(objProductList);
    }

    //EDIT
    public IActionResult Upsert(int? id)
    {
        ProductVM productVM = new ProductVM()
        {
            Product = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i=> new SelectListItem { 
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
        };
        
        if (id == null || id == 0)
        {
            //일부 데이터를 불러올 때 사용 ViewBag
            //자동으로 변수형을 유추
            return View(productVM);
        }
        else
        { 
        }
        

        return View(productVM);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken] //XSRf/CSRF 공격 방지용
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
        //정확한 데이터가 들어왔는 지 체크 
        if (ModelState.IsValid)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if (file != null) {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\products");
                var extension = Path.GetExtension(file.FileName);

                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create)) 
                {
                    file.CopyTo(fileStreams);
                }
                obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
            }

            _unitOfWork.Product.Update(obj.Product);
            _unitOfWork.Save();
            TempData["success"] = "Product created successfully";
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
