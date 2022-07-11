using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Data;
using BulkyBook.Models;
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
            IEnumerable<Category> objCategoryList = _db.Categories;
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
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "등록이 완료되었습니다.";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //EDIT
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
                return NotFound();

            //var categoryFromDb = _db.Categories.Find(id);
            var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Name == "id");
            //var categoryFromDbSingle = _db.Categories.FirstOrDefault(u => u.Id == id);

            if(categoryFromDbFirst == null)
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
                _db.Categories.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        //Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var categoryFromDb = _db.Categories.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.FirstOrDefault(u => u.Id == id);

            if (categoryFromDb == null)
                return NotFound();

            return View(categoryFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] //XSRf/CSRF 공격 방지용
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Categories.Find(id);
            if (obj == null)
                return NotFound();

            //정확한 데이터가 들어왔는 지 체크 
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "삭제가 완료되었습니다.";
            return RedirectToAction("Index");
        }
    }
}
