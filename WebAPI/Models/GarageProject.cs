﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageProject : GeneralSchema
    {
        public GarageProject()
        {
            GarageProjectImages = new HashSet<GarageProjectImages>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Garage))]
        public long GarageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Garage Garage { get; set; }
        public ICollection<GarageProjectImages> GarageProjectImages { get; set; }
    }
}
