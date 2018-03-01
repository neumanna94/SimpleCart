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
        [HttpGet("/Item/{sessionId}")]
        public ActionResult Display(int sessionId)
        {
            string inputOrderBy = "id";
            ViewBag.sessionId = sessionId;
            ViewBag.tags = Tag.GetAll();
            if(!string.IsNullOrEmpty(Request.Query["orderBy"]))
            {
                inputOrderBy = Request.Query["orderBy"];
            }
            if (sessionId != -1)
            {
                Cart myCart = new Cart(sessionId);
                int myUserId = myCart.GetUserId();
                AppUser myUser = AppUser.Find(myUserId);
                Console.WriteLine(myUser.GetName());
                ViewBag.myUserName = myUser.GetName();
                return View("AllItems", Item.GetAll(inputOrderBy));
            }

            return View("AllItems", Item.GetAll(inputOrderBy));
        }

        [HttpGet("/Item/SortByTag/{sessionId}")]
        public ActionResult SortByTag(int sessionId)
        {
            ViewBag.sessionId = sessionId;
            List<Tag> myTags = new List<Tag>(){Tag.Find(Int32.Parse(Request.Query["tagId"]))};
            Console.WriteLine("Number of tags: " + myTags.Count);
            List<Item> myItems = Item.GetAllByTags(myTags);
            Console.WriteLine(myItems[0].GetName());
            ViewBag.tags = Tag.GetAll();
            return View("AllItems", Item.GetAllByTags(myTags));
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

            List<Tag> myTags = myItem.GetTags();
            Console.WriteLine(myTags.Count); 
            ViewBag.relatedItems = Item.GetAllByTags(myTags);


            return View("ItemDetail", myItem);
        }
    }
}
