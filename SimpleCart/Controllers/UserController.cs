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
                return RedirectToAction("Display", "Item", new {sessionId=sessionId});
            }
            return RedirectToAction("Form", new {sessionId=-1});
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
            return RedirectToAction("Display", "Item", new {sessionId=sessionId});
        }

        [HttpGet("/User/Logout/{sessionId}")]
        public ActionResult Logout(int sessionId)
        {
            AppUser.Logout(sessionId);
            ViewBag.sessionId = -1;
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("/User/Display/{sessionId}")]
        public ActionResult Display(int sessionId)
        {
            ViewBag.sessionId = sessionId;
            Cart myCart = new Cart(sessionId);
            AppUser myUser = AppUser.Find(myCart.GetUserId());
            ViewBag.myUserName = myUser.GetName();

            return View(myUser);
        }
        [HttpPost("/User/Update/{sessionId}")]
        public ActionResult Update(int sessionId)
        {
            ViewBag.sessionId = sessionId;
            Cart myCart = new Cart(sessionId);
            AppUser myUser = AppUser.Find(myCart.GetUserId());
            ViewBag.myUserName = myUser.GetName();

            string name = Request.Form["nameInput"];
            string login = Request.Form["username"];
            string address = Request.Form["addressInput"];
            string email = Request.Form["emailInput"];

            AppUser.Update(name, login, address, email, myCart.GetUserId());
            myUser.SetId(myCart.GetUserId());
            return RedirectToAction("Display", new {id = sessionId});
        }


        [HttpGet("User/Forgot/{sessionId}")]
        public ActionResult Forgot(int sessionId)
        {
            ViewBag.sessionId = -1;
            return View();
        }

        [HttpPost("User/Forgot/Update")]
        public ActionResult ForgotAction()
        {
            string name = Request.Form["nameForgot"];
            string username = Request.Form["usernameForgot"];
            string email = Request.Form["emailForgot"];
            List<string> info = AppUser.Forgot(name, username, email);
            if (info.Count == 0)
            {
                return RedirectToAction("Forgot", new {sessionId = -1 });
            }
            string login = info[0];
            string password = info[1];
            int sessionId = AppUser.Login(login, password);
            return RedirectToAction("Display", "Item", new {sessionId=sessionId});
        }

    }
}
