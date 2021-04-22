using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicStore.Models
{


    public class Phone
    {
        private string _Name, _Screen_type, _Screen, _GPU, _CPU;

        public int Id { get; set; }

        [Required(ErrorMessage = "Name not specified")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value == null)
                {
                    _Name = null;
                }
                else
                    _Name = value.Trim();
            }
        }

        [Required(ErrorMessage = "Price not specified")]
        public int? Price { get; set; }

        public string Screen_type
        {
            get
            {
                return _Screen_type;
            }
            set
            {
                if (value == null)
                {
                    _Screen_type = null;

                }
                else
                    _Screen_type = value.Trim();
            }
        }
        public string Screen
        {
            get
            {
                return _Screen;
            }
            set
            {
                if (value == null)
                {
                    _Screen = null;
                }
                else
                    _Screen = value.Trim();
            }
        }

        public int? ROM { get; set; }




        public int? RAM { get; set; }

        public string GPU
        {
            get
            {
                return _GPU;
            }
            set
            {
                if (value == null)
                {
                    _GPU = null;
                }
                else
                    _GPU = value.Trim();
            }
        }
        public string CPU
        {
            get
            {
                return _CPU;
            }
            set
            {
                if (value == null)
                {
                    _CPU = null;
                }
                else
                    _CPU = value.Trim();
            }
        }

        [Required(ErrorMessage = "Quantity not specified")]
        public int Quantity { get; set; }

        public string Camera { get; set; }

        public byte[] Path_image
        {
            get
            {
                return _ImageByte;
            }
            set
            {
                _ImageByte = value;
            }
        }

        private byte[] _ImageByte;

        [Required(ErrorMessage = "Brands not specified")]
        [NotMapped]
        public string Brands { get; set; }

        [NotMapped]
        public string URL_Downloud_Image { get; set; }

        [NotMapped]
        public string UrlBack { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
        public int Id_Brands { get; set; }
    }
}
