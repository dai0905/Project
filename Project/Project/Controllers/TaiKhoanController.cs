using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Helpers;
using Project.ViewModels;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;

namespace Project.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly ProjectContext db;
        private readonly IMapper _mapper;

        // Constructor với ProjectContext và IMapper
        public TaiKhoanController(ProjectContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        // Phương thức Đăng Ký (GET)
        public IActionResult DangKy()
        {
            return View();
        }

        // Phương thức Đăng Ký (POST)
        
        [HttpPost]
        public IActionResult DangKy(DangKyVM model)
        {
            if (ModelState.IsValid)
            {
                var taiKhoanTonTai = db.TaiKhoans.Any(tk => tk.MaTaiKhoan == model.MaTaiKhoan);

                if (taiKhoanTonTai)
                {
                    ModelState.AddModelError("MaTaiKhoan", "Tài khoản đã tồn tại. Vui lòng chọn tên tài khoản khác.");
                    return View(model);
                }

                var taiKhoan = _mapper.Map<TaiKhoan>(model);
                taiKhoan.MaQuyen = 2;
                taiKhoan.MatKhau = model.MatKhau;

                try
                {
                    db.TaiKhoans.Add(taiKhoan);
                    db.SaveChanges();

                    TempData["DangKyMessage"] = "Đăng ký thành công!";
                    return RedirectToAction("DangNhap", "TaiKhoan");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi khi lưu tài khoản: " + ex.Message);
                    ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi đăng ký. Vui lòng thử lại.");
                }
            }
            return View(model);
        }


        public IActionResult DangKySuccess()
        {
            return View();
        }

        // Phương thức Đăng Nhập (GET)
        [HttpGet]
        public IActionResult DangNhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View(new DangNhapVM()); // Khởi tạo model đúng
        }

        // Phương thức Đăng Nhập (POST)
        [HttpPost]
        public async Task<IActionResult> DangNhap(DangNhapVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            if (ModelState.IsValid)
            {
                // Truy vấn tài khoản từ cơ sở dữ liệu
                var taiKhoan = await db.TaiKhoans.Include(tk => tk.MaQuyenNavigation)
                                                 .SingleOrDefaultAsync(tk => tk.MaTaiKhoan == model.MaTaiKhoan);

                if (taiKhoan == null || taiKhoan.MatKhau != model.MatKhau)
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu của quý khách bị sai.");
                }
                else
                {
                    // Thiết lập thông tin xác thực người dùng
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, taiKhoan.MaTaiKhoan)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Chuyển hướng đến ReturnUrl hoặc trang chủ
                    return Redirect(ReturnUrl ?? Url.Action("Index", "Home"));
                }
            }
            return View(model);
        }
        [Authorize]
        public IActionResult ThongTinCaNhan()
        {
            var maTK = User.Identity?.Name;

            if (maTK == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để xem thông tin cá nhân.";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            var taiKhoan = db.TaiKhoans.FirstOrDefault(t => t.MaTaiKhoan == maTK);

            if (taiKhoan == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin cá nhân.";
                return RedirectToAction("Index");
            }

            var taiKhoanVM = new TaiKhoanVM
            {
                MaTaiKhoan = taiKhoan.MaTaiKhoan,
                MatKhau = taiKhoan.MatKhau,
                Ten = taiKhoan.Ten,
                Sdt = taiKhoan.Sdt,
                DiaChi = taiKhoan.DiaChi
            };

            return View(taiKhoanVM);
        }


        public IActionResult Success()
        {
            // Có thể truyền model vào view nếu cần
            return View();
        }

        // Trang cá nhân yêu cầu xác thực
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult SuaThongTin()
        {
            var maTK = User.Identity?.Name; // Lấy MaTaiKhoan từ User.Identity
            if (maTK == null)
                return RedirectToAction("DangNhap", "TaiKhoan");

            var taiKhoan = db.TaiKhoans.FirstOrDefault(t => t.MaTaiKhoan == maTK);
            if (taiKhoan == null)
                return RedirectToAction("Index", "Home");

            // Chuyển đổi từ TaiKhoan sang ThongTinCaNhanVM
            var model = new ThongTinCaNhanVM
            {
                MaTaiKhoan = taiKhoan.MaTaiKhoan,
                Ten = taiKhoan.Ten,
                Sdt = taiKhoan.Sdt,
                DiaChi = taiKhoan.DiaChi
            };

            return View(model); // Truyền model sang View
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken] // Bảo vệ chống lại CSRF
        public IActionResult SuaThongTin(ThongTinCaNhanVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var maTK = User.Identity?.Name; // Lấy MaTaiKhoan từ User.Identity
            if (maTK == null)
                return RedirectToAction("DangNhap", "TaiKhoan");

            var taiKhoan = db.TaiKhoans.FirstOrDefault(t => t.MaTaiKhoan == maTK);
            if (taiKhoan != null)
            {
                // Cập nhật thông tin
                taiKhoan.Ten = model.Ten;
                taiKhoan.Sdt = model.Sdt;
                taiKhoan.DiaChi = model.DiaChi;

                db.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công.";

                // Redirect to the "Thông tin cá nhân" page
                return RedirectToAction("ThongTinCaNhan", "TaiKhoan");
            }

            TempData["ErrorMessage"] = "Không tìm thấy tài khoản.";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ThayDoiMatKhau()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult ThayDoiMatKhau(ThayDoiMKVM model)
        {
            // Kiểm tra tính hợp lệ của dữ liệu
            if (!ModelState.IsValid)
            {
                return View(model); // Trả về view với các thông báo lỗi
            }

            // Lấy tên tài khoản đăng nhập hiện tại (sử dụng User.Identity.Name)
            var maTK = User.Identity?.Name;

            // Tìm tài khoản trong cơ sở dữ liệu
            var taiKhoan = db.TaiKhoans.FirstOrDefault(t => t.MaTaiKhoan == maTK);
            if (taiKhoan == null)
            {
                TempData["ErrorMessage"] = "Tài khoản của bạn không tồn tại trong hệ thống.";
                return RedirectToAction("DangNhap", "TaiKhoan"); // Chuyển hướng đến trang đăng nhập
            }

            // Kiểm tra mật khẩu cũ
            if (taiKhoan.MatKhau != model.MatKhauCu)
            {
                ModelState.AddModelError("MatKhauCu", "Mật khẩu cũ không đúng.");
                return View(model); // Trả về view với thông báo lỗi
            }

            // Cập nhật mật khẩu mới
            taiKhoan.MatKhau = model.MatKhauMoi;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công.";
            return RedirectToAction("ThongTinCaNhan", "TaiKhoan"); // Chuyển đến trang thông tin cá nhân
        }

        // Đăng xuất
        [Authorize]
        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

    }
}
