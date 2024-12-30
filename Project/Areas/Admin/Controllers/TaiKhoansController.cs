using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Data;

namespace Project.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/TaiKhoans")]
    [Authorize(Roles = "1")]
    public class TaiKhoansController : Controller
    {
        public readonly ProjectContext db;
        public TaiKhoansController(ProjectContext context)
        {
            db = context;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            ViewBag.Quyen = new SelectList(db.PhanQuyens, "MaQuyen", "TenQuyen");
            var tks = db.TaiKhoans.ToList();
            return View(tks);
        }

        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Quyen = new SelectList(db.PhanQuyens, "MaQuyen", "TenQuyen");
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public IActionResult Create(TaiKhoan tk)
        {
            if (ModelState.IsValid)
            {
                db.TaiKhoans.Add(tk);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tk);
        }

        [Route("Edit")]
        [HttpPost]
        public IActionResult Edit(string maTk, int maQuyen)
        {
            var tk = db.TaiKhoans.Find(maTk);

            if (tk == null)
            {
                return NotFound();
            }

            tk.MaQuyen = maQuyen;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Route("Delete")]
        [HttpGet]
        public IActionResult Delete(string maTk)
        {
            var tk = db.TaiKhoans.Find(maTk);

            if (tk == null)
            {
                return NotFound();
            }

            TempData["Message"] = "";
            var hd = db.HdBanHangs.Where(x => x.MaTaiKhoan == maTk).ToList();
            if (hd.Count > 0)
            {
                TempData["Message"] = "Không thể xóa tài khoản này do đã có trong hóa đơn";
                return RedirectToAction("Index");
            }

            db.TaiKhoans.Remove(tk);
            db.SaveChanges();
            TempData["Message"] = "Tài khoản đã được xóa";
            return RedirectToAction("Index");
        }
    }
}
