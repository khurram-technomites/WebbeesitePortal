using AutoMapper;
using HelperClasses.DTOs.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.SurveyMapper
{
    public class SurveyMapper : Profile
    {
        public SurveyMapper()
        {
            CreateMap<Survey, SurveyDTO>();
            CreateMap<SurveyDTO, Survey>();

            CreateMap<SurveyQuestion, SurveyQuestionDTO>();
            CreateMap<SurveyQuestionDTO, SurveyQuestion>()
                .ForAllMembers(x => x.Condition((source, destination, member) => member != null))
                ;

            CreateMap<SurveyOption, SurveyOptionDTO>();
            CreateMap<SurveyOptionDTO, SurveyOption>();
        }
    }
}
