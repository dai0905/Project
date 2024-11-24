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
    }
}
