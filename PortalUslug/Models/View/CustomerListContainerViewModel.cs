using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;

namespace PortalUslug.Models.View
{
    public class CustomerListContainerViewModel
    {
        public IPagination<CustomerViewModel> CustomerPageList { get; set; }
        public CustomerFilterViewModel CustomerFilterViewModel { get; set; }
        public GridSortOptions GridSortOptions { get; set; }
    }
}