using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApiMagazin.Model
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id {  get; set; }
        [Required]
        public int id_product {  get; set; }
        public int? id_client { get; set; }
        public int? id_credit_card {  get; set; }
        public int? id_transaction {  get; set; }
        [Required]
        public int quantity { get; set; }
        [Required]
        public int total_price {  get; set; } 
        [Required]
        public bool paid { get; set; }
        public int? shipping_price {  get; set; }

    }
}
