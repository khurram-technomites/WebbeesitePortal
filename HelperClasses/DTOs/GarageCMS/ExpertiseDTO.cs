﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.GarageCMS
{
    public class ExpertiseDTO
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
