using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{

    public class User : IdentityUser
    {
        public string Name { get; set; }

        [DataType(DataType.Time)]
        public DateTime Date
        {
            get
            {
                return _date.AddHours(3);
            }
            set
            {
                _date = value;
            }
        }
        [NotMapped]
        private DateTime _date;
    }



    public class UserIdentityName : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        public UserIdentityName(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        public async Task<IViewComponentResult> InvokeAsync(string UserName)
        {
            var result = await _userManager.FindByNameAsync(UserName);
            if (result.Name == null)
            {
                return new HtmlContentViewComponentResult(new HtmlString("No Name"));
            }
            return new HtmlContentViewComponentResult(new HtmlString(result.Name));
        }
    }
}
