using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.ViewModels;

namespace Project.Controllers
{
    public class DonHangController : Controller
    {
        private readonly ProjectContext db;
        public DonHangController(ProjectContext context)
        {
            db = context;
        }
        [Authorize]
        public IActionResult TheoDoiDonHang()
        {
            var userId = User.Identity?.Name;

            if (userId == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var orders = from hd in db.HdBanHangs
                         join ct in db.CtHdBanHangs on hd.MaHd equals ct.MaHd
                         where hd.MaTaiKhoan == userId
                         group new { hd, ct } by hd.MaHd into orderGroup
                         select new DonHangVM
                         {
                             MaHd = orderGroup.Key,
                             MaTaiKhoan = orderGroup.First().hd.MaTaiKhoan,
                             NgayBan = orderGroup.First().hd.NgayBan,
                             TrangThai = orderGroup.First().hd.MaTrangThaiNavigation.TenTrangThai,
                             TongTien= orderGroup.Sum(o => o.ct.Gia * o.ct.SoLuong) // Calculate total for each order
                         };

            var orderList = orders.ToList();

            return View(orderList);
        }
        [Authorize]
        public IActionResult ChiTietDonHang(string maHd)
        {
            try
            {
                var userId = User.Identity?.Name;
                if (userId == null)
                {
                    return RedirectToAction("DangNhap", "TaiKhoan");
                }

                var taiKhoan = db.TaiKhoans.FirstOrDefault(t => t.MaTaiKhoan == userId);
                if (taiKhoan == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy tài khoản.";
                    return RedirectToAction("TheoDoiDonHang");
                }

                ViewBag.HoTen = taiKhoan.Ten;
                ViewBag.DiaChi = taiKhoan.DiaChi;
                ViewBag.Sdt = taiKhoan.Sdt;

                var orderDetails = from ct in db.CtHdBanHangs
                                   join hd in db.HdBanHangs on ct.MaHd equals hd.MaHd
                                   where hd.MaTaiKhoan == userId && hd.MaHd == maHd
                                   select new DonHangVM
                                   {
                                       MaHd = ct.MaHd,
                                       MaTaiKhoan = hd.MaTaiKhoan,
                                       NgayBan = hd.NgayBan,
                                       TenSp = ct.MaSpNavigation.TenSp,
                                       Mau = ct.MaSpNavigation.MaMauNavigation.TenMau,
                                       DungLuong = ct.MaSpNavigation.MaBoNhoTrongNavigation.DungLuong,
                                       DungLuongRam = ct.MaSpNavigation.MaRamNavigation.DungLuong,
                                       SoLuong = ct.SoLuong,
                                       Gia = ct.Gia,
                                       HinhAnh = ct.MaSpNavigation.HinhAnh ?? "",
                                       TrangThai = hd.MaTrangThaiNavigation.TenTrangThai
                                   };


                var orderDetailList = orderDetails.ToList();
                Console.WriteLine($"Order details count: {orderDetailList.Count}");

                if (!orderDetailList.Any())
                {
                    TempData["ErrorMessage"] = "Không tìm thấy đơn hàng.";
                    return RedirectToAction("DangKy", "TaiKhoan");
                }

                return View(orderDetailList);

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi, vui lòng thử lại.";
                return RedirectToAction("Index", "Home");
            }
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
