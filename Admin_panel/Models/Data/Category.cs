using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin_panel.Models.Data
{
    public class Category
    {
        [Key]
        public int cat_id { get; set; }
        [Required(ErrorMessage ="Enter Category Name")]
        [Column(TypeName="Varchar(50)")]
        public string cat_name {  get; set; }
        public int cat_status {  get; set; }
        [NotMapped]
        public int p_count {  get; set; }
    }
}
