using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortalUslug.Models;
using PortalUslug.Models.View;

namespace PortalUslug.Repositories.IRepository
{
    /// <summary>
    /// Interfejs repozytorium Uslugodawca
    /// </summary>
    public interface IServiceProviderRepository : IRepository<ServiceProvider>
    {
        /// <summary>
        /// Pobranie uslugodawcy po podanym identyfikatorze
        /// </summary>
        /// <param name="id">Identyfikator uslugodawcy</param>
        /// <returns>uslugodawca o podanym identyfikatorze</returns>
        ServiceProvider GetServiceProviderById (int id);

        /// <summary>
        /// Pobranie uslugodawcy po podanym identyfikatorze uzytkownika
        /// </summary>
        /// <param name="id">Identyfikator uzytkownika</param>
        /// <returns>uslugodawca o podanym identyfikatorze</returns>
        ServiceProvider GetServiceProviderByUserId(string id);

        /// <summary>
        /// Pobranie uslugodawcy z potwierdzonym kontem
        /// </summary>
        /// <returns>Uslugodawcy z potwierdzonym kontem</returns>
        IQueryable<ServiceProvider> GetAllServiceProviderIsConfirmed();

        /// <summary>
        /// Pobiera wszystkich usługodawców
        /// </summary>
        /// <returns>Usługodawcy</returns>
        IQueryable<ServiceProviderViewModel> GetAllServiceProvider();

        /// <summary>
        /// Pobiera uslugodawcy z Newsletterem
        /// </summary>
        /// <returns>uslugodawcy Newslettera</returns>
        IQueryable<ServiceProvider> GetAllServiceProviderWhitNewsletter();

        /// <summary>
        /// Sprawdzenie czy podany uzytkownik jest uslugodawca
        /// </summary>
        /// <param name="id">Identyfikator uzytkownika</param>
        /// <returns>True, jesli uzytkownik jest uslugodawca</returns>
        bool IsServiceProvider(string id);

        /// <summary>
        /// Sprawdzenie czy uslugodawca o podanym identyfikatorze uzytkownika ma potwierdzone konto
        /// </summary>
        /// <param name="id">Identyfkator uzytkownika</param>
        /// <returns>True, jesli uslugodawca posiada potwierdzone konto</returns>
        bool IsConfirmed(string id);
    }
}