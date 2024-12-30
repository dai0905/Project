using System.Text;

namespace Project.Helpers
{
    public class MyUtil
    {
        public static string UpLoadHinh(IFormFile Hinh)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", Hinh.FileName);

            if (System.IO.File.Exists(fullPath))
            {
                // Nếu file đã tồn tại, trả về tên file  
                return Hinh.FileName;
            }

            try
            {
                using (var myfile = new FileStream(fullPath, FileMode.Create))
                {
                    Hinh.CopyTo(myfile);
                }
                return Hinh.FileName;
            } catch(Exception ex)
            {
                return string.Empty;
            }
            
        }
    }
}
