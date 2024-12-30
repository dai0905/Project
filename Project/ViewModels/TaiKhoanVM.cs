using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class TaiKhoanVM
    {
        [Display(Name = "Tài Khoản")]
        [Required(ErrorMessage = "Chưa nhập Tài khoản")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 kí tự")]
        [MinLength(6, ErrorMessage = "Tối thiểu 6 kí tự")]
        public string MaTaiKhoan { get; set; } 
        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Chưa nhập Mật khẩu")]
        [DataType(DataType.Password)]
        [MaxLength(20, ErrorMessage = "Tối đa 20 kí tự")]
        [MinLength(6, ErrorMessage = "Tối thiểu 6 kí tự")]
        public string MatKhau { get; set; }
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
