using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBasedSystem.Models;

namespace WebBasedSystem.ViewModels
{
    public class NotesModel
    {
        public int NoteId { get; set; }

        public string NotesDescription { get; set; }

        public Nullable<bool> TakingNewPatients { get; set; }

        public string price { get; set; }

        public string Waiting_Time { get; set; }
        public int Waiting_Time_Id { get; set; }



        public Nullable<int> SpecialistId { get; set; }

        public Nullable<bool> IsBooking { get; set; }



        public virtual Specialist Specialist { get; set; }
    }
}