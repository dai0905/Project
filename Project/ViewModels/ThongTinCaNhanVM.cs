using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class ThongTinCaNhanVM
    {
        public string MaTaiKhoan { get; set; }
        [Display(Name = "Họ và tên")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 kí tự")]
        public string Ten { get; set; }
        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"0[98753]\d{8}", ErrorMessage = "Chưa đúng định dạng số điện thoại")]
        public string Sdt { get; set; }
        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 kí tự")]
        public string DiaChi { get; set; }
    }
}
