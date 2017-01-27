using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalUslug.Models.View
{
    public class ServiceProviderViewModel
    {
        [ScaffoldColumn(false)]
        public int ServiceProviderId { get; set; }

        [ScaffoldColumn(false)]
        public string UserId { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }

        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name = "Nr telefonu")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Potwierdzony")]
        public bool IsConfirmed { get; set; }
         
        [Display(Name = "Aktywny")]
        public string IsActive { get; set; }

        [Display(Name = "Data rejestracji")]
        public DateTime RegistrationDate { get; set; }

        [Display(Name = "Newsletter")]
        public bool Newsletter { get; set; }
    }

    public class ServiceProviderFilterViewModel
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }

    public class ServiceProviderListContainerViewModel
    {
        public IPagination<ServiceProviderViewModel> ServiceProviderPagedList { get; set; }
        public ServiceProviderFilterViewModel ServiceProviderFilterViewModel { get; set; }
        public GridSortOptions GridSortOptions { get; set; }
    }
}