using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiMagazin.Model
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public bool In_stock { get; set; }
        [Required]
        public float Price {  get; set; }
        [Required]
        public int Weight {  get; set; }  
        public string? Image {  get; set; }  
    }
}
