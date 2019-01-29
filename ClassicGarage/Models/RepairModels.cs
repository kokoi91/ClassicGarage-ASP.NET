using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace ClassicGarage.Models
{
    public class RepairModels
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Marka auta")]
        public int? CarID { get; set; }
        [Required(ErrorMessage = "Wprowadź nazwe")]
        [DisplayName("Nazwa naprawy")]
        public string Name { get; set; }
        [StringLength(200)]
        [DisplayName("Opis naprawy")]
        public string Description { get; set; }
        [DisplayName("Cena naprawy")]
        public double? RepairAmount { get; set; }


        public virtual CarModels Car { get; set; }

        public ICollection<PartModels> Part { get; set; }

    }
}