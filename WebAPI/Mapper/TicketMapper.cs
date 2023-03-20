using AutoMapper;
using WebAPI.Models;
using HelperClasses.DTOs; 

namespace WebAPI.Mapper
{
    public class TicketMapper : Profile
    {
        public TicketMapper()
        {
            CreateMap<Ticket, TicketDTO>();
            CreateMap<TicketDTO, Ticket>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));

            CreateMap<TicketMessages, TicketMessagesDTO>();
            CreateMap<TicketMessagesDTO, TicketMessages>();

            CreateMap<TicketDocument, TicketDocumentDTO>();
            CreateMap<TicketDocumentDTO, TicketDocument>();
        }
    }
}
