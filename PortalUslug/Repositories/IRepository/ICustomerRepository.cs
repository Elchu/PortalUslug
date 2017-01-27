using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalUslug.Models;
using PortalUslug.Models.View;

namespace PortalUslug.Repositories.IRepository
{
    /// <summary>
    /// Interfejs repozytorium Uslugobiorca
    /// </summary>
    public interface ICustomerRepository : IRepository<Customer>
    {
        /// <summary>
        /// Pobranie uslugobiorcy po podanym identyfikatorze
        /// </summary>
        /// <param name="id">Identyfikator uslugobiorcy</param>
        /// <returns>Uslugobiorca o podanym identyfikatorze</returns>
        Customer GetCustomerById(int id);

        /// <summary>
        /// Pobranie uslugobiorcy po podanym identyfikatorze uzytkownika
        /// </summary>
        /// <param name="id">Identyfikator uzytkownika</param>
        /// <returns>Uslugobiorce o podanym identyfikatorze</returns>
        Customer GetCustomerByUserId(string id);

        /// <summary>
        /// Pobiera uslugobiorcow z Newsletterem
        /// </summary>
        /// <returns>Uslugobiorcy Newslettera</returns>
        IQueryable<Customer> GetCustomerWhitNewsletter();

        /// <summary>
        /// Pobranie wszystkich usługobiorców
        /// </summary>
        /// <returns>Usługobiorcy</returns>
        IQueryable<CustomerViewModel> GetAllCustomer();

        /// <summary>
        /// Sprawdzenie czy podany uzytkownik jest uslugobiorca
        /// </summary>
        /// <param name="id">Identyfikator uzytkownika</param>
        /// <returns>True, jesli uzytkownik jest uslugobiorca</returns>
        bool IsCustomer(string id);

        /// <summary>
        /// Sprawdzenie czy uslugobiorca o podanym identyfikatorze uzytkownika ma potwierdzone konto
        /// </summary>
        /// <param name="id">Identyfkator uzytkownika</param>
        /// <returns>True, jesli uslugobiorca posiada potwierdzone konto</returns>
        bool IsConfirmed(string id);
    }
}
