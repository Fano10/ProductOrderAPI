using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApiMagazin.Model
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string email { get; set; } = null!;
        [Required]
        public string country { get; set; } = null!;
        [Required]
        public string address { get; set; } = null!;
        [Required]
        public string city { get; set; } = null!;
        [Required]
        public string province { get; set; } = null!;
        [Required]
        public string postal_code { get; set; } = null!;
    }
}
