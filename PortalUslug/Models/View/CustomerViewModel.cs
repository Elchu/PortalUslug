using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalUslug.Models.View
{
    public class CustomerViewModel
    {
        [ScaffoldColumn(false)]
        [Key]
        public int CustomerId { get; set; }

        [ScaffoldColumn(false)]
        public string UserId { get; set; }

        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        
        public string Email { get; set; }

        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }

        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name = "Aktywny")]
        public string IsActive { get; set; }

        [Display(Name = "Potwierdzony")]
        public bool IsConfirmed { get; set; }

        [Display(Name = "Data rejestracji")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Newsletter")]
        public bool Newsletter { get; set; }
    }
}