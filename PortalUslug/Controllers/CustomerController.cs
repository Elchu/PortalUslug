using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib.Pagination;
using MvcContrib.UI.Grid;
using MvcContrib.Sorting;
using PortalUslug.Models;
using PortalUslug.Models.View;
using PortalUslug.Repositories;
using Microsoft.AspNet.Identity;

namespace PortalUslug.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly CustomerRepository _customerRepo ;
        private readonly CommentRepository _commentRepo;

        public CustomerController()
        {
           _customerRepo = new CustomerRepository();
           _commentRepo = new CommentRepository();
        }

        [Authorize(Roles="administrator")]
        public ActionResult Index(string FirstName, string LastName, string City, string Street, GridSortOptions sort, [DefaultValue(1)]int page)
        {
            IQueryable<CustomerViewModel> customerList = _customerRepo.GetAllCustomer();

            if (string.IsNullOrWhiteSpace(sort.Column))
                sort.Column = "CustomerId";

            if (!string.IsNullOrWhiteSpace(FirstName))
                customerList = customerList.Where(c => c.FirstName.Contains(FirstName));
            
            if (!string.IsNullOrWhiteSpace(LastName))
                customerList = customerList.Where(c => c.LastName.Contains(LastName));
            
            if (!string.IsNullOrWhiteSpace(City))
                customerList = customerList.Where(c => c.City.Contains(City));
            
            if (!string.IsNullOrWhiteSpace(Street))
                customerList = customerList.Where(c => c.Street.Contains(Street));

            //potrzebna przestrzen nazw using MvcContrib.Sorting;
            var customerPagedList = customerList.OrderBy(sort.Column, sort.Direction).AsPagination(page, 5);

            CustomerFilterViewModel customerFilterViewModel = new CustomerFilterViewModel();

            CustomerListContainerViewModel customerListContainerViewModel = new CustomerListContainerViewModel
            {
                CustomerPageList = customerPagedList,
                CustomerFilterViewModel = customerFilterViewModel,
                GridSortOptions = sort
            };

            return View(customerListContainerViewModel);
        }

        [Authorize(Roles = "customer")]
        public ActionResult Create()
        {
            Customer customer = _customerRepo.GetCustomerByUserId(User.Identity.GetUserId());
            //jesli uzytkownik ma juz uzupelniony profil to przekierowujemy do edycji
            if (customer != null)
                return RedirectToAction("Edit", new { id = customer.CustomerId });

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "customer")]
        public ActionResult Create(Customer customer)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    customer.RegistrationDate = DateTime.Now;
                    customer.UserId = User.Identity.GetUserId();
                    _customerRepo.Add(customer);
                    _customerRepo.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["Error"] = "Uzupełnij poprawnie formularz!";
                }

                TempData["Message"] = "Profil usługobiorcy został poprawnie dodany";
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        [Authorize(Roles="administrator, customer")]
        public ActionResult Edit(int id)
        {
            Customer customer = _customerRepo.GetCustomerById(id);
            
            //using Microsoft.AspNet.Identity;
            if (customer.UserId == User.Identity.GetUserId())
                return View(customer);

            if (User.IsInRole("administrator"))
                return View(customer);

            TempData["Error"] = "Nie masz uprawnień do edytowania tego usługobiorcy!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles="administrator, customer")]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _customerRepo.Edit(customer);
                    _customerRepo.SaveChanges();

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
            return View(customer);
        }

        [Authorize(Roles="administrator")]
        public ActionResult Delete(int id)
        {
            Customer customer = _customerRepo.GetCustomerById(id);
            if (customer != null)
            {
                bool customerComments = _commentRepo.HasUserComment(customer.UserId);

                if (!customerComments)
                    return View(customer);

                TempData["Error"] = "Nie można usunąć usługobiorcy ponieważ posiada komentarze!";          
            }
            else
                TempData["Error"] = "Niestaty usługobiorca o podanym ID nieistnieje!";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                Customer customer = _customerRepo.GetCustomerById(id);
                _customerRepo.Delete(customer);

                TempData["Message"] = "Usługobiorca został usunięty!";
            }
            catch
            {
                TempData["Error"] = "Wystąpił błąd podczas usuwania usługobiorcy. Spróbuj później!";
                return View("Delete", new { id = id });
            }

            return RedirectToAction("Index");
        }
    }
}
