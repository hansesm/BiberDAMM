﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiberDAMM.ViewModels
{
    public class CreationContactType
    {
        [Required]
        [Display(Name = "Kontakttyp")]
        public string Name { get; set; }
    }
}