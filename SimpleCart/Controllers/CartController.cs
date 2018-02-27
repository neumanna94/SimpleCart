using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SimpleCart.Models;

namespace SimpleCart.Controllers
{
  public class CartController : Controller
  {
    [HttpGet("/Cart/Display/{id}")]
    public ActionResult Display(int id)
    {
      Cart myCart = new Cart(id);
      List<Item> myItems = myCart.GetItems();
      ViewBag.sessionId = Request.Query["id"];
      return View(myItems);
    }

    [HttpGet("/Cart/Update/{itemId}/{id}")]
    public ActionResult Update(int itemId, int userId)
    {
      Cart myCart = new Cart(userId);
      myCart.AddItem(itemId);
      return RedirectToAction("Display", new {id = userId});
    }
  }
}
