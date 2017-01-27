using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalUslug.Models
{
    public class Newsletter
    {       
        [Required(ErrorMessage = "Temat jest wymagany.")]
        [Display(Name = "Temat")]
        [StringLength(20, ErrorMessage = "Temat może mieć maksymalnie 20 znaków.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Treść jest wymagana.")]
        [Display(Name = "Treść")]
        [StringLength(200, ErrorMessage = "Treść może mieć maksymalnie 200 znaków.")]
        public string Content { get; set; }
    }
}