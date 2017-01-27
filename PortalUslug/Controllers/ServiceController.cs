using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using PortalUslug.Models;
using PortalUslug.Models.View;
using PortalUslug.Repositories;
using MvcContrib.UI.Grid;
using MvcContrib.Sorting;

namespace PortalUslug.Controllers
{
    public class ServiceController : Controller
    {
        /// <summary>
        /// liczba dni aktywnej uslugi
        /// </summary>
        private int daysNumber = 0;

        private ServiceRepository _serviceRepo;
        private CustomerRepository _customerRepo;
        private CategoryRepository _categoryRepo;
        private CommentRepository _commentRepo;
        private ServiceProviderRepository _serviceProviderRepo;

        public ServiceController()
        {
            _serviceRepo = new ServiceRepository();
            _customerRepo = new CustomerRepository();
            _categoryRepo = new CategoryRepository();
            _commentRepo = new CommentRepository();
            _serviceProviderRepo = new ServiceProviderRepository();

            if (!int.TryParse(ConfigurationManager.AppSettings["DaysNumber"], out daysNumber))
                daysNumber = 7;
        }
        public ActionResult Index(string ServiceName, string ServiceContent, int? ServiceProviderId, int? CategoryId, GridSortOptions sort, [DefaultValue(1)]int page)
        {
            IQueryable<ServiceViewModel> serviceList = _serviceRepo.GetAllActiveService();

            if (User.IsInRole("administrator"))
                serviceList = _serviceRepo.GetAllService();

            if (string.IsNullOrWhiteSpace(sort.Column))
                sort.Column = "ServiceId";

            if (ServiceProviderId.HasValue)
                serviceList = serviceList.Where(s => s.ServiceId == ServiceProviderId);

            if (CategoryId.HasValue)
                serviceList = serviceList.Where(s => s.CategoryId == CategoryId);

            if (!string.IsNullOrWhiteSpace(ServiceName))
                serviceList = serviceList.Where(s => s.Name.Contains(ServiceName));

            if (!string.IsNullOrWhiteSpace(ServiceContent))
                serviceList = serviceList.Where(s => s.Content.Contains(ServiceContent));

            ServiceFilterViewModel serviceFilterViewModel = new ServiceFilterViewModel();
            serviceFilterViewModel.SelectedCategoryId = CategoryId ?? -1;
            serviceFilterViewModel.SelectedServiceProviderId = ServiceProviderId ?? -1;
            serviceFilterViewModel.Fill();

            IPagination<ServiceViewModel> serivcePagedList = serviceList.OrderBy(sort.Column, sort.Direction).AsPagination(page, 5);

            ServiceListContainerViewModel serviceListContainer = new ServiceListContainerViewModel
            {
                ServicePagedList = serivcePagedList,
                ServiceFilterViewModel = serviceFilterViewModel,
                GridSortOptions = sort
            };

            return View(serviceListContainer);
        }

