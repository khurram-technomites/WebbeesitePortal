using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class CustomerMapper : Profile
    {
        public CustomerMapper()
        {
            CreateMap<Customer, CustomerDTO>()
                .ForMember(x => x.Logo, x => x.MapFrom(y => y.User.Logo));

            CreateMap<CustomerDTO, Customer>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null)); ;

        }
    }
}
