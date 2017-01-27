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
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _categoryRepository;
     
        public CategoryController()
        {
            _categoryRepository = new CategoryRepository();
        }
        
        public ActionResult Index()
        {
            var categoryList = _categoryRepository.GetAllCategories();
            
            return View(categoryList);
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,Name")] Category category)
        {
            if (ModelState.IsValid)
                try
                {
                    _categoryRepository.Add(category);
                    _categoryRepository.SaveChanges();

                    TempData["Message"]  = "Pomyślnie dodano kategorię";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["Error"] = "Wystąpił błąd podczas dodawania kategorii!";
                    return View(category);
                }

            TempData["Error"] = "Wprowadź poprawnie dane!";
            return View(category);
        }

        public ActionResult Edit(int id = 0)
        {
            Category category = _categoryRepository.GetCategoryById(id);

            if (category != null)
                return View(category);

            TempData["Error"] = "Brak kategorii o podanym ID!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,Name")] Category category)
        {
            if (ModelState.IsValid)
                try
                {
                    _categoryRepository.Edit(category);
                    _categoryRepository.SaveChanges();

                    TempData["Message"]  = "Pomyślnie zmodyfikowano kategorię!";
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["Error"] = "Wystąpił błąd podczas modyfikowania kategorii!";
                    return View(category);
                }

            TempData["Error"] = "Wprowadź poprawnie modyfikowane dane!";
            return View(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _categoryRepository.Dispose();

            base.Dispose(disposing);
        }
    }
}
