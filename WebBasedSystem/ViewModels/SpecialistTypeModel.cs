using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedSystem.ViewModels
{
    public class SpecialistTypeModel
    {
        [Required(ErrorMessage = "Please enter Specialist Type Name")]
        public string SpecialistTypeName { get; set; }
        [Required(ErrorMessage = "Please enter Specialist Type Name")]
        public int SpecialistTypeId { get; set; }
    }
}