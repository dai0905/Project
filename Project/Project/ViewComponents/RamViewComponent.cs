using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.ViewModels;

namespace Project.ViewComponents
{
    public class RamViewComponent : ViewComponent
    {
        private readonly ProjectContext db;

        public RamViewComponent(ProjectContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.Rams.Select(lo => new RamVM
            {
                MaRam = lo.MaRam,
                DungLuong = lo.DungLuong,
            });
            return View(data);
        }
    }
}
