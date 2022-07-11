using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]
public class CoverTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CoverTypeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
        return View(objCoverTypeList);
    }

    //GET
    public IActionResult Create()
    {
        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken] //XSRf/CSRF 공격 방지용
    public IActionResult Create(CoverType obj)
    {
        //정확한 데이터가 들어왔는 지 체크 
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Add(obj);
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

        var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOfrDefault(u => u.Id == id);
        //var categoryFromDbSingle = _db.CoverType.FirstOrDefault(u => u.Id == id);

        if (coverTypeFromDbFirst == null)
            return NotFound();

        return View(coverTypeFromDbFirst);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken] //XSRf/CSRF 공격 방지용
    public IActionResult Edit(CoverType obj)
    {
        //정확한 데이터가 들어왔는 지 체크 
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Update(obj);
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
        var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOfrDefault(u => u.Id == id);
        //var categoryFromDbSingle = _db.Categories.FirstOrDefault(u => u.Id == id);

        if (coverTypeFromDbFirst == null)
            return NotFound();

        return View(coverTypeFromDbFirst);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken] //XSRf/CSRF 공격 방지용
    public IActionResult DeletePOST(int? id)
    {
        //var obj = _db.Categories.Find(id);
        var obj = _unitOfWork.CoverType.GetFirstOfrDefault(u => u.Id == id);
        if (obj == null)
            return NotFound();

        //정확한 데이터가 들어왔는 지 체크 
        _unitOfWork.CoverType.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "삭제가 완료되었습니다.";
        return RedirectToAction("Index");
    }
}
