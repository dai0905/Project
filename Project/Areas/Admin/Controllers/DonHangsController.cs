using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Data;
using Project.ViewModels;

namespace Project.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/DonHangs")]
    [Authorize(Roles = "1")]
    public class DonHangsController : Controller
    {
        private readonly ProjectContext db;
        public DonHangsController(ProjectContext context)
        {
            db = context;
        }

        [Route("Index")]
        public IActionResult Index(string? maTT)
        {
            ViewBag.TrangThai = new SelectList(db.TrangThais, "MaTrangThai", "TenTrangThai");
            var orders = from hd in db.HdBanHangs
                         join ct in db.CtHdBanHangs on hd.MaHd equals ct.MaHd
                         group new { hd, ct } by hd.MaHd into orderGroup
                         select new DonHangVM
                         {
                             MaHd = orderGroup.Key,
                             MaTaiKhoan = orderGroup.First().hd.MaTaiKhoan,
                             NgayBan = orderGroup.First().hd.NgayBan,
                             TrangThai = orderGroup.First().hd.MaTrangThaiNavigation.TenTrangThai,
                             MaTrangThai = orderGroup.First().hd.MaTrangThai,
                             TongTien = orderGroup.Sum(o => o.ct.Gia * o.ct.SoLuong) // Calculate total for each order
                         };

            if (maTT != null && maTT != "o")
            {
                orders = orders.Where(p=>p.MaTrangThai == maTT);
            }

            ViewBag.SelectedStatusId = maTT;

            var orderList = orders.ToList();

            return View(orderList);
        }

        [Route("Details")]
        public IActionResult Details(string maHd)
        {
            var orderDetails = from ct in db.CtHdBanHangs
                               join hd in db.HdBanHangs on ct.MaHd equals hd.MaHd
                               where hd.MaHd == maHd
                               select new DonHangVM
                               {
                                   MaHd = ct.MaHd,
                                   //MaTaiKhoan = hd.MaTaiKhoan,
                                   NgayBan = hd.NgayBan,
                                   TenSp = ct.MaSpNavigation.TenSp,
                                   Mau = ct.MaSpNavigation.MaMauNavigation.TenMau,
                                   DungLuong = ct.MaSpNavigation.MaBoNhoTrongNavigation.DungLuong,
                                   DungLuongRam = ct.MaSpNavigation.MaRamNavigation.DungLuong,
                                   SoLuong = ct.SoLuong,
                                   Gia = ct.Gia,
                                   HinhAnh = ct.MaSpNavigation.HinhAnh ?? "",
                                   TrangThai = hd.MaTrangThaiNavigation.TenTrangThai,
                                   HoTen = hd.MaTaiKhoanNavigation.Ten,
                                   DiaChi = hd.MaTaiKhoanNavigation.DiaChi,
                                   SDT = hd.MaTaiKhoanNavigation.Sdt
                               };
            
            return View(orderDetails);
        }

        [HttpPost]
        [Route("Edit")]
        public IActionResult Edit(string maHd, string maTT)
        {
            var dh = db.HdBanHangs.Find(maHd);
            if (dh == null)
            {
                return NotFound();
            }

            dh.MaTrangThai = maTT;
            // db.HdBanHangs.Update(dh);
            db.SaveChanges();
            //ViewBag.SelectedStatusId = maTT;
            return RedirectToAction("Index");
        }
    }
}
