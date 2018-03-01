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
    [HttpGet("/Cart/Display/{sessionId}")]
    public ActionResult Display(int sessionId)
    {
      Cart myCart = new Cart(sessionId);
      List<Item> myItems = myCart.GetItems();
      ViewBag.sessionId = sessionId;

      AppUser myUser = AppUser.Find(myCart.GetUserId());
      ViewBag.myUserName = myUser.GetName();

      return View(myItems);
    }

    [HttpGet("/Cart/Update/{itemId}/{sessionId}")]
    public ActionResult Update(int itemId, int sessionId)
    {
      Cart myCart = new Cart(sessionId);
      myCart.AddItem(itemId);
      return RedirectToAction("Display", new {sessionId = sessionId});
    }

    [HttpGet("/Cart/DeleteItem/{itemId}/{sessionId}")]
    public ActionResult DeleteItem(int itemId, int sessionId)
    {
      Cart myCart = new Cart(sessionId);
      myCart.DeleteItem(itemId);
      return RedirectToAction("Display", new {sessionId = sessionId});
    }
    [HttpGet("/Cart/Confirm/{sessionId}")]
    public ActionResult Confirm(int sessionId)
    {
      Cart myCart = new Cart(sessionId);
      AppUser myUser = AppUser.Find(myCart.GetUserId());
      ViewBag.myUserName = myUser.GetName();
      ViewBag.sessionId = sessionId;
      ViewBag.totalCost = myCart.GetTotalCost();
      return View(myCart.GetItems());
    }
    [HttpGet("/Cart/Checkout/{sessionId}/{totalCost}")]
    public ActionResult Checkout(int sessionId,double totalCost)
    {
      Cart myCart = new Cart(sessionId);
      List<Item> myItems = myCart.GetItems();
      ViewBag.totalCost = totalCost;
      ViewBag.sessionId = sessionId;

      AppUser myUser = AppUser.Find(myCart.GetUserId());
      ViewBag.myUserName = myUser.GetName();
      //"Show them something different."
      return View(myItems);
    }

  }
}
