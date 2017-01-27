using System;
using System.Data.Entity;
using System.Linq;
using PortalUslug.Models;
using PortalUslug.Models.View;
using PortalUslug.Repositories.IRepository;

namespace PortalUslug.Repositories
{
    /// <summary>
    /// Klasa repozytorium komentarza.
    /// </summary>
    public class CommentRepository : ICommentRepository
    {
        /// <summary>
        /// Obiekt klasy kontekstowej.
        /// </summary>
        private PortalUslugContext _db;

        /// <summary>
        /// Konstruktor repozytorium komentarza.
        /// </summary>
        public CommentRepository()
        {
            _db = new PortalUslugContext();
        }

        /// <summary>
        /// Pobranie komentarza o podanym identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator komentarza.</param>
        /// <returns>Komentarz o podanym identyfikatorze.</returns>
        public Comment GetCommentById(int id)
        {
            return _db.Comments.FirstOrDefault(comment => comment.CommnetId == id);
        }

        /// <summary>
        /// Sprawdzenie, czy użytkownik o podanym identyfikatorze dodał komentarz/-e.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <returns>True, jeśli użytkownik dodał komentarz/-e.</returns>
        public bool HasUserComment(string userId)
        {
            return _db.Comments.Any(comment => comment.UserId == userId);
        }

        /// <summary>
        /// Dodanie komentarza.
        /// </summary>
        /// <param name="comment">Dodawany komentarz.</param>
        public void Add(Comment comment)
        {
            _db.Comments.Add(comment);
        }

        /// <summary>
        /// Usunięcie komentarza.
        /// </summary>
        /// <param name="comment">Usuwany komentarz.</param>
        public void Delete(Comment comment)
        {
            _db.Comments.Remove(comment);
        }

        /// <summary>
        /// Edytowanie komentarza
        /// </summary>
        /// <param name="element">Komentarz do edytowania</param>
        public void Edit(Comment element)
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
        /// Pobiera wszystkie komentarze dla wybranej uslugi
        /// </summary>
        /// <param name="id">ID wybranej uslugii</param>
        /// <returns>Komentarze dla wybranej uslugi</returns>
        public IQueryable<CommentViewModel> GetCommentViewModelByServiceId(int id)
        {
            IQueryable<CommentViewModel> comments = (from c in _db.Comments
                where c.ServiceId == id
                select new CommentViewModel
                {
                    CommentId = c.CommnetId,
                    Content = c.Content,
                    Date = c.Date,
                    IPAddress = c.IPAddress,
                    ServiceId = c.ServiceId,
                    ServiceName = c.Services.Name,
                    CommentCategoryId = c.CommentCategoryId,
                    CommentCategoryName = c.CommentCategory.Name,
                    UserId = c.UserId,
                    User = (_db.ServiceProviders.FirstOrDefault(u => u.UserId == c.UserId) != null) ? 
                        (_db.ServiceProviders.FirstOrDefault(u => u.UserId == c.UserId).Name) : 
                            (_db.Customers.FirstOrDefault(u => u.UserId == c.UserId) != null) ? 
                                (_db.Customers.FirstOrDefault(u => u.UserId == c.UserId).FirstName + " " + _db.Customers.FirstOrDefault(u => u.UserId == c.UserId).LastName) : 
                                    "Administrator"
                });

            return comments;
        }
    }
}