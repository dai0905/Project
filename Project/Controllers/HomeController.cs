using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                    maRom = p.MaBoNhoTrong,
                    maRam = p.MaRam,
                    maMau = p.MaMau,
                    ManHinh = p.ManHinh
                })
                .ToList();

            return View(paginatedResult);
        }


        public IActionResult CTDienThoai(string? ten, string? maMau, string? ram, string? rom, IFormCollection? form)
        {
            var maus = (from dt in db.DienThoais
                        join mau in db.Maus on dt.MaMau equals mau.MaMau
                        where dt.TenSp == ten
                        select new
                        {
                            MaMau = mau.MaMau,
                            TenMau = mau.TenMau
                        }).Distinct().ToList();
            
            ViewBag.Mau = new SelectList(maus, "MaMau", "TenMau");

            var dungluongs = (from dt in db.DienThoais
                        join ram1 in db.Rams on dt.MaRam equals ram1.MaRam
                        join rom1 in db.BoNhoTrongs on dt.MaBoNhoTrong equals rom1.MaBoNhoTrong
                        where dt.TenSp == ten && dt.MaMau == maMau
                        select new
                        {
                            MaRam = ram1.MaRam,
                            TenRam = ram1.DungLuong,
                            MaRom = rom1.MaBoNhoTrong,
                            TenRom = rom1.DungLuong,
                        }).Distinct().ToList();
            
            ViewBag.RamRomList = dungluongs.Select(x =>
                new
                {
                    Value = $"{x.MaRam}-{x.MaRom}", // Giá trị kết hợp sẽ được gửi  
                    Text = $"{x.TenRam} - {x.TenRom}" // Tên hiển thị  
                }).ToList();
            

            if (ram == null || rom == null)
            {
                var a = db.DienThoais
                .Where(p => p.TenSp == ten && p.MaMau == maMau)
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
                    maRom = p.MaBoNhoTrong,
                    maRam = p.MaRam,
                    maMau = p.MaMau,
                    ManHinh = p.ManHinh,
                    MoTa = p.MoTa,
                    AnhThongSo = p.AnhThongSo ?? ""
                })
                .FirstOrDefault();

                return View(a);
            }

            string maRam = "";
            string maRom = "";
            if (ram != "1" && rom!= "1" )
            {
                maRam = ram;
                maRom = rom;
            }

            if (form != null && form.Count > 0)
            {
                string ramRomValue = form["ramRom"]; // Lấy giá trị từ dropdown  

                // Kiểm tra nếu ramRomValue không null và chứa dấu '-'  
                if (!string.IsNullOrEmpty(ramRomValue) && ramRomValue.Contains('-'))
                {
                    var parts = ramRomValue.Split('-'); // Tách ra thành mã RAM và mã ROM  

                    // Kiểm tra số lượng phần tử sau khi tách  
                    if (parts.Length >= 2)
                    {
                        maRam = parts[0]; // Mã RAM  
                        maRom = parts[1]; // Mã ROM   
                    }
                    else
                    {
                        // Xử lý khi không có đủ dữ liệu  
                        throw new Exception("Giá trị không hợp lệ: cần ít nhất 2 phần tử.");
                    }
                }
                else
                {
                    // Xử lý trường hợp giá trị không hợp lệ  
                    throw new Exception("Giá trị ramRom không hợp lệ hoặc không chứa dấu '-'.");
                }
            }

            ViewBag.SelectedRamRom = $"{maRam}-{maRom}";

            var dienThoai = db.DienThoais
                .Where(p => p.TenSp == ten && p.MaMau == maMau && p.MaRam == maRam && p.MaBoNhoTrong == maRom)
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
                    maRom = p.MaBoNhoTrong,
                    maRam = p.MaRam,
                    maMau = p.MaMau,
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
