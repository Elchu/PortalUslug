using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using PortalUslug.Repositories;

namespace PortalUslug.Models.View
{
    public class ServiceViewModel
    {
        [ScaffoldColumn(false)]
        public int ServiceId { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data umieszczenia")]
        public DateTime PostedDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Data wygaśnięcia")]
        public DateTime ExpirationDate { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Treść usługi")]
        public string Content { get; set; }

        [ScaffoldColumn(false)]
        public int CategoryId { get; set; }

        [Display(Name = "Kategoria")]
        public string CategoryName { get; set; }

        [Display(Name = "Usługodawca")]
        public string ServiceProvider { get; set; }

        [Display(Name = "Adres IP")]
        public string IPAddress { get; set; }

        [ScaffoldColumn(false)]
        public string IsActive { get; set; }

        public string UserId { get; set; }
    }

    public class ServiceFilterViewModel
    {
        private int selectedCategoryId = -1;
        private int selectedServiceProviderId = -1;

        public IEnumerable<SelectListItem> Categories { get; set; }

        public IEnumerable<SelectListItem> ServiceProvider { get; set; }

        public string ServiceName { get; set; }

        public string ServiceContent { get; set; }


        public int SelectedCategoryId
        {
            get { return selectedCategoryId; }

            set { selectedCategoryId = value; }
    
        }

        public int SelectedServiceProviderId
        {
            get { return selectedServiceProviderId; }

            set { selectedServiceProviderId = value; }
        }

        public void Fill()
        {
            CategoryRepository _categoryRepo = new CategoryRepository();
            ServiceProviderRepository _serviceProviderRepo = new ServiceProviderRepository();

            //Categories = new SelectList(_categoryRepo.GetAllCategories(), "CategoryId", "Name", SelectedCategoryId);
            //ServiceProvider = new SelectList(_serviceProviderRepo.GetAllServiceProvider(), "ServiceProviderId", "Name", SelectedServiceProviderId);

            Categories = new SelectList(_categoryRepo.GetAllCategories(), "CategoryId", "Name", SelectedCategoryId);
            ServiceProvider = new SelectList(_serviceProviderRepo.GetAllServiceProvider(), "ServiceProviderId", "Name", SelectedServiceProviderId);
        }
    }

    public class ServiceListContainerViewModel
    {
        public IPagination<ServiceViewModel> ServicePagedList{ get; set; }
        public ServiceFilterViewModel ServiceFilterViewModel { get; set; }
        public GridSortOptions GridSortOptions { get; set; }
    }
}