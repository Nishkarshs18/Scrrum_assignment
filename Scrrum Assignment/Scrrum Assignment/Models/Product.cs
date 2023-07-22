using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Scrrum_Assignment.Models
{
    public class Product
    {
        [ValidateNever]
        public int Id { get; set; }
        public string ProductTitle { get; set; }
        [ValidateNever]
        public string ImageURL { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
    }
}
