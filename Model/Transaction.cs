using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApiMagazin.Model
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id {  get; set; }
        [Required]
        public string idString { get; set; } = null!;
        [Required]
        public bool success { get; set; }
        [Required]
        public int amount_charged {  get; set; }
    }
}
