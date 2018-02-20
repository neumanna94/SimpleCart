using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Inventory.Models;

namespace Inventory.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost("/AmazonInventory")]
        public ActionResult IndexPost()
        {

            return View("Results", Item.GetAll());
        }
        [HttpPost("/AddItem")]
        public ActionResult AddItem()
        {
            string name = Request.Form["name"];
            string stringCost = Request.Form["cost"];
            int cost = 0;
            if(stringCost != "")
            {
                cost = Int32.Parse(stringCost);
            }
            Item newItem = new Item(name, cost, "", 0);
            newItem.Save();
            return View("Index");
        }
    }
}
