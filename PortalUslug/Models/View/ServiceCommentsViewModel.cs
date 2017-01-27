using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.Pagination;

namespace PortalUslug.Models.View
{
    public class ServiceCommentsViewModel
    {
        public ServiceViewModel Service { get; set; }
        public IPagination<CommentViewModel> CommentPagedList { get; set; }
        public bool ConfirmedUser { get; set; }
    }
}