﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalUslug.Models
{
    public class ServiceProvider
    {
        [Key]
        [ScaffoldColumn(false)]
        public int ServiceProviderId { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [RegularExpression(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$", ErrorMessage = "Adres e-mail jest niepoprawny.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Miasto jest wymagane.")]
        [StringLength(40, ErrorMessage = "Miasto może mieć maksymalnie 40 znaków.")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required(ErrorMessage = "Kod pocztowy jest wymagany.")]
        [Display(Name = "Kod pocztowy")]
        [RegularExpression(@"^[0-9]{2}\-[0-9]{3}$", ErrorMessage = "Wprowadzony kod jest niepoprawny.")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Ulica jest wymagana.")]
        [StringLength(40, ErrorMessage = "Ulica może mieć maksymalnie 40 znaków.")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name = "Nr telefonu")]
        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "Wprowadzony numer telefonu jest niepoprawny.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Potwierdzony")]
        public bool IsConfirmed { get; set; }

        [Display(Name = "Data rejestracji")]
        [DisplayFormat(DataFormatString = "{0:yy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; }
        
        public bool Newsletter { get; set; }

        [ScaffoldColumn(false)]
        public string UserId { get; set; }

    }
}