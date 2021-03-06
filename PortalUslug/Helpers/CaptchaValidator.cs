﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PortalUslug.Models.View;

namespace PortalUslug.Helpers
{
    /// <summary>
    /// Filtr w postaci atrybutu CaptchaValidator
    /// </summary>
    public class CaptchaValidatorAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Metoda wykonywana przed uruchomieniem akcji.
        /// </summary>
        /// <param name="filterContext">Kontekst filtru.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Pobranie modelu RegisterModel z parametrów kontekstu filtra
            RegisterViewModel model = filterContext.ActionParameters["model"] as RegisterViewModel;

            // Sprawdzenie, czy wynik działania arytmetycznego wpisany przez użytkownika (i zapisany w modelu) jest równy wynikowi zapisanemu w sesji
            if (filterContext.HttpContext.Session["Captcha"] == null || filterContext.HttpContext.Session["Captcha"].ToString() != model.Captcha)
            {
                // Dodanie nowego parametru akcji do kontekstu filtra
                // Wartość parametru zależy od tego, czy wynik działania wpisany przez użytkownika jest poprawny (taki sam jak wynik zapisany w sesji).
                filterContext.ActionParameters["captchaValid"] = false;
            }
            else
            {
                filterContext.ActionParameters["captchaValid"] = true;
            }

            base.OnActionExecuting(filterContext);
        }
    }

}