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
        [HttpGet("/Item")]
        public ActionResult Display()
        {
            ViewBag.sessionId = Request.Query["id"];
            return View("AllItems", Item.GetAll("id"));
        }

        [HttpPost("/Item")]
        public ActionResult DisplaySort()
        {
            string inputOrderBy = Request.Form["orderBy"];

            return View("AllItems", Item.GetAll(inputOrderBy));
        }

        [HttpGet("/Item/Detail/{id}")]
        public ActionResult Detail(int id)
        {
            Item myItem = Item.Find(id);
            return View("ItemDetail", myItem);
        }
    }
}
