using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class DangKyVM
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Tài khoản")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 kí tự")]
        [MinLength(6, ErrorMessage = "Tối thiểu 6 kí tự")]
        public string MaTaiKhoan { get; set; } = null!;

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 kí tự")]
        [MinLength(6, ErrorMessage = "Tối thiểu 6 kí tự")]
        public string MatKhau { get; set; } = null!;

        [Display(Name = "Nhập lại mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu không khớp")]
        public string MatKhauXacNhan { get; set; } = null!;

        [Display(Name = "Họ và tên")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 kí tự")]
        public string Ten { get; set; } = null!;

        [Display(Name = "Số điện thoại")]
        [RegularExpression(@"0[98753]\d{8}", ErrorMessage = "Chưa đúng định dạng số điện thoại")]
        public string Sdt { get; set; } = null!;

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "*")]
        [MaxLength(100, ErrorMessage = "Tối đa 100 kí tự")]
        public string DiaChi { get; set; } = null!;
    }
}
