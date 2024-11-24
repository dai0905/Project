using System;
using System.Collections.Generic;

namespace Project.Data;

public partial class DienThoai
{
    public string MaSp { get; set; } = null!;

    public string? TenSp { get; set; }

    public string? MaThuongHieu { get; set; }

    public string? HeDieuHanh { get; set; }

    public string? CameraTruoc { get; set; }

    public string? CameraSau { get; set; }

    public string? ManHinh { get; set; }

    public string? MaBoNhoTrong { get; set; }

    public string? MaRam { get; set; }

    public string? Cpu { get; set; }

    public string? Pin { get; set; }

    public string? ChatLieu { get; set; }

    public string? Ktkl { get; set; }

    public string? Tdrm { get; set; }

    public string? MaMau { get; set; }

    public int? Sl { get; set; }

    public decimal? GiaMoi { get; set; }

    public decimal? GiaCu { get; set; }

    public string? MoTa { get; set; }

    public string? HinhAnh { get; set; }

    public string? AnhThongSo { get; set; }

    public virtual ICollection<CtHdBanHang> CtHdBanHangs { get; set; } = new List<CtHdBanHang>();

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();

    public virtual BoNhoTrong? MaBoNhoTrongNavigation { get; set; }

    public virtual Mau? MaMauNavigation { get; set; }

    public virtual Ram? MaRamNavigation { get; set; }

    public virtual ThuongHieu? MaThuongHieuNavigation { get; set; }
}
