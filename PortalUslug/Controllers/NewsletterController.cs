using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalUslug.Helpers;
using PortalUslug.Models;
using PortalUslug.Repositories;

namespace PortalUslug.Controllers
{
    [Authorize(Roles="administrator")]
    public class NewsletterController : Controller
    {

        private readonly ServiceProviderRepository _servisProviderRepo;
        private readonly CustomerRepository _customerRepo;

        public NewsletterController()
        {
            _servisProviderRepo = new ServiceProviderRepository();
            _customerRepo = new CustomerRepository();
        }
        public ActionResult Send()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Send(Newsletter newsletter)
        {
            if (ModelState.IsValid)
            {
                var serviceProviderEmails = _servisProviderRepo.GetAllServiceProviderWhitNewsletter().Select(e => e.Email).ToList();
                var customerEmails = _customerRepo.GetCustomerWhitNewsletter().Select(e => e.Email).ToList();

                customerEmails.AddRange(serviceProviderEmails);

                if (customerEmails.Any())
                {
                    MailHelper.SendEmail(customerEmails, newsletter.Subject, newsletter.Content);
                    TempData["Message"] = "Wiadomość została rozesłana!";
                    return RedirectToAction("Index", "Home");
                }

                TempData["Error"] = "Brak aktywnych użytkowników. Newsletter nie został wysłany!";
                return Send();
            }

            TempData["Error"] = "Wypełnij poprawnie Newsletter!";
            return View(newsletter);
        }
    }
}