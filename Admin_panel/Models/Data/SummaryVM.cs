namespace Admin_panel.Models.Data
{
    public class SummaryVM
    {
        public IEnumerable<CartProduct> listCart { get; set; }
        public CartProduct Carts { get; set; }
        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
        public IEnumerable<Product> listProducts { get; set;}
        public Order Order { get; set; }
    }
}
