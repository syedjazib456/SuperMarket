using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin_panel.Models.Data
{
    public class Order
    {
        [Key]
        public int Order_id { get; set; }
        public string? user_id { get; set; }
        [ForeignKey("user_id")]
        [ValidateNever]
        public ApplicationUser app_user { get; set; }
        [Required]
        public DateTime? Order_date { get; set; }
        public DateTime? Shipping_date { get; set; }
        public double? Order_total { get; set; }
        public string? Order_status { get; set; }
        public string? payment_status { get; set; }
        public string? Tracking_number { get; set; }
        public string? Carrier { get; set; }
        public DateTime? Payment_date { get; set; }
        public DateTime? Paymentdue_date { get; set; }
        public string? Session_id { get; set; }
        public string? Payment_intentid { get; set; }
       
        [Required(ErrorMessage = " Enter First Name")]
        [Column(TypeName = "Varchar(50)")]
        public string? first_name { get; set; }
        [Required(ErrorMessage = " Enter Last Name")]
        [Column(TypeName = "Varchar(50)")]
        public string? last_name { get; set; }
        [Required(ErrorMessage = "Enter Town Name")]
        [Column(TypeName = "Varchar(50)")]
        public string? Town { get; set; }
        [Required(ErrorMessage = "Enter City Name")]
        [Column(TypeName = "Varchar(50)")]
        public string? City { get; set; }
        [Required(ErrorMessage = "Enter Address")]
        [Column(TypeName = "Varchar(max)")]
        public string? Address {  get; set; }
        [Required(ErrorMessage = "Enter Postal Code")]
        [Column(TypeName = "Varchar(50)")]
        public string? Pcode { get; set; }
        [Required(ErrorMessage = "Enter contact number")]
        [Column(TypeName = "Varchar(50)")]
        public string? Contact_No { get; set; }
        [Required(ErrorMessage = "Enter Country Name")]
        [Column(TypeName = "Varchar(50)")]
        public string? Country { get; set; }
        [ValidateNever]
        [Column(TypeName = "Varchar(max)")]
        public string Note {  get; set; }
        [NotMapped]
        public double st_rate { get; set; }



    }
}
