using System.ComponentModel.DataAnnotations;

namespace UnitTestApp.Models
{
    public class Phone
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Company { get; set; }
        public int Price { get; set; }
    }
}
