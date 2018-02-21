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
        [HttpGet("/AmazonInventory")]
        public ActionResult AmazonInventory_default_Get()
        {
            return View("Results", Item.GetAll());
        }
        [HttpPost("/AmazonInventory")]
        public ActionResult AmazonInventory_default_post()
        {
            return View("Results", Item.GetAll());
        }
        [HttpPost("/AmazonInventory/orderAlpha")]
        public ActionResult AmazonInventory_Alphabetically()
        {
            Item.SetOrderBy("name");
            return RedirectToAction("AmazonInventory_default_Get");
        }
        [HttpPost("/AmazonInventory/orderCost")]
        public ActionResult AmazonInventory_ByCost()
        {
            Item.SetOrderBy("cost");
            return RedirectToAction("AmazonInventory_default_Get");
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
        [HttpGet("/items/{id}")]
        public ActionResult ItemDetail(int id)
        {
            return View(Item.Find(id));
        }
        [HttpGet("delete/{id}")]
        public ActionResult deleteItem(int id)
        {
            Item.DeleteRow(id);
            return RedirectToAction("/AmazonInventory");
        }
    }
}
