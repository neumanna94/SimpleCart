using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SimpleCart.Models;

namespace SimpleCart.Controllers
{
    public class HomeController : Controller
    {
      [HttpGet("/User/Form/Register")]
      public ActionResult RegisterGET()
      {
          return View("Register");
      }
      [HttpPost("/User/Form/Register")]
      public ActionResult RegisterPOST()
      {
          string name = Request.Form["name"];
          string username = Request.Form["username"];
          string password = Request.Form["password"];
          string address = Request.Form["address"];
          string number = Request.Form["email"];

          return View("Register");
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
          return View("Login");
      }
      [HttpGet("/Item")]
      public ActionResult AllItemsGET()
      {
          return View("AllItems");
      }
      // [HttpPost("/User/Login")]
      // public ActionResult AllItemsPOST()
      // {
      //     string username = Request.Form["username"];
      //     string password = Request.Form["password"];
      //     return View("Login");
      // }
      [HttpGet("/User/Cart")]
      public ActionResult UserCartGET()
      {
          return View("UserCart");
      }
      [HttpPost("/User/Cart")]
      public ActionResult UserCartPOST()
      {
          return View("UserCart");
      }


    }
}