using System;
using System.Collections.Generic;

namespace Project.Data;

public partial class HdBanHang
{
    public string MaHd { get; set; } = null!; // Khóa chính

    public string MaTaiKhoan { get; set; } = null!; // Khóa ngoại tới bảng TaiKhoan

    public string Ten { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public string DiaChi { get; set; } = null!;

    public DateTime NgayBan { get; set; }

    public decimal TongTien { get; set; }

    public string MaTrangThai { get; set; } = null!; // Khóa ngoại tới bảng TrangThai

    // Quan hệ 1-nhiều: 1 hóa đơn có nhiều chi tiết hóa đơn
    public virtual ICollection<CtHdBanHang> CtHdBanHangs { get; set; } = new List<CtHdBanHang>();

    // Navigation properties
    public virtual TaiKhoan MaTaiKhoanNavigation { get; set; } = null!;
    public virtual TrangThai MaTrangThaiNavigation { get; set; } = null!;
}
