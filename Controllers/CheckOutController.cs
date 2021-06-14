using Microsoft.AspNetCore.Mvc;
using System;
using KezyMart.Models;
using KezyMart.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace KezyMart.Controllers
{
    [Authorize]
    public class CheckOutController : Controller
    {
        private readonly ICheckOutRepository _checkOutRepo;
        private readonly IShoppingCartRepository _shoppingCartRepo;

        public CheckOutController (ICheckOutRepository checkOutRepo, IShoppingCartRepository shoppingCartRepo)
        {
            _checkOutRepo = checkOutRepo;
            _shoppingCartRepo = shoppingCartRepo;
        }

        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            bool isValid = _checkOutRepo.CheckOrderValidity(id);

            // Verify if the Customer owns the order
            if (isValid && _checkOutRepo.GetOrder(id).Username == User.Identity.Name)
            {
                return View(id);
            }

            return View("Error");
        }

        // GET: CheckOut
        public ActionResult AddressAndPayment()
        {
            var cart = _shoppingCartRepo.GetCart(HttpContext);

            return cart.GetCartItemCount() == 0 ? View("Error") : View();
        }

        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order {Username = User.Identity.Name, OrderDate = DateTime.Now};

            //Process the order
            var cart = _shoppingCartRepo.GetCart(HttpContext);
            cart.CreateOrder(order);

            return RedirectToAction("Complete", new {id = order.OrderId});
        }
    }
}
