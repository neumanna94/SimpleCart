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
            User newUser = new User(name, username, password, address, email);
            newUser.Save();

            return RedirectToAction("Display", "Item");
        }
        [HttpGet("/User/Login")]
        public ActionResult LoginGET()
        {
            return View("Login");
        }

        [HttpPost("/User/Login")]
        public ActionResult LoginPOST()
        {
            string username = Request.Form["username"];
            string password = Request.Form["password"];

            return RedirectToAction("AllItemsGET", "Item");
        }

        [HttpGet("/User/Cart")]
        public ActionResult UserCartGET()
        {

            return View("Cart");
        }

    }
}
