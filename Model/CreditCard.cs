using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApiMagazin.Model
{
    public class CreditCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string name { get; set; } = null!;
        [Required]
        public string number { get; set; } = null!;
        [Required]
        public int expiration_year { get; set; }
        [Required]
        public int expiration_month { get; set; }
        [Required]
        public string cvv { get; set; } = null!;
    }
}
