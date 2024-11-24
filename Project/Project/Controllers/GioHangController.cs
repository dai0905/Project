using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.ViewModels;
using Project.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Project.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ProjectContext db;
        public GioHangController(ProjectContext context)
        {
            db = context;
        }

        const string GIOHANG_KEY = "GIOHANGCUATOI";
        public List<GioHangItem> GioHang => HttpContext.Session.Get<List<GioHangItem>>(MySetting.GIOHANG_KEY) ?? new List<GioHangItem>();

        public IActionResult Index()
        {
            return View(GioHang);
        }

        [HttpGet("/GioHang/ThemVaoGioHang/{MaSp}")]
        public IActionResult ThemVaoGioHang(string MaSp, int quantity = 1)
        {
            var gioHang = GioHang;
            var item = gioHang.SingleOrDefault(p => p.MaSp == MaSp);

            if (item == null)
            {
                var dienThoai = db.DienThoais
                    .Include(p => p.MaRamNavigation)
                    .Include(p => p.MaBoNhoTrongNavigation)
                    .Include(p => p.MaMauNavigation)
                    .SingleOrDefault(p => p.MaSp == MaSp);

                if (dienThoai == null)
                {
                    TempData["Message"] = $"Không tìm thấy điện thoại nào có mã {MaSp}";
                    return NotFound();
                }

                item = new GioHangItem
                {
                    MaSp = dienThoai.MaSp,
                    TenSp = dienThoai.TenSp,
                    Gia = dienThoai.GiaMoi ?? 0,
                    DungLuongRam = dienThoai.MaRamNavigation?.DungLuong ?? "Không có dữ liệu",
                    DungLuong = dienThoai.MaBoNhoTrongNavigation?.DungLuong ?? "Không có dữ liệu",
                    Mau = dienThoai.MaMauNavigation?.TenMau ?? "Không có dữ liệu",
                    HinhAnh = dienThoai.HinhAnh ?? "",
                    SoLuong = quantity,
                    Sl = dienThoai.Sl ?? 0 // Lấy số lượng tồn kho
                };
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }

            HttpContext.Session.Set(MySetting.GIOHANG_KEY, gioHang);

            // Quay lại trang trước đó
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            // Nếu không tìm thấy trang trước đó, quay về trang giỏ hàng
            return RedirectToAction("Index", "GioHang");
        }


        public IActionResult XoaSP(string MaSp)
        {
            var gioHang = GioHang;
            var item = gioHang.SingleOrDefault(p => p.MaSp == MaSp);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.GIOHANG_KEY, gioHang);
            }
            return RedirectToAction("Index", "GioHang");
        }



        [HttpPost]
        public IActionResult CapNhatGioHang(List<GioHangItem> items)
        {
            var gioHang = GioHang;

            foreach (var item in items)
            {
                var gioHangItem = gioHang.FirstOrDefault(x => x.MaSp == item.MaSp);
                if (gioHangItem != null)
                {
                    var dienThoai = db.DienThoais.FirstOrDefault(dt => dt.MaSp == item.MaSp);
                    if (dienThoai != null)
                    {
                        gioHangItem.SoLuong = Math.Min(item.SoLuong, (int)dienThoai.Sl);
                    }
                }
            }

            HttpContext.Session.Set(MySetting.GIOHANG_KEY, gioHang); // Cập nhật lại Session
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ThanhToan()
        {
            if (GioHang == null || GioHang.Count == 0)
            {
                return Redirect("/"); // Chuyển hướng về trang chủ nếu giỏ hàng trống
            }

            var userId = User.Identity?.Name;
            if (userId == null)
            {
                return RedirectToAction("DangNhap", "TaiKhoan"); // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
            }

            var taiKhoan = db.TaiKhoans.FirstOrDefault(t => t.MaTaiKhoan == userId);
            if (taiKhoan == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy tài khoản.";
                return RedirectToAction("TheoDoiDonHang"); // Chuyển hướng đến trang theo dõi đơn hàng nếu không tìm thấy tài khoản
            }

            ViewBag.HoTen = taiKhoan.Ten;
            ViewBag.DiaChi = taiKhoan.DiaChi;
            ViewBag.Sdt = taiKhoan.Sdt;

            return View(GioHang); // Trả về view với danh sách sản phẩm trong giỏ hàng
        }

        [Authorize]
        [HttpPost]
        public IActionResult ThanhToan(string HoTen, string DiaChi, string DienThoai, List<GioHangItem> items)
        {
            if (items == null || items.Count == 0)
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn trống!";
                return RedirectToAction("Index"); // Chuyển hướng nếu giỏ hàng trống
            }

            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Bạn cần đăng nhập để thanh toán.";
                return RedirectToAction("DangNhap", "TaiKhoan"); // Chuyển hướng đến trang đăng nhập nếu chưa đăng nhập
            }

            var gioHang = GioHang;
            if (!gioHang.Any())
            {
                TempData["ErrorMessage"] = "Giỏ hàng của bạn trống!";
                return RedirectToAction("Index"); // Chuyển hướng nếu giỏ hàng trống
            }

            using var transaction = db.Database.BeginTransaction();
            try
            {
                // Lấy mã hóa đơn lớn nhất và tạo mã mới
                var lastMaHd = db.HdBanHangs
                    .OrderByDescending(h => h.MaHd)
                    .Select(h => h.MaHd)
                    .FirstOrDefault();

                string newMaHd = string.IsNullOrEmpty(lastMaHd) ? "HD001" : $"HD{(int.Parse(lastMaHd.Substring(2)) + 1):D3}";

                // Tính tổng tiền
                decimal tongTien = gioHang.Sum(item => item.Gia * item.SoLuong);

                // Tạo hóa đơn mới
                var hdBanHang = new HdBanHang
                {
                    MaHd = newMaHd,
                    MaTaiKhoan = userId,
                    Ten = HoTen,
                    Sdt = DienThoai,
                    DiaChi = DiaChi,
                    NgayBan = DateTime.Now,
                    TongTien = tongTien,
                    MaTrangThai = "TT001" // Trạng thái mặc định: Đang chờ xử lý
                };

                db.HdBanHangs.Add(hdBanHang);
                db.SaveChanges();

                // Thêm chi tiết hóa đơn
                foreach (var item in gioHang)
                {
                    var dienThoai = db.DienThoais.FirstOrDefault(dt => dt.MaSp == item.MaSp);
                    if (dienThoai == null || dienThoai.Sl < item.SoLuong)
                    {
                        TempData["ErrorMessage"] = $"Sản phẩm {item.TenSp} không còn đủ hàng.";
                        transaction.Rollback();
                        return RedirectToAction("Index"); // Nếu không đủ hàng, rollback và hiển thị thông báo lỗi
                    }

                    db.CtHdBanHangs.Add(new CtHdBanHang
                    {
                        MaHd = newMaHd,
                        MaSp = item.MaSp,
                        Gia = item.Gia,
                        SoLuong = item.SoLuong
                    });

                    // Cập nhật tồn kho
                    dienThoai.Sl -= item.SoLuong;
                    db.DienThoais.Update(dienThoai);
                }

                db.SaveChanges();
                transaction.Commit();

                // Xóa giỏ hàng sau khi thanh toán thành công
                HttpContext.Session.Remove(MySetting.GIOHANG_KEY);

                return RedirectToAction("TheoDoiDonHang", "DonHang"); // Chuyển hướng đến trang theo dõi đơn hàng sau khi thanh toán thành công
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                return RedirectToAction("Index"); // Nếu có lỗi, rollback và hiển thị thông báo lỗi
            }
        }
    }
}
