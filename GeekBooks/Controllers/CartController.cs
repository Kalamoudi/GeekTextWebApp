﻿using GeekBooks.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeekBooks.Controllers
{
    public class CartController : Controller
    {
        BookContext db = new BookContext();
        // GET: Cart
        public ActionResult Index()
        {

            //Init the cart list
            var cart = Session["cart"] as List<CartVM> ?? new List<CartVM>();
            //Check if cart is empty
            if (cart.Count == 0 || Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View();
            }

            //Calculate total and save to Viewbag
            decimal total = 0m;
            foreach (var item in cart)
            {
                total += item.Total;
            }

            ViewBag.GrandTotal = total;
            //Return the view with list
            return View(cart);
        }

        public ActionResult CartPartial()
        {
            //Initialize CartVM
            CartVM model = new CartVM();
            // Initilize qty
            decimal qty = 0;
            //Initialize price 
            decimal price = 0m;

            //Check for cart session
            if (Session["cart"] != null)
            {
                //get total qty and price
                var list = (List<CartVM>)Session["cart"];

                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.Price;

                }
            }
            else
            {
                //Or set qty and price to 0
                model.Quantity = 0;
                model.Price = 0m;
            }

            //Return partial view with model
            return PartialView(model);
        }
    }
}
