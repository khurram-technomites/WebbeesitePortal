using AutoMapper;
using HelperClasses.DTOs.Aggregators;
using WebAPI.Models;

namespace WebAPI.Mapper.AggregatorMapper
{
    public class AggregatorMapper:Profile
    {
        public AggregatorMapper()
        {
            CreateMap<Aggregator, AggregatorDTO>();
            CreateMap<AggregatorDTO, Aggregator>();
        }
    }
}
