using System;
using System.Collections.Generic;

namespace Project.Data;

public partial class TaiKhoan
{
    public string MaTaiKhoan { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public int? MaQuyen { get; set; }

    public string Ten { get; set; } = null!;

    public string Sdt { get; set; } = null!;

    public string DiaChi { get; set; } = null!;

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual ICollection<HdBanHang> HdBanHangs { get; set; } = new List<HdBanHang>();

    public virtual PhanQuyen? MaQuyenNavigation { get; set; }
}
