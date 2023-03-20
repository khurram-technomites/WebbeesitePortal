using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
namespace WebApp.Mapper
{
    public class ClientEmailsMapper:Profile
    {
        public ClientEmailsMapper()
        {
            CreateMap<ClientEmails, ClientEmailsDTO>();
            CreateMap<ClientEmailsDTO, ClientEmails>();
        }

    }
}
