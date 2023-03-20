using AutoMapper;
using HelperClasses.DTOs.Aggregators;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class AggregatorMapper : Profile
    {
        public AggregatorMapper()
        {
            CreateMap<AggregatorDTO, AggregatorViewModel>();
            CreateMap<AggregatorViewModel, AggregatorDTO>();
        }
    }
}
