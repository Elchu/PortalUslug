using MvcContrib.UI.Grid;
using PortalUslug.Models.View;
using PortalUslug.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Sorting;
using MvcContrib.Pagination;
using Microsoft.AspNet.Identity;
using PortalUslug.Models;

namespace PortalUslug.Controllers
{
    public class ServiceProviderController : Controller
    {
        private readonly ServiceProviderRepository _serviceProviderRepo;
        private readonly CommentRepository _commentRepo;
        private readonly ServiceRepository _serviceRepo;

        public ServiceProviderController()
        {
            _serviceProviderRepo = new ServiceProviderRepository();
            _commentRepo = new CommentRepository();
            _serviceRepo = new ServiceRepository();
        }

        [Authorize]
        public ActionResult Index(string Name, string City, string Street, GridSortOptions sort,
            [DefaultValue(1)] int page)
        {
            IQueryable<ServiceProviderViewModel> providers = _serviceProviderRepo.GetAllServiceProvider();

            if (!User.IsInRole("administrator"))
                providers.Where(s => s.IsConfirmed == true);

            if (string.IsNullOrWhiteSpace(sort.Column))
                sort.Column = "ServiceProviderId";

            if (!string.IsNullOrWhiteSpace(Name))
                providers = providers.Where(c => c.Name.Contains(Name));

            if (!string.IsNullOrWhiteSpace(City))
                providers = providers.Where(c => c.City.Contains(City));

            if (!string.IsNullOrWhiteSpace(Street))
                providers = providers.Where(c => c.Street.Contains(Street));

            //potrzebna przestrzen nazw using MvcContrib.Sorting;  using MvcContrib.Pagination;
            var serviceProviderPagedList = providers.OrderBy(sort.Column, sort.Direction).AsPagination(page, 5);

            ServiceProviderFilterViewModel serviceProviderFilterViewModel = new ServiceProviderFilterViewModel();

            ServiceProviderListContainerViewModel customerListContainerViewModel = new ServiceProviderListContainerViewModel
            {
                ServiceProviderPagedList = serviceProviderPagedList,
                ServiceProviderFilterViewModel = serviceProviderFilterViewModel,
                GridSortOptions = sort
            };

            return View(customerListContainerViewModel);
        }

        [Authorize(Roles = "serviceProvider")]
        public ActionResult Create()
        {
            ServiceProvider serviceProvider = _serviceProviderRepo.GetServiceProviderByUserId(User.Identity.GetUserId());
            //jesli uzytkownik ma juz uzupelniony profil to przekierowujemy do edycji
            if (serviceProvider != null)
                return RedirectToAction("Edit", new {id = serviceProvider.ServiceProviderId});

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "serviceProvider")]
        public ActionResult Create(ServiceProvider serviceProvider)
        {
            serviceProvider.RegistrationDate = DateTime.Now;
            serviceProvider.UserId = User.Identity.GetUserId();
            serviceProvider.IsConfirmed = false;

            if (ModelState.IsValid)
            {
                try
                {
                    _serviceProviderRepo.Add(serviceProvider);
                    _serviceProviderRepo.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["Error"] = "Uzupełnij poprawnie formularz!";
                }

                TempData["Message"] = "Profil usługobiorcy został poprawnie dodany";
                return RedirectToAction("Index");
            }

            return View(serviceProvider);
        }

        [Authorize(Roles = "administrator, serviceProvider")]
        public ActionResult Edit(int id)
        {
            ServiceProvider serviceProvider = _serviceProviderRepo.GetServiceProviderById(id);

            //using Microsoft.AspNet.Identity;
            if (serviceProvider.UserId == User.Identity.GetUserId() || User.IsInRole("administrator"))
                return View(serviceProvider);

            TempData["Error"] = "Nie masz uprawnień do edytowania tego usługodawcy!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "administrator, serviceProvider")]
        public ActionResult Edit(ServiceProvider serviceProvider)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _serviceProviderRepo.Edit(serviceProvider);
                    _serviceProviderRepo.SaveChanges();

                    TempData["Message"] = "Pomyślnie zaktualizowano dane!";
                }
                catch (Exception)
                {
                    TempData["Error"] = "Wystąpił błąd podczas uaktualniania. Srpóbuj później!";
                }

                if (User.IsInRole("administrator"))
                    return RedirectToAction("Index");
            }

            TempData["Error"] = "Uzupełnij poprawnie profil";
            return View(serviceProvider);
        }

        [Authorize(Roles = "administrator")]
        public ActionResult Delete(int id)
        {
            ServiceProvider serviceProvider = _serviceProviderRepo.GetServiceProviderById(id);

            if (serviceProvider != null)
            {
                bool customerComments = _commentRepo.HasUserComment(serviceProvider.UserId);
                bool service = _serviceRepo.HasUserServices(serviceProvider.UserId);

                if (!customerComments && !service)
                    return View(serviceProvider);

                TempData["Error"] = "Nie można usunąć usługodawcy ponieważ posiada komentarze!";
            }
            else
                TempData["Error"] = "Niestaty usługodawca o podanym ID nieistnieje!";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                ServiceProvider serviceProvider = _serviceProviderRepo.GetServiceProviderById(id);
                _serviceProviderRepo.Delete(serviceProvider);

                TempData["Message"] = "Usługodawca został usunięty!";
            }
            catch
            {
                TempData["Error"] = "Wystąpił błąd podczas usuwania usługodawcy. Spróbuj później!";
                return RedirectToAction("Delete", id);
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Details(int id, string ServiceName, string ServiceContent, int? CategoryId, GridSortOptions sort, [DefaultValue(1)] int page)
        {
            ServiceProvider provider = _serviceProviderRepo.GetServiceProviderById(id);

            IQueryable<ServiceViewModel> servicesList = _serviceRepo.GetActiveServiceViewModelByUserId(provider.UserId);

            if (string.IsNullOrWhiteSpace(sort.Column))
                sort.Column = "ServiceId";

            if (!string.IsNullOrWhiteSpace(ServiceName))
                servicesList = servicesList.Where(c => c.Name.Contains(ServiceName));

            if (!string.IsNullOrWhiteSpace(ServiceContent))
                servicesList = servicesList.Where(c => c.Content.Contains(ServiceContent));

            if (CategoryId != null)
                servicesList = servicesList.Where(c => c.CategoryId == CategoryId);

            //utworzenie modelu do filtrowania
            ServiceFilterViewModel serviceFilterViewModel = new ServiceFilterViewModel();
            serviceFilterViewModel.SelectedCategoryId = CategoryId ?? -1;
            serviceFilterViewModel.Fill();

            //stronicowanie i sortowanie uslug
            IPagination<ServiceViewModel> servicesPagedList = servicesList.OrderBy(sort.Column, sort.Direction).AsPagination(page, 10);

            ServiceListContainerViewModel serviceListContainerViewModel = new ServiceListContainerViewModel
            {
                GridSortOptions = sort,
                ServiceFilterViewModel = serviceFilterViewModel,
                ServicePagedList = servicesPagedList
            };

            //utworzenie zbiorczego kontenera dla uslugodawcy i dla uslug
            ServiceProviderServicesListViewModel serviceProviderServicesListViewModel = new ServiceProviderServicesListViewModel
            {
                ServiceProvider = provider,
                Services = serviceListContainerViewModel
            };

            return View(serviceProviderServicesListViewModel);
        }
	}
}
