using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBasedSystem.ViewModels
{
    public class StandardUserModel
    {
        public Models.SpecialistType specialistType { get; set; }
        public Models.SpecialistTypeSubMenu specialistTypesubmenu { get; set; }

        public Models.WayPoint waypoints { get; set; }

    }
}