        [Authorize(Roles="serviceProvider")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_categoryRepo.GetAllCategories(), "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "serviceProvider")]
        public ActionResult Create(Service service, int? DaysNumber)
        {
            if (ModelState.IsValid)
            {
                service.PostedDate = DateTime.Now;
                service.UserId = User.Identity.GetUserId();
                service.IPAddress = Request.UserHostAddress;

                if (DaysNumber.HasValue)
                    service.ExpirationDate = service.PostedDate.AddDays(DaysNumber.Value);
                else
                    service.ExpirationDate = service.PostedDate.AddDays(daysNumber);

                _serviceRepo.Add(service);
                _serviceRepo.SaveChanges();
                TempData["Message"] = "Usługa została dodana";

                return RedirectToAction("Details", new { id = service.ServiceId });
            }

            TempData["Error"] = "Uzupełnij poprawnie formularz";
            ViewBag.CategoryId = new SelectList(_categoryRepo.GetAllCategories(), "CategoryId", "Name");
            
            return View(service);
        }

        [Authorize(Roles=("administrator, serviceProvider"))]
        public ActionResult Edit(int id = 0)
        {
            Service service = _serviceRepo.GetServiceById(id);
            ViewBag.CategoryId = new SelectList(_categoryRepo.GetAllCategories(), "CategoryId", "Name", service.CategoryId);

            if (service != null)
            {
                if (User.IsInRole("administrator") || User.Identity.GetUserId() == service.UserId)
                    return View(service);

                TempData["Error"] = "Nie masz uprawnień do edytowania usługi!";
                return RedirectToAction("Index");
            }
                
            TempData["Error"] = "Podana usługa nieistnieje!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = ("administrator, serviceProvider"))]
        public ActionResult Edit(Service service, int? DaysNumber)
        {
            if (ModelState.IsValid)
            {
                service.IPAddress = Request.UserHostAddress;

                if (DaysNumber.HasValue)
                    service.ExpirationDate = service.ExpirationDate.AddDays(DaysNumber.Value);
                try
                {
                    _serviceRepo.Edit(service);
                    _serviceRepo.SaveChanges();

                    TempData["Message"] = "Usługa została zmieniona";
                    return RedirectToAction("Details", new {id = service.ServiceId});
                }
                catch (Exception)
                {
                    TempData["Error"] = "Wystąpił błąd podczas zapisu!";
                }
            }
            else
                TempData["Error"] = "Uzupełnij poprawnie formularz";


            ViewBag.CategoryId = new SelectList(_categoryRepo.GetAllCategories(), "CategoryId", "Name", service.CategoryId);
            return View(service);
        }

        public ActionResult Details(int id, [DefaultValue(1)]int page)
        {
            bool isConfirmed = false;

            ServiceViewModel service = _serviceRepo.GetServiceViewModelById(id);
            IQueryable<CommentViewModel> comments = _commentRepo.GetCommentViewModelByServiceId(service.ServiceId);

            //sprawdzam czy uslugodawca ma potwierdzone konto jesli tak to bedzie mogl przegladac komentarze
            ServiceProvider serviceProvider = _serviceProviderRepo.GetServiceProviderByUserId(User.Identity.GetUserId());
            if (serviceProvider != null && serviceProvider.IsConfirmed)
                isConfirmed = true;
            //sprawdzam czy uslugobiorca ma potwierdzone konto jesli tak to bedzie mogl przegladac komentarze
            Customer customer = _customerRepo.GetCustomerByUserId(User.Identity.GetUserId());
            if (customer != null && customer.IsConfirmed)
                isConfirmed = true;

            IPagination<CommentViewModel> commentList = comments.OrderBy("Date", SortDirection.Ascending).AsPagination(page, 5);

            ServiceCommentsViewModel serviceCommentsViewModel = new ServiceCommentsViewModel
            {
                Service = service,
                CommentPagedList = commentList,
                ConfirmedUser = isConfirmed
            };

            return View(serviceCommentsViewModel);
        }

        [Authorize(Roles = ("administrator, serviceProvider"))]
        public ActionResult Delete(int id = 0)
        {
            Service service = _serviceRepo.GetServiceById(id);

            if (service != null)
            {
                if (service.Comments.Count == 0)
                {
                    if (User.Identity.GetUserId() == service.UserId || User.IsInRole("administrator"))
                    {
                        return View(service);
                    }
                    TempData["Error"] = "Nie posiadasz uprawnień aby usunąć usługę!";
                    return RedirectToAction("Index");
                }
                TempData["Error"] = "Wybrana usługa nie może zostać usunięta ponieważ posiada komentarze!";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Podana usługa nie została znaleziona!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = ("administrator, serviceProvider"))]
        public ActionResult Delete(int id, FormCollection collection )
        {
            Service service = _serviceRepo.GetServiceById(id);
            try
            {
                _serviceRepo.Delete(service);
                _serviceRepo.SaveChanges();

                TempData["Error"] = "Usługa została usunięta!";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["Error"] = "Wystąpił błąd podczas usuwania usługi!";
                return RedirectToAction("Index");
            }
        }
    }
}
