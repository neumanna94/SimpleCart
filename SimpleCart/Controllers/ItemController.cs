using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SimpleCart.Models;

namespace SimpleCart.Controllers
{
    public class ItemController : Controller
    {
        [HttpGet("/Item/{id}")]
        public ActionResult Display(int id)
        {
            ViewBag.sessionId = id;
            if (id != -1)
            {
              Cart myCart = new Cart(id);
              int myUserId = myCart.GetUserId();
              AppUser myUser = AppUser.Find(myUserId);
              Console.WriteLine(myUser.GetName());
              ViewBag.myUserName = myUser.GetName();
              return View("AllItems", Item.GetAll("id"));
            }
            return View("AllItems", Item.GetAll("id"));
        }

        [HttpPost("/Item")]
        public ActionResult DisplaySort()
        {
            string inputOrderBy = Request.Form["orderBy"];

            return View("AllItems", Item.GetAll(inputOrderBy));
        }

        [HttpGet("/Item/Detail/{itemId}/{sessionId}")]
        public ActionResult Detail(int itemId, int sessionId)
        {
            Item myItem = Item.Find(itemId);
            ViewBag.sessionId = sessionId;

            Cart myCart = new Cart(sessionId);
            int myUserId = myCart.GetUserId();
            AppUser myUser = AppUser.Find(myUserId);
            Console.WriteLine(myUser.GetName());
            ViewBag.myUserName = myUser.GetName();

            return View("ItemDetail", myItem);
        }
    }
}
