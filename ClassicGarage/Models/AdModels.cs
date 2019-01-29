using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClassicGarage.Models
{
    public class AdModels
    {
        public int ID { get; set; }
        [DisplayName("Marka auta")]
        public int? CarID { get; set; }
        [DisplayName("Czy aktywne")]
        public bool? IfActive { get; set; }
        [Required(ErrorMessage = "Wprowadź opis")]
        [DisplayName("opis")]
        public string des { get; set; }

        public virtual CarModels Car { get; set; }
        

    }
}