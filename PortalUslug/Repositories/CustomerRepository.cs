using System.Data.Entity;
using PortalUslug.Models;
using System.Linq;
using System.Web.Security;
using PortalUslug.Models.View;
using PortalUslug.Repositories.IRepository;

namespace PortalUslug.Repositories
{
    /// <summary>
    /// Klasa repozytorium usługobiorcy.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        /// <summary>
        /// Obiekt klasy kontekstowej.
        /// </summary>
        private PortalUslugContext _db;

        /// <summary>
        /// Konstruktor repozytorium usługobiorcy.
        /// </summary>
        public CustomerRepository()
        {
            _db = new PortalUslugContext();
        }

        /// <summary>
        /// Pobranie usługobiorcy o podanym identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator usługobiorcy.</param>
        /// <returns>Usługobiorca o podanym identyfikatorze.</returns>
        public Customer GetCustomerById(int id)
        {
            return _db.Customers.Find(id);
        }

        /// <summary>
        /// Pobranie usługobiorcy o podanym identyfikatorze użytkownika.
        /// </summary>
        /// <param name="id">Identyfikator użytkownika.</param>
        /// <returns> Usługobiorca o podanym identyfikatorze użytkownika.</returns>
        public Customer GetCustomerByUserId(string id)
        {
            return _db.Customers.FirstOrDefault(customer => customer.UserId == id);
        }

        /// <summary>
        /// Pobranie usługobiorców z newsletterem.
        /// </summary>
        /// <returns>Usługobiorcy z newsletterem.</returns>
        public IQueryable<Customer> GetCustomerWhitNewsletter()
        {
            return _db.Customers.Where(u => u.Newsletter && u.IsConfirmed);
        }
        /// <summary>
        /// Sprawdzenie, czy usługobiorca o podanym identyfikatorze użytkownika jest usługobiorcą.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik o podanym identyfikatorze jest usługobiorcą.</returns>
        public bool IsCustomer(string userId)
        {
            //return _db.Customers.FirstOrDefault(u => u.UserId == userId) != null ? true : false;
            return _db.Customers.Any(u => u.UserId == userId);
        }

        /// <summary>
        /// Sprawdzenie, czy usługobiorca o podanym identyfikatorze użytkownika ma potwierdzone konto.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik o podanym identyfikatorze ma potwierdzone konto.</returns>
        public bool IsConfirmed(string userId)
        {
            return _db.Customers.FirstOrDefault(u => u.UserId == userId).IsConfirmed;
        }

        /// <summary>
        /// Dodanie usługobiorcy.
        /// </summary>
        /// <param name="customer">Dodawany usługobiorca.</param>
        public void Add(Customer customer)
        {
            _db.Customers.Add(customer);
        }

        /// <summary>
        /// Usunięcie usługobiorcy.
        /// </summary>
        /// <param name="customer">Usuwany usługobiorca.</param>
        public void Delete(Customer customer)
        {
            _db.Customers.Remove(customer);
            _db.SaveChanges();
        }
        /// <summary>
        /// Edytowanie Uslugobiorcy
        /// </summary>
        /// <param name="element">Edytowany Uslugobiorca</param>
        public void Edit(Customer element)
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
        /// Pobranie wszystkich uslugobiorcow
        /// </summary>
        /// <returns>Lista uslugobiorcow</returns>
        public IQueryable<CustomerViewModel> GetAllCustomer()
        {
            IQueryable<CustomerViewModel> customer = from c in _db.Customers
                select new CustomerViewModel
                {
                    CustomerId = c.CustomerId,
                    UserId = c.UserId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    City = c.City,
                    Street = c.Street,
                    ZipCode = c.ZipCode,
                    IsActive = c.IsConfirmed ? "Tak" : "Nie",
                    IsConfirmed = c.IsConfirmed,
                    RegistrationDate = c.RegistrationDate
                };

            return customer;
        }
    }
}