using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin_panel.Models.Data
{
    public class OrderDetail
    {
        [Key]
       public int orderdetail_Id {  get; set; }
        [Required]
        public int order_id { get; set; }
        [ForeignKey("order_id")]
        public Order order { get; set; }
        [Required]
        public int product_id {  get; set; }
        [ForeignKey("product_id")]
        public Product product { get; set; }
        [Required]
        public int Count {  get; set; }
        [Required]
        public double price {  get; set; }
    }
}
