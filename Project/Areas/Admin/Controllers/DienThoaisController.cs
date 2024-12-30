using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Helpers;
using Project.ViewModels;

namespace Project.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/DienThoais")]
    [Authorize(Roles = "1")]
    public class DienThoaisController : Controller
    {
        private ProjectContext db;
        private IMapper _mapper;
        public DienThoaisController (ProjectContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        [Route("Index")]
        public IActionResult Index(int page = 1, int pageSize = 12)
        {
            var dienThoais = db.DienThoais.AsQueryable();
            // Tính toán số trang
            var query = dienThoais.AsNoTracking();

            int totalItems = query.Count(); // Đếm tổng số bản ghi
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.CurrentPage = page;

            // Phân trang và lấy dữ liệu
            var paginatedResult = dienThoais
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new DienThoaiVM
                {
                    MaSp = p.MaSp,
                    TenSp = p.TenSp,
                    GiaCu = p.GiaCu,
                    GiaMoi = p.GiaMoi,
                    HinhAnh = p.HinhAnh ?? "",
                    Sl = p.Sl ?? 0
                })
                .ToList();
            return View(paginatedResult);
        }

        [Route("Details")]
        public IActionResult Details(string id)
        {
            var dienThoais = db.DienThoais.Include(p => p.MaMauNavigation)
                .Include(p => p.MaThuongHieuNavigation)
                .Include(p => p.MaRamNavigation)
                .Include(p => p.MaBoNhoTrongNavigation).FirstOrDefault(p => p.MaSp == id);

            if (dienThoais == null)
            {
                return NotFound();
            }

            return View(dienThoais);
        }

        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieus, "MaThuongHieu", "TenThuongHieu");
            ViewBag.MaMau = new SelectList(db.Maus, "MaMau", "TenMau");
            ViewBag.MaRam = new SelectList(db.Rams, "MaRam", "DungLuong");
            ViewBag.MaBoNhoTrong = new SelectList(db.BoNhoTrongs, "MaBoNhoTrong", "DungLuong");
            return View();
        }

        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DienThoai model, IFormFile hinh1, IFormFile hinh2)
        {
            if (ModelState.IsValid)
            {
                if (hinh1 != null && hinh2 != null)
                {
                    model.HinhAnh = MyUtil.UpLoadHinh(hinh1);
                    model.AnhThongSo = MyUtil.UpLoadHinh(hinh2);
                }

                db.DienThoais.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Route("Edit")]
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var sp = db.DienThoais.Find(id);
            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieus, "MaThuongHieu", "TenThuongHieu");
            ViewBag.MaMau = new SelectList(db.Maus, "MaMau", "TenMau");
            ViewBag.MaRam = new SelectList(db.Rams, "MaRam", "DungLuong");
            ViewBag.MaBoNhoTrong = new SelectList(db.BoNhoTrongs, "MaBoNhoTrong", "DungLuong");
            return View(sp);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, DienThoai model, IFormFile? hinh1, IFormFile? hinh2)
        {
            var sp = db.DienThoais.Find(id);
            if (sp == null) { return NotFound(); }

            if (ModelState.IsValid)
            {
                _mapper.Map(model, sp);

                if (hinh1 != null)
                {
                    sp.HinhAnh = MyUtil.UpLoadHinh(hinh1);
                }
                if (hinh2 != null)
                {
                    sp.AnhThongSo = MyUtil.UpLoadHinh(hinh2);
                }

                db.DienThoais.Update(sp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Route("Delete")]
        [HttpGet]
        public IActionResult Delete(string id)
        {
            var dienThoais = db.DienThoais.Include(p => p.MaMauNavigation)
                .Include(p => p.MaThuongHieuNavigation)
                .Include(p => p.MaRamNavigation)
                .Include(p => p.MaBoNhoTrongNavigation).FirstOrDefault(p => p.MaSp == id);

            if (dienThoais == null)
            {
                return NotFound();
            }

            ViewBag.MaThuongHieu = new SelectList(db.ThuongHieus, "MaThuongHieu", "TenThuongHieu");
            ViewBag.MaMau = new SelectList(db.Maus, "MaMau", "TenMau");
            ViewBag.MaRam = new SelectList(db.Rams, "MaRam", "DungLuong");
            ViewBag.MaBoNhoTrong = new SelectList(db.BoNhoTrongs, "MaBoNhoTrong", "DungLuong");
            return View(dienThoais);
        }

        [Route("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            TempData["Message"] = "";
            var sp = db.CtHdBanHangs.Where(x => x.MaSp == id).ToList();
            if (sp.Count > 0)
            {
                TempData["Message"] = "Không xóa được sản phẩm này vì đã có trong hóa đơn bán hàng";
                return RedirectToAction("Index");
            }

            
            db.DienThoais.Remove(db.DienThoais.Find(id));
            db.SaveChanges();
            TempData["Message"] = "Sản phẩm đã được xóa";
            return RedirectToAction("Index");
        }
    }
}
