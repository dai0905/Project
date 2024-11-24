using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.ViewModels;

namespace Project.ViewComponents
{
    public class BoNhoTrongViewComponent : ViewComponent
    {
        private readonly ProjectContext db;

        public BoNhoTrongViewComponent(ProjectContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.BoNhoTrongs.Select(lo => new BoNhoTrongVM
            {
                MaBoNhoTrong = lo.MaBoNhoTrong,
                DungLuong = lo.DungLuong,
            });
            return View(data);
        }
    }
}
