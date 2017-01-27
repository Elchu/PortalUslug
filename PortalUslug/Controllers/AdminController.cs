using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using PortalUslug.Models;


namespace WypozyczalniaSamochodow.Controllers
{
    public class AdminController : Controller
    {
        PortalUslugContext db = new PortalUslugContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        #region Autoryzacja

        public ActionResult ListaRol()
        {
            var roles = db.Roles.ToList();
            return View(roles);
        }

        public ActionResult UtworzRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UtworzRole(FormCollection collection)
        {
            try
            {
                db.Roles.Add(
                    new IdentityRole()//Microsoft.AspNet.Identity.EntityFramework.
                    {
                        Name = collection["RoleName"]
                    });
                db.SaveChanges();
                ViewBag.ResultMessage = "Rola utworzona pomyślnie";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EdytujRole(string RoleName)
        {

            var role = db.Roles.FirstOrDefault(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase));

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EdytujRole(IdentityRole role)
        {
            try
            {
                var isRole = db.Roles.FirstOrDefault(r => r.Name.Equals(role.Name, StringComparison.CurrentCultureIgnoreCase));
                if (isRole == null)
                {
                    db.Entry(role).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ListaRol");
                }
                else
                {
                    ViewBag.Blad = "Rola o takiej nazwie już istnieje";
                    return View(role);
                }
            }
            catch
            {
                return View();

            }
        }

        public ActionResult DodajRoleDoUzytkownika()
        {
            var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            var userList = db.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();

            ViewBag.Roles = list;
            ViewBag.Users = userList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DodajRoleDoUzytkownika(string UserName, string RoleName)
        {
            Uzytkownik user = (Uzytkownik)db.Users.FirstOrDefault(u => u.UserName.Equals(UserName, StringComparison.InvariantCultureIgnoreCase));
            var um = new UserManager<Uzytkownik>(new UserStore<Uzytkownik>(new PortalUslugContext()));

            um.AddToRole(user.Id, RoleName);

            var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Roles = list;
            ViewBag.Powterdzenie = "Rola została dodana";

            return RedirectToAction("ListaRol");
        }

        public ActionResult WyswietlRoleUzytkownika()
        {
            var userList = db.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WyswietlRoleUzytkownika(string UserName)
        {
            var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                           new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }
                           ).ToList();
            ViewBag.Roles = list;

            var userList = db.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();
            ViewBag.Users = userList;

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                Uzytkownik user = (Uzytkownik)db.Users.FirstOrDefault(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));
                var um = new UserManager<Uzytkownik>(new UserStore<Uzytkownik>
                (new PortalUslugContext()));

                ViewBag.RolesForThisUser = um.GetRoles(user.Id);
            }
            return View("WyswietlRoleUzytkownika");
        }

        public ActionResult UsunRoleUzytkownikowi()
        {
            var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            var userList = db.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();

            ViewBag.Roles = list;
            ViewBag.Users = userList;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UsunRoleUzytkownikowi(string UserName, string RoleName)
        {

            var um = new UserManager<Uzytkownik>(new UserStore<Uzytkownik>(new PortalUslugContext()));

            Uzytkownik user = (Uzytkownik)db.Users.FirstOrDefault(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));

            if (um.IsInRole(user.Id, RoleName))
            {
                um.RemoveFromRole(user.Id, RoleName);
                ViewBag.ResultSuccess = "Rola została usunięta!";
            }
            else
            {
                ViewBag.ResultWarning = "Ten użytkownik nie posiada takiej roli.";
            }

            // prepopulat roles for the view dropdown

            var list = db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Roles = list;

            return RedirectToAction("ListaRol");

        }
        #endregion //Autoryzacja
    }
}