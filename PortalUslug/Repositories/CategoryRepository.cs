using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PortalUslug.Models;
using PortalUslug.Repositories.IRepository;

namespace PortalUslug.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private PortalUslugContext _db;
        /// <summary>
        /// Konstruktor inicjalizujacy baze danych
        /// </summary>
        public CategoryRepository()
        {
            _db = new PortalUslugContext();
        }
        /// <summary>
        /// Pobranie wszystkich kategorii uslug
        /// </summary>
        /// <returns>Kategorie uslug</returns>
        public IQueryable<Category> GetAllCategories()
        {
            return _db.Categories;
        }
        /// <summary>
        /// Pobranie kategorii uslug dla podanego identyfikatora
        /// </summary>
        /// <param name="id">Identyfikator kategorii uslugi</param>
        /// <returns></returns>
        public Category GetCategoryById(int id)
        {
            return _db.Categories.Find(id);
        }
        /// <summary>
        /// Pobranie Kategorii komentarzy
        /// </summary>
        /// <returns>Kategorie komentarzy</returns>
        public IQueryable<CommentCategory> GetAllCommentCategories()
        {
            return _db.CommentCategories;
        }
        /// <summary>
        /// Pobranie kategorii komentarzy o podanym identyfikatorze
        /// </summary>
        /// <param name="id">Identyfikator kategorii komantarza</param>
        /// <returns></returns>
        public CommentCategory GetCommentCategoryById(int id)
        {
            return _db.CommentCategories.Find(id);
        }
        /// <summary>
        /// Dodanie kategorii uslug
        /// </summary>
        /// <param name="element">Kategoria uslugi do dodania</param>
        public void Add(Category element)
        {
            _db.Categories.Add(element);
        }
        /// <summary>
        /// Dodanie kategorii komentarzy
        /// </summary>
        /// <param name="element">Kategoria komentarza do dodania</param>
        public void Add(CommentCategory element)
        {
            _db.CommentCategories.Add(element);
        }
        /// <summary>
        /// Edytowanie kategorii uslug
        /// </summary>
        /// <param name="element">Kategoria uslug do edycji</param>
        public void Edit(Category element)
        {
            _db.Entry(element).State = EntityState.Modified;
        }
        /// <summary>
        /// Edytowanie kategorii komentarzy
        /// </summary>
        /// <param name="element">Kategoria komentarzy do edycji</param>
        public void Edit(CommentCategory element)
        {
            _db.Entry(element).State = EntityState.Modified;
        }
        /// <summary>
        /// Usuniecie kategorii uslug
        /// </summary>
        /// <param name="element">Kategoria uslug do usuniecia</param>
        public void Delete(Category element)
        {
            _db.Categories.Remove(element);
        }
        /// <summary>
        /// Usuniecie kategorii komentarzy
        /// </summary>
        /// <param name="element">Kategoria komentarzy do usuniecia</param>
        public void Delete(CommentCategory element)
        {
            _db.CommentCategories.Remove(element);
        }
        /// <summary>
        /// Zapisanie zmian
        /// </summary>
        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}