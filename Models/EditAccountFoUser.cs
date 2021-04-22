using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{
    public class EditAccountFoUser
    {
       
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        [PersonName(ErrorMessage = "Name must be in Latin or Cyrilic")]
        public string Name { get; set; }

        public string Password { get; set; }
    }
}
