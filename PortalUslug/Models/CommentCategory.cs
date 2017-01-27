using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PortalUslug.Models
{
    public class CommentCategory
    {
        [Key]
        public int CommentCategoryId { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana.")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        
        public virtual ICollection<Comment> Comments{ get; set; }

    }
}
