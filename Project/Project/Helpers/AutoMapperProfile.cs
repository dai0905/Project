using AutoMapper;
using Project.Data;
using Project.ViewModels;

namespace Project.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() {
            CreateMap<DangKyVM, TaiKhoan>();
        }   
    }
}
