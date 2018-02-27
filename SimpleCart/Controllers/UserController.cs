using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SimpleCart.Models;

namespace SimpleCart.Controllers
{
    public class UserController : Controller
    {

        [HttpGet("/User/Form")]
        public ActionResult Form()
        {
          return View();
        }

        [HttpPost("/User/Create")]
        public ActionResult Create()
        {
            string name = Request.Form["nameForm"];
            string username = Request.Form["usernameForm"];
            string password = Request.Form["passwordForm"];
            string address = Request.Form["addressForm"];
            string email = Request.Form["emailForm"];
            AppUser newUser = new AppUser(name, username, password, address, email);
            newUser.Save();

            return RedirectToAction("Display", "Item");
        }

        [HttpPost("/User/Login")]
        public ActionResult LoginAction()
        {
            string login = Request.Form["usernameLogin"];
            string password = Request.Form["passwordLogin"];
            int sessionId = AppUser.Login(login, password);
            if (sessionId == -1)
            {
              return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Display", "Item", new {id=sessionId});
        }
    }
}
