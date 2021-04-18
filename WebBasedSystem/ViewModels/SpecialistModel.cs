using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBasedSystem.ViewModels
{
    public class SpecialistModel
    {
        public int SpecialistId { get; set; }
        [Required(ErrorMessage = "Please enter Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter SpecialistName")]
        public string SpecialistName { get; set; }
        [Required(ErrorMessage = "Please enter Latitude")]
        public Nullable<decimal> Latitude { get; set; }
        [Required(ErrorMessage = "Please enter Longitude")]
        public Nullable<decimal> Longitude { get; set; }
        [Required(ErrorMessage = "Please enter MobileNo")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Please enter PhoneNo")]
        public string PhoneNo { get; set; }
        [Required(ErrorMessage = "Please enter Address1")]
        public string Address1 { get; set; }

        public string Address2 { get; set; }
        [Required(ErrorMessage = "Please enter EmailAddress")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Please enter BusinessName")]
        public string BusinessName { get; set; }
        [Required(ErrorMessage = "Please enter SpecialistType")]
        public Nullable<int> SpecialistTypeId { get; set; }
        [Required(ErrorMessage = "Please enter Surname")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Please enter OfficeNo")]
        public string OfficeNo { get; set; }

        public Nullable<bool> IsDeleted { get; set; }
        [Required(ErrorMessage = "Please enter SubMenu")]
        public string SubMenu { get; set; }
        [Required(ErrorMessage = "Please enter Age")]
        public Nullable<int> Age { get; set; }
        [Required(ErrorMessage = "Please enter SubMenuId")]
        public int SubMenuId { get; set; }
        [Required(ErrorMessage = "Please enter TitleId")]
        public int TitleId { get; set; }
        [Required(ErrorMessage = "Please enter PostCode")]
        public string PostCode { get; set; }
        [Required(ErrorMessage = "Please enter Suburbs")]
        public string Suburbs { get; set; }
        [Required(ErrorMessage = "Please enter State")]
        public string State { get; set; }
        [Required(ErrorMessage = "Please enter NotesDescription")]
        public string NotesDescription { get; set; }
        [Required(ErrorMessage = "Please enter Taking New Patients")]
        public Nullable<bool> TakingNewPatients { get; set; }

[DisplayName("Price")]
[Range(0.01, 100000.00)]
        [Required(ErrorMessage = "Please enter Price")]
        public Nullable<decimal> price { get; set; }
        [Required(ErrorMessage = "Please enter waiting time")]
        public string Waiting_Time { get; set; }
        public Nullable<bool> IsBooking { get; set; }
        public int Waiting_Time_Id { get; set; }


    }
}