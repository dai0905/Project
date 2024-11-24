namespace Project.ViewModels
{
    public class DonHangVM
    {
        public string MaHd { get; set; } = null!;
        public string MaTaiKhoan { get; set; } = null!;
        public DateTime NgayBan { get; set; }
        public string MaSp { get; set; } = null!;
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
        public string HinhAnh { get; set; }
        public string TenSp { get; set; }
        public string Mau { get; set; }
        public string? DungLuong { get; set; }
        public string? DungLuongRam { get; set; }
        public decimal? ThanhTien => Gia * SoLuong;
        public decimal TongTien;
        public string TrangThai { get; set; } = null!;
    }

}
