namespace Project.ViewModels
{
    public class GioHangItem
    {
        public string MaSp { get; set; }
        public string? HinhAnh { get; set; }
        public string? TenSp {  get; set; }
        public string? Mau { get; set; }
        public string? DungLuong { get; set; }

        public string? DungLuongRam { get; set; }
        public int SoLuong { get; set; }
        public int Sl { get; set;}
        public decimal Gia { get; set; }
        public decimal ThanhTien => Gia * SoLuong;
    }
}
