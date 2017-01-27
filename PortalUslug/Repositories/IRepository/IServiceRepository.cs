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
    /// Interfejs repozytorium Uslugi
    /// </summary>
    public interface IServiceRepository : IRepository<Service>
    {
        /// <summary>
        /// Pobranie uslugi o podanym identyfikatorze
        /// </summary>
        /// <param name="id">Identyfikator uslugi</param>
        /// <returns>Usluga o podanym identyfikatorze</returns>
        Service GetServiceById(int id);

        /// <summary>
        /// Sprawdzenie czy uzytkwonik dodal usluge
        /// </summary>
        /// <param name="id">Identyfikator uzytkownika</param>
        /// <returns>True, jesli uzytwkonik dodal usluge</returns>
        bool HasUserServices(string id);

        /// <summary>
        /// Pobiera wszystkie aktywne uslugi
        /// </summary>
        /// <returns>Wszystkie aktywne uslugi</returns>
        IQueryable<ServiceViewModel> GetAllActiveService();

        /// <summary>
        /// Pobiera wszystkie uslugi
        /// </summary>
        /// <returns>Wszystkie uslugi</returns>
        IQueryable<ServiceViewModel> GetAllService();

        /// <summary>
        /// Pobiera usluge o podanym ID
        /// </summary>
        /// <param name="id">ID uslugi</param>
        /// <returns>Zwraca usluge o podanym ID</returns>
        ServiceViewModel GetServiceViewModelById(int id);

        /// <summary>
        /// Pobiera uslugi dla wybranego UserId
        /// </summary>
        /// <param name="id">ID usera</param>
        /// <returns>Zwraca uslugi dla danego userId</returns>
        IQueryable<ServiceViewModel> GetActiveServiceViewModelByUserId(string id);
    }
}
