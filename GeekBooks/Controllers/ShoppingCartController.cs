﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeekBooks.Controllers
{
    public class  
        ShoppingCartController : Controller
    {
        private BookContext _context;

        public ShoppingCartController()
        {
            _context = new BookContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: ShoppingCart
        public ActionResult Index()//String username)
        {
            //Init the cart list
           // var cart = Session["cart"] as List<ShoppingCart> ?? new List<ShoppingCart>();
            //Ch/
            var carts = from sc in _context.ShoppingCarts
                        where sc.Username == "guest"
                        select sc;

            return View(carts);
        }
        public ActionResult CartPartial()
        {
            //Initialize CartVM
            ShoppingCart model = new ShoppingCart();
            // Initilize qty
            decimal qty = 0;
            //Initialize price 
            decimal price = 0m;

            //Check for cart session
            if (Session["cart"] != null)
            {
                //get total qty and price
                var list = (List<ShoppingCart>)Session["cart"];

                foreach (var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity * item.PriceEach;

                }
            }
            else
            {
                //Or set qty and price to 0
                model.Quantity = 0;
                model.PriceEach = 0m;
            }

            //Return partial view with model
             return View(model);
            //return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { model.Username });
        }
        public ActionResult AddToCartPartial(string isbn, string username) {

            //Init ShoppingCart View Model list

            List<ShoppingCart> cart = Session["cart"] as List<ShoppingCart> ?? new List<ShoppingCart>();

            //Initialize ShoppingCart view Model
            ShoppingCart model = new ShoppingCart();

            using (BookContext db = new BookContext())
            {
                //Get Book
                Book book = db.Books.Find(isbn);
                //Check if the book is already in the cart
                var bookInCart = cart.FirstOrDefault(x => x.ISBN == isbn );
                var userInCart = cart.FirstOrDefault(u => u.Username == username);
                //If not, Add new ShoppingCart View Model
                if (bookInCart == null)
                {
                    cart.Add(new ShoppingCart()
                    {
                        ISBN = book.ISBN,
                        Username = username,
                        PriceEach = book.Price,
                        Quantity = 1

                    });
                }
                else
                {
                    //If it is, increment
                    bookInCart.Quantity++;
                }
            }
            //Get Total qty and price and add to model
            short qty = 0;
            decimal price = 0m;

            foreach (var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * item.PriceEach;
            }

            model.Quantity = qty;
            model.PriceEach = price;

            //Save cart back to session
            Session["cart"] = cart;

                return PartialView(model);
        }

        //all working good with the functions below
        public ActionResult AddBookToShoppingCart(ShoppingCart shoppingCart)
        {
            //Check if user is logged in
            if (Session["Username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ShoppingCart sCart = _context.ShoppingCarts.Find(shoppingCart.Username, shoppingCart.ISBN);



            if (sCart == null)
            {

                _context.ShoppingCarts.Add(shoppingCart);
                _context.SaveChanges();
            }

            //return RedirectToAction("DisplayShoppingCartDetail", "ShoppingCart", new { shoppingCart.Username });
            return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { shoppingCart.Username });
        }
        // oh my god y just noticed it another thing is
        

        //[Route("ShoppingCart/DisplayShoppingCartDetail/{username}")]

        public ActionResult DisplayShoppingCartDetail(string username)
        {
            IEnumerable<ShoppingCart> sCart = _context.ShoppingCarts.Where(w => w.Username == username).ToList();

            if (sCart == null)
                return HttpNotFound();

            return View(sCart);
        }
        //[Route("ShoppingCart/ShoppingCartDetail/{username}")]

        public ActionResult ShoppingCartDetail(string username)
        {
            IEnumerable<ShoppingCart> sCart = _context.ShoppingCarts.Where(w => w.Username == username ).ToList();

            if (sCart == null)
                return HttpNotFound();

            return View(sCart);
        }

        //[Route("ShoppingCart/DeleteShoppingCartBook/{username}/{isbn}")]
        public ActionResult DeleteShoppingCartBook(string username, string isbn)
        {
            var sCart = _context.ShoppingCarts.Find(username, isbn);

            if (sCart == null)
                return HttpNotFound();

            _context.ShoppingCarts.Remove(sCart);
            _context.SaveChanges();

            return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { username });
        }
        [Route("ShoppingCart/UpdateShoppingCartQuantity/{Username}/{Isbn}")]
        public ActionResult UpdateShoppingCartQuantity(string Username, string Isbn)
        {
            ShoppingCart sCart = _context.ShoppingCarts.Find(Username, Isbn);

            return View(sCart);
        }
        public ActionResult SaveShoppingCartQuantity(ShoppingCart id)
        {
            var oldShoppingCart = _context.ShoppingCarts.Find(id.Username, id.ISBN);

            _context.ShoppingCarts.Remove(oldShoppingCart);
            _context.ShoppingCarts.Add(id);
            _context.SaveChanges();

            return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { id.Username });
        }
        //Update save for later
        [Route("ShoppingCart/UpdateShoppingCartSaveForLater/{Username}/{Isbn}")]
        public ActionResult UpdateShoppingCartSaveForLater(string Username, string Isbn)
        {
            ShoppingCart sCart = _context.ShoppingCarts.Find(Username, Isbn);
            //sCart.SaveForLater = true;
            return View(sCart);
        }
 
        public ActionResult SaveForLater(ShoppingCart id)
        {
            var oldShoppingCart = _context.ShoppingCarts.Find(id.Username, id.ISBN);

            _context.ShoppingCarts.Remove(oldShoppingCart);
            id.SaveForLater = true;
            _context.ShoppingCarts.Add(id);
            _context.SaveChanges();

            return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { id.Username });
        }

        public ActionResult BackToShoppingCart(ShoppingCart id)
        {
            var oldShoppingCart = _context.ShoppingCarts.Find(id.Username, id.ISBN);

            _context.ShoppingCarts.Remove(oldShoppingCart);
            id.SaveForLater = false;
            _context.ShoppingCarts.Add(id);
            _context.SaveChanges();

            return RedirectToAction("ShoppingCartDetail", "ShoppingCart", new { id.Username });
        }
    }
}