using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        [PersonName(ErrorMessage = "Name must be in Latin or Cyrilic")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        public string Password { get; set; }

    }
}
