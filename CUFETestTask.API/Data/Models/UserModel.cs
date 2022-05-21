using System.ComponentModel.DataAnnotations;

namespace CUFETestTask.API.Data.Models
{
    public class UserModel
    {
        [Key]

        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string FatherName { get; set; }
        [Required]
        public string FamilyName { get; set; }
        [Required]
        public string Occupation { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string BirthDate { get; set; }



    }
}
