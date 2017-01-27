using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortalUslug.Models;
using PortalUslug.Models.View;

namespace PortalUslug.Repositories.IRepository
{
    /// <summary>
    /// Interfejs repozytorium Comentarzy
    /// </summary>
    public interface ICommentRepository : IRepository<Comment>
    {
        /// <summary>
        /// Pobranie komentarza po identyfikatorze
        /// </summary>
        /// <param name="id">Identyfikator komentarza</param>
        /// <returns>Komentarz o podanym identyfikatorze</returns>
        Comment GetCommentById(int id);

        /// <summary>
        /// Sprawdza czy uzytkownik napisal komentarz
        /// </summary>
        /// <param name="userId">Identyfikator uzytkownika</param>
        /// <returns>True, jesli uzytwkonik dodal komentarz</returns>
        bool HasUserComment(string userId);

        /// <summary>
        /// Pobiera wszystkie komentarze dla wybranej uslugi
        /// </summary>
        /// <param name="id">ID wybranej uslugii</param>
        /// <returns>Komentarze dla wybranej uslugi</returns>
        IQueryable<CommentViewModel> GetCommentViewModelByServiceId(int id);
    }
}