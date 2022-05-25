using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCart.Data
{
    public class SmallCartViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            SmallCartViewModel viewModel;

            if (cart==null||cart.Count==0)
            {
                viewModel = null;
            }
            else
            {
                viewModel = new SmallCartViewModel
                {
                    NumberOfItem = cart.Sum(x => x.Quantity),
                    TotalAmount=cart.Sum(x => x.Quantity * x.Price)
                 };
            }
            return View(viewModel);
        }
    }
}
