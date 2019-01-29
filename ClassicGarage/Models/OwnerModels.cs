using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClassicGarage.Models
{
    public class OwnerModels
    {
        public int ID { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage ="Wprowadź imie")]
        [DisplayName("Imie właściciela")]
        public string FirstName { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Wprowadź Nazwisko")]
        [DisplayName("Nazwisko właściciela")]
        public string LastName { get; set; }
        [DisplayName("Nr telefonu")]
        public int PhoneNo { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Wprowadź email")]
        [DisplayName("Email")]
        public string Email { get; set; }


        public virtual ICollection<CarModels> Cars { get; set; }
    }
}