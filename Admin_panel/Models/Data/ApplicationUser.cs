using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin_panel.Models.Data
{
    public class ApplicationUser:IdentityUser

    {
        [Required(ErrorMessage =" Enter First Name")]
        [Column(TypeName ="Varchar(50)")]
        public string first_name {  get; set; }
        [Required(ErrorMessage = " Enter Last Name")]
        [Column(TypeName = "Varchar(50)")]
        public string last_name { get; set; }
        [Required(ErrorMessage ="Enter Town Name")]
        [Column(TypeName ="Varchar(50)")]
        public string? Town {  get; set; }
        [Required(ErrorMessage = "Enter City Name")]
        [Column(TypeName = "Varchar(50)")]
        public string? City { get; set; }
        [Required(ErrorMessage = "Enter Country Name")]
        [Column(TypeName = "Varchar(50)")]
        public string? Country { get; set; }
        [NotMapped]
        [EmailAddress]
        public string new_email { get; set; }

    }
}
