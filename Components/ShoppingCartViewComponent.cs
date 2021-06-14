using KezyMart.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace KezyMart.Components
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IShoppingCartRepository _shoppingCartRepo;

        public ShoppingCartViewComponent(IShoppingCartRepository shoppingCartRepo)
        {
            _shoppingCartRepo = shoppingCartRepo;
        }
        public IViewComponentResult Invoke()
        {
            var result = _shoppingCartRepo.GetCartItemCount();

            return View(result);
        }
    }
}
