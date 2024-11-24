using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.ViewModels;

namespace Project.ViewComponents
{
    public class ThuongHieuViewComponent :ViewComponent
    {
        private readonly ProjectContext db;

        public ThuongHieuViewComponent(ProjectContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.ThuongHieus.Select(lo => new ThuongHieuVM
            {
                MaThuongHieu = lo.MaThuongHieu,
                TenThuongHieu = lo.TenThuongHieu,
            });
            return View(data);
        }
    }
}
