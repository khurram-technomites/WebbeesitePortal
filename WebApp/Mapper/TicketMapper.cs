using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Helpers;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class TicketMapper : Profile
    {
        public TicketMapper()
        {
            CreateMap<TicketDTO, TicketViewModel>();
            CreateMap<TicketViewModel, TicketDTO>()
                .ForMember(x => x.CreationDate , x => x.MapFrom(y => y.CreationDate.ToDubaiDateTime()));

            CreateMap<TicketMessagesDTO, TicketMessageViewModel>();
            CreateMap<TicketMessageViewModel, TicketMessagesDTO>();

            CreateMap<TicketDocumentDTO, TicketDocumentViewModel>();
            CreateMap<TicketDocumentViewModel, TicketDocumentDTO>();
        }
    }
}
