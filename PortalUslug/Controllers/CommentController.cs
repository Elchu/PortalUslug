using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PortalUslug.Models;
using PortalUslug.Repositories;

namespace PortalUslug.Controllers
{
    public class CommentController : Controller
    {
        private CommentRepository _commentRepo;
        private CategoryRepository _categoryRepo;

        public CommentController()
        {
            _commentRepo = new CommentRepository();
            _categoryRepo = new CategoryRepository();
        }

        [Authorize]
        public ActionResult Create(int id)
        {
            ViewBag.CommentCategoryId = new SelectList(_categoryRepo.GetAllCommentCategories(), "CommentCategoryId", "Name");

            Comment comment = new Comment
            {
                ServiceId = id
            };

            return View(comment);
        }


        [HttpPost]
        [Authorize]
        public ActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.IPAddress = Request.UserHostAddress;
                comment.Date = DateTime.Now;
                comment.UserId = User.Identity.GetUserId();

                try
                {
                    _commentRepo.Add(comment);
                    _commentRepo.SaveChanges();

                    TempData["Message"] = "Komentarz został dodany!";
                    return RedirectToAction("Details", "Service", new {id = comment.ServiceId});
                }
                catch (Exception)
                {
                    TempData["Error"] = "Wystąpił błąd podczas zapisywania!";
                    return RedirectToAction("Details", "Service", new { id = comment.ServiceId });
                }
            }

            TempData["Error"] = "Uzupełnij poprawnie formularz!";
            return View(comment);
        }

        [Authorize(Roles="administrator")]
        public ActionResult Edit(int id)
        {
            Comment comment = _commentRepo.GetCommentById(id);
            if(comment != null)
                return View(comment);

            TempData["Error"] = "Nie ma takiego komentarza!";
            return RedirectToAction("Index", "Service");
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public ActionResult Edit(Comment comment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Comment editComment = _commentRepo.GetCommentById(comment.CommnetId);
                    editComment.Content = comment.Content;

                    _commentRepo.Edit(editComment);
                    _commentRepo.SaveChanges();

                    TempData["Message"] = "Komentarz został zmieniony!";
                    return RedirectToAction("Index", "Service");
                }
                catch (Exception)
                {
                    TempData["Error"] = "Wystąpił błąd podczas zapisywania komentarza!";
                    return RedirectToAction("Index", "Service");
                }
            }

            TempData["Error"] = "Uzupełnij poprawnie komentarz!";
            return RedirectToAction("Index", "Service");
        }

        [Authorize(Roles = "administrator")]
        public ActionResult Delete(int id)
        {
            Comment comment = _commentRepo.GetCommentById(id);

            if (comment != null)
            {
                try
                {
                    _commentRepo.Delete(comment);
                    _commentRepo.SaveChanges();

                    TempData["Message"] = "Komentarz został usunięty!";
                    return RedirectToAction("Index", "Service");
                }
                catch (Exception)
                {
                    TempData["Error"] = "Wystąpił błąd podczas usuwania komentarza!";
                    return RedirectToAction("Index", "Service");
                }
            }

            TempData["Error"] = "Podany komentarz nieistnieje!";
            return RedirectToAction("Index", "Service");
        }
    }
}
