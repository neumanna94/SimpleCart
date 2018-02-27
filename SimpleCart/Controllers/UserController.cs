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

        [HttpGet("/User/Form/{sessionId}")]
        public ActionResult Form(int sessionId)
        {
          ViewBag.sessionId = sessionId; 
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
            if (newUser.Save())
            {
              string login = username;
              int sessionId = AppUser.Login(login, password);
              return RedirectToAction("Display", "Item", new {id=sessionId});
            }
            return RedirectToAction("Form", new {id=-1});
        }

        [HttpGet("/User/Login")]
        public ActionResult Login()
        {
          ViewBag.sessionId = -1;
          return View();
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

        [HttpGet("/User/Logout/{sessionId}")]
        public ActionResult Logout(int sessionId)
        {
          AppUser.Logout(sessionId);
          ViewBag.sessionId = -1;
          return RedirectToAction("Index", "Home");
        }
    }
}
