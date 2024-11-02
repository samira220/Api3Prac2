using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class Readers
    {
        [Key]
        public int Id_Readers { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ContactInfo { get; set; }

        public DateTime RegistrationDate { get; set; }

    }
}
