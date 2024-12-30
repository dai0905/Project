using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class DangNhapVM
    {
        [Display(Name = "Tài Khoản")]
        [Required(ErrorMessage = "Chưa nhập Tài khoản")]
        [MaxLength(20, ErrorMessage = "Tài khoản không được vượt quá 20 ký tự")]
        [MinLength(6, ErrorMessage = "Tài khoản phải có ít nhất 6 ký tự")]
        public string MaTaiKhoan { get; set; }

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Chưa nhập Mật khẩu")]
        [DataType(DataType.Password)]
        [MaxLength(20, ErrorMessage = "Mật khẩu không được vượt quá 20 ký tự")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string MatKhau { get; set; }
    }
}
