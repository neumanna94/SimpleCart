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
        public ActionResult AllItemsGET()
        {

            return View("AllItems", Item.GetAll("id"));
        }

        [HttpGet("/Item/Detail/{id}")]
        public ActionResult Detail(int id)
        {
            Item myItem = Item.Find(id);
            return View("ItemDetail", myItem);
        }
    }
}
