using System.ComponentModel.DataAnnotations;

namespace OnlineShopping.Models
{
    public class SpecialTag
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Special Tag")]
        public string Name { get; set; }
    }
}
