﻿using DocumentFormat.OpenXml.Drawing.Charts;
using Fingers10.ExcelExport.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class BusinessSettingViewModel
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string TitleAr { get; set; }

        public string WhatsApp { get; set; }

        public string FirstMessage { get; set; }

        public string FirstMessageAr { get; set; }

        public string MapIframe { get; set; }

        public string StreetAddress { get; set; }

        public string StreetAddressAr { get; set; }

        public string PhoneCode { get; set; }

        public string PhoneCode2 { get; set; }

        public string Contact { get; set; }

        public string Contact2 { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Email2 { get; set; }

        public string WorkingDays { get; set; }

        public string WorkingTime { get; set; }

        public string AndroidAppUrl { get; set; }

        public string IosAppUrl { get; set; }

        public string Facebook { get; set; }

        public string Instagram { get; set; }

        public string Youtube { get; set; }

        public string Twitter { get; set; }

        public string Snapchat { get; set; }
    }
}
