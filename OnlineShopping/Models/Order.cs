using System.ComponentModel.DataAnnotations;

namespace OnlineShopping.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Display(Name="Order No")]
        public string OrderNo { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name="Phone Number")]
        public string PhoneNo { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }

        public virtual List<OrderDetail> OrderDetails {get;set;}
    }
}
