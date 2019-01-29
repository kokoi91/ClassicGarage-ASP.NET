using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassicGarage.Models
{
    public class CarModels
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Wprowadź Marke")]
        [DisplayName("Marka")]
        public string Make { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Wprowadź model")]
        [DisplayName("Model")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Wprowadź rok produkcji")]
        [DisplayName("Rok produkcji")]
        public int Year { get; set; }
        [DisplayName("Nr VIN")]
        public string VIN { get; set; }
        [DisplayName("Nazwa")]
        public string Name { get; set; }
        public string Picture { get; set; }
        [Required(ErrorMessage = "Wprowadź date zakupu")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Data zakupu")]
        public DateTime PurchaseDate { get; set; }
        [DisplayName("Cena zakupu")]
        public double PurchaseAmount { get; set; }
        [DisplayName("Data sprzedaży")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SalesDate { get; set; }
        [DisplayName("Cena sprzedaży")]
        public double? SalesAmount { get; set; }
        public int OwnerID { get; set; }


        public virtual OwnerModels Owner { get; set; }       
        public virtual ICollection<RepairModels> Repair { get;    set; }


    }
}