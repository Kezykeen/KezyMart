using System.Text.Encodings.Web;
using KezyMart.Repositories;
using KezyMart.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KezyMart.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepo;
        private readonly IProductRepository _productRepo;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepo, IProductRepository productRepo)
        {
            _shoppingCartRepo = shoppingCartRepo;
            _productRepo = productRepo;
        }

        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View(new ShoppingCartViewModel
            {
                CartItems = _shoppingCartRepo.GetCart(this.HttpContext).GetCartItems(),
                CartTotal = _shoppingCartRepo.GetCart(this.HttpContext).GetTotal()
            });
        }

        public ActionResult AddToCart(int id)
        {
            var addedProduct = _productRepo.FindProduct(id);

            _shoppingCartRepo.GetCart(this.HttpContext).AddToCart(addedProduct);

            return View();
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var productName = _shoppingCartRepo.GetCartById(id).Product.Name;

            return Json(new ShoppingCartRemoveViewModel
            {
                Message = HtmlEncoder.Default.Encode(productName) +
                          " has been removed from your shopping cart.",
                CartTotal = _shoppingCartRepo.GetCart(this.HttpContext).GetTotal(),
                CartCount = _shoppingCartRepo.GetCart(this.HttpContext).GetCount(),
                ItemCount = _shoppingCartRepo.GetCart(this.HttpContext).RemoveFromCart(id),
                DeleteId = id
            });
        }
    }
}
