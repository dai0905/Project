using System;
using System.Collections.Generic;

namespace Project.Data;

public partial class GioHang
{
    public string MaTaiKhoan { get; set; } = null!;

    public string MaSp { get; set; } = null!;

    public int SoLuong { get; set; }

    public decimal Gia { get; set; }

    public virtual DienThoai MaSpNavigation { get; set; } = null!;

    public virtual TaiKhoan MaTaiKhoanNavigation { get; set; } = null!;
}
