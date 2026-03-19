using System.ComponentModel.DataAnnotations;

namespace MovieQuotesAPI.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }


    }
}
