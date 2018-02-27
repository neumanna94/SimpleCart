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
      ViewBag.sessionId = id;

      AppUser myUser = AppUser.Find(myCart.GetUserId());
      ViewBag.myUserName = myUser.GetName();
      
      return View(myItems);
    }

    [HttpGet("/Cart/Update/{itemId}/{sessionId}")]
    public ActionResult Update(int itemId, int sessionId)
    {
      Cart myCart = new Cart(sessionId);
      myCart.AddItem(itemId);
      return RedirectToAction("Display", new {id = sessionId});
    }
  }
}
