using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
        return View(objCategoryList);
    }

    //GET
    public IActionResult Create()
    {
        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken] //XSRf/CSRF 공격 방지용
    public IActionResult Create(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "이름과 주문번호는 같을 수 없습니다.");
        }

        //정확한 데이터가 들어왔는 지 체크 
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "등록이 완료되었습니다.";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    //EDIT
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        //var categoryFromDb = _db.Categories.Find(id);
        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOfrDefault(u => u.Id == id);
        //var categoryFromDbSingle = _db.Categories.FirstOrDefault(u => u.Id == id);

        if (categoryFromDbFirst == null)
            return NotFound();

        return View(categoryFromDbFirst);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken] //XSRf/CSRF 공격 방지용
    public IActionResult Edit(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "이름과 주문번호는 같을 수 없습니다.");
        }

        //정확한 데이터가 들어왔는 지 체크 
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(obj);
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
        var categoryFromDbFirst = _unitOfWork.Category.GetFirstOfrDefault(u => u.Id == id);
        //var categoryFromDbSingle = _db.Categories.FirstOrDefault(u => u.Id == id);

        if (categoryFromDbFirst == null)
            return NotFound();

        return View(categoryFromDbFirst);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken] //XSRf/CSRF 공격 방지용
    public IActionResult DeletePOST(int? id)
    {
        //var obj = _db.Categories.Find(id);
        var obj = _unitOfWork.Category.GetFirstOfrDefault(u => u.Id == id);
        if (obj == null)
            return NotFound();

        //정확한 데이터가 들어왔는 지 체크 
        _unitOfWork.Category.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "삭제가 완료되었습니다.";
        return RedirectToAction("Index");
    }
}
