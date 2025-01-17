﻿using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.ViewModels;
using Project.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Project.Services;

namespace Project.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ProjectContext db;
        private readonly IVnPayService _vnPayService;
        private readonly IEmailSender _emailSender;

        public GioHangController(ProjectContext context, IVnPayService vnPayService, IEmailSender emailSender)
        {
            db = context;
            _vnPayService = vnPayService;
            _emailSender = emailSender;
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
                    Sl = dienThoai.Sl ?? 0, // Lấy số lượng tồn kho

                    maRom = dienThoai.MaBoNhoTrong,
                    maRam = dienThoai.MaRam,
                    maMau = dienThoai.MaMau
                };
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }

            HttpContext.Session.Set(MySetting.GIOHANG_KEY, gioHang);

            // Quay lại trang trước đó
            //var referer = Request.Headers["Referer"].ToString();
            //if (!string.IsNullOrEmpty(referer))
            //{
            //    return Redirect(referer);
            //}

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
        public async Task<IActionResult> ThanhToan(string HoTen, string DiaChi, string DienThoai, List<GioHangItem> items, string payment = "ĐẶT HÀNG (COD)")
        {
            if(HoTen == "Chưa có" || DiaChi == "Chưa có" || DienThoai == "Chưa có")
            {
                TempData["ErrorMessage"] = "Vui lòng điền đủ thông tin";
                return RedirectToAction("SuaThongTin", "TaiKhoan");
            }

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

            // Thanh toán VNPay
            if (payment == "Thanh toán VNPay")
            {
                var vnPayModel = new VnPaymentRequestModel
                {
                    Amount = (double)GioHang.Sum(p => p.SoLuong * p.Gia),
                    CreatedDate = DateTime.Now,
                    Description = $"{HoTen} {DienThoai}",
                    FullName = HoTen,
                    OrderId = new Random().Next(1000, 10000)
                };
                return Redirect(_vnPayService.CreatePaymentUrl(HttpContext, vnPayModel));
            }


            using var transaction = db.Database.BeginTransaction();
            try
            {

                string newMaHd = DateTime.Now.ToString("HHmmssddMMyyyy");

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
                    MaTrangThai = "TT001", // Trạng thái mặc định: Đang chờ xử lý
                    PTThanhToan = "Thanh toán khi nhận hàng (COD)"
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

                var tk = db.TaiKhoans.FirstOrDefault(t => t.MaTaiKhoan == userId);
                if (tk?.Email != null)
                {
                    var receiver = tk.Email;
                    var subject = "Đặt hàng thành công";
                    var message = "Đặt hàng thành công, vui lòng chờ duyệt đơn hàng nhé!";

                    await _emailSender.SendEmailAsync(receiver, subject, message);
                }

                return RedirectToAction("TheoDoiDonHang", "DonHang"); // Chuyển hướng đến trang theo dõi đơn hàng sau khi thanh toán thành công
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi: {ex.Message}";
                return RedirectToAction("Index"); // Nếu có lỗi, rollback và hiển thị thông báo lỗi
            }
        }

        [Authorize]
        public IActionResult PaymentFail()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> PaymentCallBack()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            

            if(response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Lỗi thanh toán VNPay: {response.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }


            //lưu đơn hàng vô database
            var gioHang = GioHang;
            var userId = User.Identity?.Name;
            var tk = db.TaiKhoans.Find(userId);
            using var transaction = db.Database.BeginTransaction();
            try
            {

                string newMaHd = response.OrderId;

                // Tính tổng tiền
                decimal tongTien = gioHang.Sum(item => item.Gia * item.SoLuong);

                // Tạo hóa đơn mới
                var hdBanHang = new HdBanHang
                {
                    MaHd = newMaHd,
                    MaTaiKhoan = userId,
                    Ten = tk.Ten,
                    Sdt = tk.Sdt,
                    DiaChi = tk.DiaChi,
                    NgayBan = DateTime.Now,
                    TongTien = tongTien,
                    MaTrangThai = "TT001", // Trạng thái mặc định: Đang chờ xử lý
                    PTThanhToan = "VNPay"
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

                if (tk.Email != null)
                {
                    var receiver = tk.Email;
                    var subject = "Thanh toán thành công";
                    var message = "Thanh toán thành công, vui lòng chờ duyệt đơn hàng nhé!";

                    await _emailSender.SendEmailAsync(receiver, subject, message);
                }

                TempData["Message"] = $"Thanh toán VNPay thành công";
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
