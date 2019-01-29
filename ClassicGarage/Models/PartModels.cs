using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClassicGarage.Models
{
    public class PartModels
    {
        public int ID { get; set; }
        public int RepairID { get; set; }

        [Required(ErrorMessage = "Wprowadź nazwe")]
        [DisplayName("Nazwa części")]
        public string Name { get; set; }
        [DisplayName("Nr katalogowy części")]
        public string CatalogNumber { get; set; }
        [Required(ErrorMessage = "Wprowadź cene zakupu")]
        [DisplayName("Cena zakupu")]
        public double PurchaseAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Data zakupu")]
        public DateTime PurchaseDate { get; set; }
        [DisplayName("Cena sprzedaży")]
        public double? SalesAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Data sprzedaży")]
        public DateTime? SalesDate { get; set; }

        public virtual RepairModels Repair { get; set; }

    }
}