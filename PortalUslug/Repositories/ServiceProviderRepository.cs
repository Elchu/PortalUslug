using System.Data.Entity;
using PortalUslug.Models;
using System.Linq;
using System.Web.Security;
using PortalUslug.Repositories.IRepository;
using PortalUslug.Models.View;

namespace PortalUslug.Repositories
{
    /// <summary>
    ///  Klasa repozytorium usługodawcy.
    /// </summary>
    public class ServiceProviderRepository : IServiceProviderRepository
    {
        /// <summary>
        /// Obiekt klasy kontekstowej.
        /// </summary>
        private PortalUslugContext _db;

        /// <summary>
        /// Konstruktor repozytorium usługodawcy.
        /// </summary>
        public ServiceProviderRepository()
        {
            _db = new PortalUslugContext();
        }

        /// <summary>
        /// Pobranie usługodawcy o podanym identyfikatorze usługodawcy.
        /// </summary>
        /// <param name="id">Identyfikator usługodawcy.</param>
        /// <returns>Usługodawca o podanym identyfikatorze usługodawcy.</returns>
        public ServiceProvider GetServiceProviderById(int id)
        {
            return _db.ServiceProviders.FirstOrDefault(provider => provider.ServiceProviderId == id);
        }

        /// <summary>
        /// Pobranie usługodawcy o podanym identyfikatorze użytkownika.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <returns>Usługodawca o podanym identyfikatorze użytkownika.</returns>
        public ServiceProvider GetServiceProviderByUserId(string userId)
        {
            return _db.ServiceProviders.FirstOrDefault(provider => provider.UserId == userId);
        }

        /// <summary>
        /// Pobieranie usługodawców z potwierdzonym kontem.
        /// </summary>
        /// <returns>Usługodawcy z potwierdzonym kontem.</returns>
        public IQueryable<ServiceProvider> GetAllServiceProviderIsConfirmed()
        {
            return _db.ServiceProviders.Where(u => u.IsConfirmed);
        }

        /// <summary>
        /// Pobieranie usługodawców z newsletterem.
        /// </summary>
        /// <returns>Usługodawcy z newsletterem.</returns>
        public IQueryable<ServiceProvider> GetAllServiceProviderWhitNewsletter()
        {
            return _db.ServiceProviders.Where(u => u.Newsletter && u.IsConfirmed);
        }

        /// <summary>
        /// Sprawdzenie, czy użytkownik o podanym identyfikatorze jest usługodawcą.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik o podanym identyfikatorze jest usługodawcą.</returns>
        public bool IsServiceProvider(string userId)
        {
            return _db.ServiceProviders.Any(u => u.UserId == userId);
        }

        /// <summary>
        /// Sprawdzenie, czy użytkownik o podanym identyfikatorze ma potwierdzone konto.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik o podanym identyfikatorze ma potwierdzone konto.</returns>
        public bool IsConfirmed(string userId)
        {
            return _db.ServiceProviders.FirstOrDefault(u => u.UserId == userId).IsConfirmed;
        }

        /// <summary>
        /// Dodanie usługodawcy.
        /// </summary>
        /// <param name="provider">Dodawany usługodawca.</param>
        public void Add(ServiceProvider provider)
        {
            _db.ServiceProviders.Add(provider);
        }

        /// <summary>
        /// Usunięcie usługodawcy.
        /// </summary>
        /// <param name="provider">Usuwany usługodawca.</param>
        public void Delete(ServiceProvider provider)
        {
            _db.ServiceProviders.Remove(provider);
        }

        /// <summary>
        /// Edytuj Uslugodawce
        /// </summary>
        public void Edit(ServiceProvider element)
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
        /// Pobieranie wszystkich usługodawców.
        /// </summary>
        /// <returns>Wszyscy uslugodawcy</returns>
        public IQueryable<ServiceProviderViewModel> GetAllServiceProvider()
        {
            IQueryable<ServiceProviderViewModel> providers =
                 from s in _db.ServiceProviders
                    select new ServiceProviderViewModel
                    {
                        ServiceProviderId = s.ServiceProviderId,
                        UserId = s.UserId,
                        Name = s.Name,
                        City = s.City,
                        Street = s.Street,
                        ZipCode = s.ZipCode,
                        PhoneNumber = s.PhoneNumber,
                        IsActive = s.IsConfirmed ? "Tak" : "Nie",
                        IsConfirmed = s.IsConfirmed,
                        RegistrationDate = s.RegistrationDate
                    };
            return providers;
        }
    }
}