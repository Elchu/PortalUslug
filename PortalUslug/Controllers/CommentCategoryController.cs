using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PortalUslug.Models;
using PortalUslug.Repositories;

namespace PortalUslug.Controllers
{
    public class CommentCategoryController : Controller
    {

        private readonly CategoryRepository _categoryRepository;

        public CommentCategoryController()
        {
            _categoryRepository = new CategoryRepository();
        }

        public ActionResult Index()
        {
            IQueryable<CommentCategory> commentCategoryList = _categoryRepository.GetAllCommentCategories();
            return View(commentCategoryList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentCategoryId,Name")] CommentCategory commentCategory)
        {
            if (ModelState.IsValid)
                try
                {
                    _categoryRepository.Add(commentCategory);
                    _categoryRepository.SaveChanges();

                    TempData["Message"] = "Pomyślnie dodano kategorię";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["Error"] = "Wystąpił błąd podczas dodawania kategorii!";
                    return View(commentCategory);
                }

            TempData["Error"] = "Wprowadź poprawnie dane!";
            return View(commentCategory);
        }

        public ActionResult Edit(int id = 0)
        {
            CommentCategory category = _categoryRepository.GetCommentCategoryById(id);

            if (category != null)
                return View(category);

            TempData["Error"] = "Brak kategorii o podanym ID!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentCategoryId,Name")] CommentCategory commentCategory)
        {
            if (ModelState.IsValid)
                try
                {
                    _categoryRepository.Edit(commentCategory);
                    _categoryRepository.SaveChanges();

                    TempData["Message"] = "Pomyślnie zmodyfikowano kategorię!";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["Error"] = "Wystąpił błąd podczas modyfikowania kategorii!";
                    return View(commentCategory);
                }

            TempData["Error"] = "Wprowadź poprawnie modyfikowane dane!";
            return View(commentCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _categoryRepository.Dispose();

            base.Dispose(disposing);
        }
    }
}
