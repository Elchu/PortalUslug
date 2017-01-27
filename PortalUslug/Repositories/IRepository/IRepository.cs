using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalUslug.Repositories.IRepository
{
    /// <summary>
    /// Interfejs bazowy repozytorium
    /// </summary>
    /// <typeparam name="T">Typ repozytorium</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Operacje dodawania
        /// </summary>
        /// <param name="element">Dodawany obiekt</param>
        void Add(T element);

        /// <summary>
        /// Operacje edycji
        /// </summary>
        /// <param name="element">Edytowany obiekt</param>
        void Edit(T element);

        /// <summary>
        /// Operacje usuwania
        /// </summary>
        /// <param name="element">Usuwany obiekt</param>
        void Delete(T element);

        /// <summary>
        /// Zapis zmian
        /// </summary>
        void SaveChanges();
    }
}