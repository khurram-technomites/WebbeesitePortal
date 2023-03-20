﻿using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class ClientIndustriesMapper : Profile
    {
        public ClientIndustriesMapper()
        {
            CreateMap<ClientIndustriesDTO, ClientIndustriesViewModel>();
            CreateMap<ClientIndustriesViewModel, ClientIndustriesDTO>();
        }
    }
}