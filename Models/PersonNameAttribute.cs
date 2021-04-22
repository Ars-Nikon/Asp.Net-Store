using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{
    public class PersonNameAttribute : ValidationAttribute
    {
      
        public override bool IsValid(object value)
        {
            
            if (value == null)
            {
                return false;
            }
            string UserName = value.ToString();

            if (String.IsNullOrEmpty(UserName.Trim()))
                return false;

            string userNamePattern = @"^[a-zA-Zа-яА-Я]+$";

            if (Regex.IsMatch(UserName, userNamePattern))
                return true;

            return false;


        }

    }

}

