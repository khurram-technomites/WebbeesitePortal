using AutoMapper;
using HelperClasses.DTOs.CurrencyNote;
using WebAPI.Models;

namespace WebAPI.Mapper.CurrencyNoteMapper
{
    public class CurrencyNoteMapper:Profile
    {
        public CurrencyNoteMapper()
        {
            CreateMap<CurrencyNote, CurrencyNoteDTO>();
            CreateMap<CurrencyNoteDTO, CurrencyNote>();
        }
    }
}
