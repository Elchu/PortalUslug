using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalUslug.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }
        
        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Treść jest wymagana.")]
        [Display(Name = "Treść usługi")]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data umieszczenia")]
        public DateTime PostedDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data wygaśnięcia")]
        public DateTime ExpirationDate { get; set; }

        public string IPAddress { get; set; }
        
        public string UserId { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Kategoria")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

    }
}