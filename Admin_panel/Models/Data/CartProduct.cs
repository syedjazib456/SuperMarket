using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin_panel.Models.Data
{
    public class CartProduct
    {
        [Key]
        public int Cart_id { get; set; }

        public int Product_id { get; set; }
        [ForeignKey("Product_id")]
        public Product Productid { get; set;}
        public string users_id {  get; set; }
        [ForeignKey("users_id")]
        public ApplicationUser User { get; set; }
        [Range(1, 100, ErrorMessage ="Range between 1 to 100.")]
        public int Quantity {  get; set; }
        [NotMapped]
        public double cart_price {  get; set; }
        [NotMapped]
        public double cart_total {  get; set; }
        [NotMapped]
        public double total_quantity {  get; set; }
    }
}
