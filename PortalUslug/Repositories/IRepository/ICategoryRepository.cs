using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalUslug.Models;

namespace PortalUslug.Repositories.IRepository
{
    /// <summary>
    /// Interfejs repozytorium Kategorii uslug
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Pobranie wszystkich kategorii
        /// </summary>
        /// <returns>Lista Kategorii Uslug</returns>
        IQueryable<Category> GetAllCategories();

        /// <summary>
        /// Pobranie kategorii po identyfikatorze
        /// </summary>
        /// <param name="id">Identyfikator kategorii uslugi</param>
        /// <returns>Kategoria uslug</returns>
        Category GetCategoryById(int id);

        /// <summary>
        /// Pobranie wszystkich komentarzy w kategorii
        /// </summary>
        /// <returns>Lista Kategorii komentarzy</returns>
        IQueryable<CommentCategory> GetAllCommentCategories();

        /// <summary>
        /// Pobranie nazwy kategorii po podaniu identyfikatorze
        /// </summary>
        /// <param name="id">Identyfikator komentarza</param>
        /// <returns>Kategoria komentarza</returns>
        CommentCategory GetCommentCategoryById(int id);


    }
}
