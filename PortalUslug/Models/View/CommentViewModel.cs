using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalUslug.Models.View
{
    public class CommentViewModel
    {
        [ScaffoldColumn(false)]
        public int CommentId { get; set; }

        [Required(ErrorMessage = "Treść jest wymagana.")]
        [Display(Name = "Treść komentarza")]
        [StringLength(200, ErrorMessage = "Treść komentarza może mieć maksymalnie 200 znaków.")]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data dodania")]
        public DateTime Date { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Usługa")]
        public int ServiceId { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Usługa")]
        public string ServiceName { get; set; }

        [ScaffoldColumn(false)]
        public string UserId { get; set; }

        [Display(Name = "Użytkownik")]
        public string User { get; set; }

        [Display(Name = "Adres IP")]
        public string IPAddress { get; set; }

        [Display(Name = "Kategoria")]
        public int CommentCategoryId { get; set; }

        [Display(Name = "Kategoria")]
        public string CommentCategoryName { get; set; }
    }
}