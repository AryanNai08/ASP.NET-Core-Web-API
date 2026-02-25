using AutoMapper;
using CollegeApi.Data;
using CollegeApi.Models;

namespace CollegeApi.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {

            //congiguration for diffrent property name 
            //CreateMap<StudentDTO,Student>().ForMember(x => x.Studentname,opt=>opt.MapFrom(x=>x.Name)).ReverseMap();


            //congiguration for ignoring some property
            //CreateMap<StudentDTO, Student>().ReverseMap().ForMember(x => x.Studentname, Opt => Opt.Ignore());

            //congigurattion for transforming some propery
            //CreateMap<StudentDTO, Student>().ReverseMap().AddTransform<string>(n=>string.IsNullOrEmpty(n)?"No address found":n);

            //congiguration for particular column
            //CreateMap<StudentDTO, Student>()
            //    .ReverseMap()
            //    .ForMember(dest => dest.Address,
            //        opt => opt.MapFrom(src =>
            //            string.IsNullOrEmpty(src.Address) ? "No address found" : src.Address
            //        ));

            CreateMap<StudentDTO, Student>().ReverseMap();
            CreateMap<RoleDTO, Role>().ReverseMap();
        }
    }
}
