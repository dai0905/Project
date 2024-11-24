using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using Project.ViewModels;
using System.Diagnostics;
using System.Linq;


namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjectContext db;
        public HomeController(ProjectContext context)
        {
            db = context;

        }
        public IActionResult Index(string[]? thuonghieu, string[]? ram, string[]? boNhoTrong, string[]? gia, string? searchTerm, string? sortOrder, int page = 1, int pageSize = 12)
        {
            var dienThoais = db.DienThoais.AsQueryable();

            // Lưu giá trị đã chọn để sử dụng lại trong view
            ViewBag.SelectedBrands = thuonghieu ?? Array.Empty<string>();
            ViewBag.SelectedRams = ram ?? Array.Empty<string>();
            ViewBag.SelectedBoNhoTrongs = boNhoTrong ?? Array.Empty<string>();
            ViewBag.SelectedGias = gia ?? Array.Empty<string>();
            ViewBag.SortOrder = sortOrder;

            // Kiểm tra và lưu searchTerm nếu có
            if (!string.IsNullOrEmpty(searchTerm))
            {
                dienThoais = dienThoais.Where(p => p.TenSp.Contains(searchTerm));
                ViewBag.SearchTerm = searchTerm; // Lưu lại giá trị tìm kiếm để hiển thị trên form
            }
            else
            {
                ViewBag.SearchTerm = ""; // Nếu không có searchTerm thì trả về trống
            }

            // Lọc theo Thương hiệu
            if (thuonghieu != null && thuonghieu.Any())
            {
                dienThoais = dienThoais.Where(p => thuonghieu.Contains(p.MaThuongHieuNavigation.TenThuongHieu));
            }

            // Lọc theo RAM
            if (ram != null && ram.Any())
            {
                dienThoais = dienThoais.Where(p => ram.Contains(p.MaRamNavigation.DungLuong));
            }

            // Lọc theo Bộ nhớ trong
            if (boNhoTrong != null && boNhoTrong.Any())
            {
                dienThoais = dienThoais.Where(p => boNhoTrong.Contains(p.MaBoNhoTrongNavigation.DungLuong));
            }

            // Lọc theo Giá
            if (gia != null && gia.Any())
            {
                dienThoais = dienThoais.Where(p =>
                    (gia.Contains("duoi-10-trieu") && p.GiaMoi < 10000000) ||
                    (gia.Contains("tu-10-den-20-trieu") && p.GiaMoi >= 10000000 && p.GiaMoi <= 20000000) ||
                    (gia.Contains("tren-20-trieu") && p.GiaMoi > 20000000)
                );
            }

            // Áp dụng sắp xếp
            dienThoais = sortOrder switch
            {
                "price_asc" => dienThoais.OrderBy(p => p.GiaMoi),
                "price_desc" => dienThoais.OrderByDescending(p => p.GiaMoi),
                _ => dienThoais
            };

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
                    TenThuongHieu = p.MaThuongHieuNavigation.TenThuongHieu,
                    Sl = p.Sl ?? 0,
                    DungLuong = p.MaBoNhoTrongNavigation.DungLuong,
                    DungLuongRam = p.MaRamNavigation.DungLuong,
                    Mau = p.MaMauNavigation.TenMau,
                    ManHinh = p.ManHinh
                })
                .ToList();

            return View(paginatedResult);
        }
        public IActionResult CTDienThoai(string id)
        {
            var dienThoai = db.DienThoais
                .Where(p => p.MaSp == id)
                .Select(p => new DienThoaiVM
                {
                    MaSp = p.MaSp,
                    TenSp = p.TenSp,
                    HeDieuHanh = p.HeDieuHanh,
                    CameraSau = p.CameraSau,
                    CameraTruoc = p.CameraTruoc,
                    Cpu = p.Cpu,
                    Pin = p.Pin,
                    ChatLieu = p.ChatLieu,
                    Ktkl = p.Ktkl,
                    Tdrm = p.Tdrm,
                    GiaCu = p.GiaCu,
                    GiaMoi = p.GiaMoi,
                    HinhAnh = p.HinhAnh ?? "",
                    TenThuongHieu = p.MaThuongHieuNavigation.TenThuongHieu,
                    Sl = p.Sl ?? 0,
                    DungLuong = p.MaBoNhoTrongNavigation.DungLuong,
                    DungLuongRam = p.MaRamNavigation.DungLuong,
                    Mau = p.MaMauNavigation.TenMau,
                    ManHinh = p.ManHinh,
                    MoTa = p.MoTa,
                    AnhThongSo = p.AnhThongSo ?? ""
                })
                .FirstOrDefault();

            if (dienThoai == null)
            {
                return NotFound();
            }

            // Trả về đối tượng duy nhất, không phải danh sách
            return View(dienThoai);
        }
    }
}
