using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedSystem.ViewModels
{
    public class WayPointsModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Latitude")]
        public Nullable<decimal> Latitude { get; set; }
        [Required(ErrorMessage = "Please enter Longitude")]
        public Nullable<decimal> Longitude { get; set; }

        [Required(ErrorMessage = "Please enter Suburbs")]
        public string Suburbs { get; set; }
        [Required(ErrorMessage = "Please enter PostCode")]
        public string PostCode { get; set; }
        
        public string State { get; set; }
        [Required(ErrorMessage = "Please enter State")]
        public int StateId { get; set; }
       

        public Nullable<int> SpecialistTypeId { get; set; }
    }
}