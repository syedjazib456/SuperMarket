using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin_panel.Models.Data
{
    public class Product
    {
        [Key]
        public int p_id { get; set; }
        [Required(ErrorMessage ="Enter Product Name")]
        [Column(TypeName ="Varchar(50)")]
        public string p_name { get; set;}
        [Column(TypeName ="Varchar(max)")]
        public string? p_description { get; set;}
        public double? p_mrp {  get; set; }
        [Required(ErrorMessage ="Enter Product Price")]
        public double p_price {  get; set; }
        [Required(ErrorMessage ="Select Product Stock")]
        [Column(TypeName ="Varchar(50)")]
        public string p_stock {  get; set; }
        [Required(ErrorMessage ="Select Product Category")]
        public int p_category {  get; set; }
        [ForeignKey("p_category")]
        public Category p_cat { get; set; }
        [Required(ErrorMessage ="Select Product Super Market")]
        public int p_supermart {  get; set; }
        [ForeignKey("p_supermart")]
        public SuperMarket p_spmart { get; set; }
        [Column(TypeName ="Varchar(max)")]
        public string p_img {  get; set; }
        [NotMapped]
        public IFormFile p_image { get; set; }
        [NotMapped]
        public IEnumerable<Category> categories { get; set; }
        [NotMapped]
        public IEnumerable<Product> products { get; set; }
        [NotMapped]
        public CartProduct p_cart { get; set; }
        [NotMapped]
        public Product p_product { get; set; }
        



    }
}
