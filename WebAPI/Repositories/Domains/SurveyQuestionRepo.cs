using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SurveyQuestionRepo : Repository<SurveyQuestion>, ISurveyQuestionRepo
    {
        public SurveyQuestionRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
