using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PortalUslug.Models
{
    public class Comment
    {
        [Key]
        public int CommnetId { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data dodania")]
        public DateTime Date { get; set; }
        
        [Required(ErrorMessage = "Treść jest wymagana.")]
        [Display(Name = "Treść komentarza")]
        [StringLength(200, ErrorMessage = "Treść komentarza może mieć maksymalnie 200 znaków.")]
        public string Content { get; set; }
        
        public string IPAddress { get; set; }
        
        public string UserId { get; set; }

        [Display(Name = "Usługa")]
        public int ServiceId { get; set; }
        
        [Display(Name = "Kategoria")]
        public int CommentCategoryId { get; set; }

        public virtual Service Services { get; set; }
        public virtual CommentCategory CommentCategory { get; set; }

    }
}
