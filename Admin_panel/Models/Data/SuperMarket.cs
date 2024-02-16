using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin_panel.Models.Data
{
    public class SuperMarket
    {
        [Key]
        public int sp_id {  get; set; }
        [Required(ErrorMessage ="Enter Super Market Name")]
        [Column(TypeName ="Varchar(50)")]
        public string sp_name { get; set;}
        [Required(ErrorMessage = "Enter Super Market Town")]
        [Column(TypeName = "Varchar(50)")]
        public string sp_town { get; set;}
    }
}
