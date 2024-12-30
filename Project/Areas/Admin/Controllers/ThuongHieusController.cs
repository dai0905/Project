using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Data;

namespace Project.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/ThuongHieus")]
    [Authorize(Roles = "1")]
    public class ThuongHieusController : Controller
    {
        private ProjectContext db;

        public ThuongHieusController(ProjectContext context)
        {
            db = context;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            var ths = db.ThuongHieus.ToList();
            return View(ths);
        }

        [Route("Details")]
        public IActionResult Details(string id)
        {
            var th = db.ThuongHieus.Find(id);

            if (th == null)
            {
                return NotFound();
            }

            return View(th);
        }

        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(ThuongHieu th)
        {
            if (ModelState.IsValid)
            {
                db.ThuongHieus.Add(th);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(th);
        }

        [Route("Edit")]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var th = db.ThuongHieus.Find(id);
            if (th == null)
            {
                return NotFound();
            }

            return View(th);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ThuongHieu th)
        {
            if (ModelState.IsValid)
            {
                db.ThuongHieus.Update(th);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(th);
        }

        [Route("Delete")]
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var th = db.ThuongHieus.Find(id);

            if (th == null)
            {
                return NotFound();
            }

            TempData["Message"] = "";
            var ck = db.DienThoais.Where(x => x.MaThuongHieu == id).ToList();
            if (ck.Count > 0)
            {
                TempData["Message"] = "Sản phẩm đã có thương hiệu này nên không xóa được";
                return RedirectToAction("Index");
            }

            db.ThuongHieus.Remove(th);
            db.SaveChanges();
            TempData["Message"] = "Thương hiệu đã được xóa";
            return RedirectToAction("Index");
        }
    }
}
