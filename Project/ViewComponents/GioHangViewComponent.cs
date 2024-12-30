using Microsoft.AspNetCore.Mvc;
using Project.Helpers;
using Project.ViewModels;
namespace Project.ViewComponents
{
    public class GioHangViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() { 
            var gioHang = HttpContext.Session.Get<List<GioHangItem>>(MySetting.GIOHANG_KEY) ?? new List<GioHangItem>();
            return View("GioHangPanel", new GioHangModel
            {
                Quantity = gioHang.Sum(p => p.SoLuong),
                TongTien = gioHang.Sum(p => p.ThanhTien)
            });
        }
    }
}
