using System.ComponentModel.DataAnnotations;

namespace Project.ViewModels
{
    public class ThayDoiMKVM
    {
        [Display(Name = "Mật khẩu cũ")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 kí tự")]
        [MinLength(6, ErrorMessage = "Tối thiểu 6 kí tự")]
        public string MatKhauCu { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 kí tự")]
        [MinLength(6, ErrorMessage = "Tối thiểu 6 kí tự")]
        public string MatKhauMoi { get; set; }

        [Display(Name = "Nhập lại mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*")]
        [Compare("MatKhauMoi", ErrorMessage = "Mật khẩu không khớp")]
        public string NhapLaiMatKhau { get; set; }
    }
}
