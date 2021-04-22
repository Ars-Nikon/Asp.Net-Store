using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace ElectronicStore.Models
{
    public class CartViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private CartContext Cart;
        public CartViewComponent(UserManager<User> userManager,CartContext cartContext )
        {
            Cart = cartContext;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            var Count = Cart.Carts.Where(x => x.Id_User == user.Id).Count();
            return new HtmlContentViewComponentResult(new HtmlString(Count.ToString()));
        }
    }
}
