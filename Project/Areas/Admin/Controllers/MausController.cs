using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Data;

namespace Project.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/Maus")]
    [Authorize(Roles = "1")]
    public class MausController : Controller
    {
        private ProjectContext db;

        public MausController (ProjectContext context)
        {
            db = context;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            var maus = db.Maus.ToList();
            return View(maus);
        }

        [Route("Details")]
        public IActionResult Details (string id)
        {
            var mau = db.Maus.Find(id); 

            if (mau == null)
            {
                return NotFound();
            }

            return View(mau);
        }

        [Route("Create")]
        [HttpGet]
        public IActionResult Create ()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(Mau mau)
        {
            if (ModelState.IsValid)
            {
                db.Maus.Add(mau);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mau);
        }

        [Route("Edit")]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var mau = db.Maus.Find(id);
            if (mau == null)
            {
                return NotFound();
            }

            return View(mau);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Mau mau)
        {
            if (ModelState.IsValid)
            {
                db.Maus.Update(mau);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mau);
        }

        [Route("Delete")]
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var mau = db.Maus.Find(id);

            if (mau == null)
            {
                return NotFound();
            }

            TempData["Message"] = "";
            var ck = db.DienThoais.Where(x => x.MaMau == id).ToList();
            if (ck.Count > 0)
            {
                TempData["Message"] = "Sản phẩm đã có màu này nên không xóa được";
                return RedirectToAction("Index");
            }

            db.Maus.Remove(mau);
            db.SaveChanges();
            TempData["Message"] = "Màu đã được xóa";
            return RedirectToAction("Index");
        }
    }
}
