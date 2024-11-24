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
    }
}
