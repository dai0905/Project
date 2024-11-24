using System;
using System.Collections.Generic;

namespace Project.Data;

public partial class CtHdBanHang
{
    public string MaHd { get; set; } = null!; // Khóa ngoại tới bảng HdBanHang

    public string MaSp { get; set; } = null!; // Khóa ngoại tới bảng DienThoai

    public decimal Gia { get; set; }

    public int SoLuong { get; set; }

    // Navigation properties
    public virtual HdBanHang MaHdNavigation { get; set; } = null!;
    public virtual DienThoai MaSpNavigation { get; set; } = null!;
}
