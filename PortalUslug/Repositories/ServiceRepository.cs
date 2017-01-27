using System;
using System.Collections.Generic;
using System.Linq;
using PortalUslug.Models;
using PortalUslug.Models.View;
using PortalUslug.Repositories.IRepository;
using System.Data.Entity;

namespace PortalUslug.Repositories
{
    /// <summary>
    /// Klasa repozytorium usługi.
    /// </summary>
    public class ServiceRepository : IServiceRepository
    {
        /// <summary>
        /// Obiekt klasy kontekstowej.
        /// </summary>
        private PortalUslugContext _db;

        /// <summary>
        /// Konstruktor repozytorium usług.
        /// </summary>
        public ServiceRepository()
        {
            _db = new PortalUslugContext();
        }

        /// <summary>
        /// Pobranie usługi o podanym identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator usługi.</param>
        /// <returns>Usługa o podanym identyfikatorze.</returns>
        public Service GetServiceById(int id)
        {
            return _db.Services.FirstOrDefault(service => service.ServiceId == id);
        }

        /// <summary>
        /// Sprawdzenie, czy użytkownik o podanym identyfikatorze dodał usługę/-i.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik dodał usługę/-i.</returns>
        public bool HasUserServices(string userId)
        {
            return _db.Services.Any(u => u.UserId == userId);
        }

        /// <summary>
        /// Dodanie usługi.
        /// </summary>
        /// <param name="service">Dodawana usługa.</param>
        public void Add(Service service)
        {
            _db.Services.Add(service);
        }

        /// <summary>
        /// Usunięcie usługi.
        /// </summary>
        /// <param name="service">Usuwana usługa.</param>
        public void Delete(Service service)
        {
            _db.Services.Remove(service);
        }

        /// <summary>
        /// Edytuj Usluge
        /// </summary>
        public void Edit(Service element)
        {
            _db.Entry(element).State = EntityState.Modified;
        }

        /// <summary>
        /// Zapisanie zmian.
        /// </summary>
        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        /// <summary>
        /// Pobiera wszystkie aktywne uslugi
        /// </summary>
        /// <returns>Wszystkie aktywne uslugi</returns>
        public IQueryable<ServiceViewModel> GetAllActiveService()
        {
            IQueryable<ServiceViewModel> allService = from s in _db.Services
                                                      where s.ExpirationDate > DateTime.Now
                                                      orderby s.PostedDate descending
                                                      select new ServiceViewModel
                                                      {
                                                          IPAddress = s.IPAddress,
                                                          PostedDate = s.PostedDate,
                                                          ExpirationDate = s.ExpirationDate,
                                                          ServiceId = s.ServiceId,
                                                          CategoryId = s.CategoryId,
                                                          UserId = s.UserId,
                                                          Content = s.Content,
                                                          CategoryName = s.Category.Name,
                                                          Name = s.Name,
                                                          IsActive = s.ExpirationDate > DateTime.Now ? "Tak" : "Nie",
                                                          ServiceProvider = _db.ServiceProviders.FirstOrDefault(u => u.UserId == s.UserId).Name
                                                      };

            return allService;
        }
        /// <summary>
        /// Pobiera wszystkie uslugi
        /// </summary>
        /// <returns>Wszystkie uslugi</returns>
        public IQueryable<ServiceViewModel> GetAllService()
        {
            IQueryable<ServiceViewModel> allService = from s in _db.Services
                                                      orderby s.PostedDate descending
                                                      select new ServiceViewModel
                                                      {
                                                          IPAddress = s.IPAddress,
                                                          PostedDate = s.PostedDate,
                                                          ExpirationDate = s.ExpirationDate,
                                                          ServiceId = s.ServiceId,
                                                          CategoryId = s.CategoryId,
                                                          UserId = s.UserId,
                                                          Content = s.Content,
                                                          CategoryName = s.Category.Name,
                                                          Name = s.Name,
                                                          IsActive = s.ExpirationDate > DateTime.Now ? "Tak" : "Nie",
                                                          ServiceProvider = _db.ServiceProviders.FirstOrDefault(u => u.UserId == s.UserId).Name
                                                      };

            return allService;
        }

        /// <summary>
        /// Pobiera usluge o podanym ID
        /// </summary>
        /// <param name="id">ID uslugi</param>
        /// <returns>Zwraca usluge o podanym ID</returns>
        public ServiceViewModel GetServiceViewModelById(int id)
        {
            ServiceViewModel allService = (from s in _db.Services
                                                      where s.ServiceId == id
                                                      select new ServiceViewModel
                                                      {
                                                          IPAddress = s.IPAddress,
                                                          PostedDate = s.PostedDate,
                                                          ExpirationDate = s.ExpirationDate,
                                                          ServiceId = s.ServiceId,
                                                          CategoryId = s.CategoryId,
                                                          UserId = s.UserId,
                                                          Content = s.Content,
                                                          CategoryName = s.Category.Name,
                                                          Name = s.Name,
                                                          IsActive = s.ExpirationDate > DateTime.Now ? "Tak" : "Nie",
                                                          ServiceProvider = _db.ServiceProviders.FirstOrDefault(u => u.UserId == s.UserId).Name
                                                      }).FirstOrDefault();
            return allService;
        }


        /// <summary>
        /// Pobiera uslugi dla wybranego UserId
        /// </summary>
        /// <param name="id">UserID</param>
        /// <returns>Zwraca uslugi dla danego userId</returns>
        public IQueryable<ServiceViewModel> GetActiveServiceViewModelByUserId(string id)
        {
            IQueryable<ServiceViewModel> allService = (from s in _db.Services
                                                       where s.UserId == id && s.ExpirationDate > DateTime.Now
                                           select new ServiceViewModel
                                           {
                                               IPAddress = s.IPAddress,
                                               PostedDate = s.PostedDate,
                                               ExpirationDate = s.ExpirationDate,
                                               ServiceId = s.ServiceId,
                                               CategoryId = s.CategoryId,
                                               UserId = s.UserId,
                                               Content = s.Content,
                                               CategoryName = s.Category.Name,
                                               Name = s.Name,
                                               IsActive = s.ExpirationDate > DateTime.Now ? "Tak" : "Nie",
                                               ServiceProvider = _db.ServiceProviders.FirstOrDefault(u => u.UserId == s.UserId).Name
                                           });
            return allService;
        }
    }
}