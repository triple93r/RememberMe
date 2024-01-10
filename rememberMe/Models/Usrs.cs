using System.ComponentModel.DataAnnotations;

namespace rememberMe.Models
{
    public class Usrs
    {
        [Key]
        public int Id { get; set; }
        public string uName { get; set; }
        public string uPassword { get; set; }
        public bool RememberMe { get; set; }
    }
}
