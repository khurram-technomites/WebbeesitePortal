using AutoMapper;
using HelperClasses.DTOs.Survey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels.Survey;

namespace WebApp.Mapper
{
    public class SurveyMapper : Profile
    {
        public SurveyMapper()
        {
            CreateMap<SurveyDTO, SurveyViewModel>();
            CreateMap<SurveyViewModel, SurveyDTO>();

            CreateMap<SurveyQuestionDTO, SurveyQuestionViewModel>();
            CreateMap<SurveyQuestionViewModel, SurveyQuestionDTO>();

            CreateMap<SurveyOptionDTO, SurveyOptionViewModel>();
            CreateMap<SurveyOptionViewModel, SurveyOptionDTO>();
        }
    }
}
