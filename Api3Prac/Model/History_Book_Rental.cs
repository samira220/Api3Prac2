using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    public class History_Book_Rental
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Books")]
        public int Books_Id { get; set; }
        public Books Books { get; set; }

        [Required]
        [ForeignKey("Readers")]
        public int Reader_ID { get; set; }
        public Readers Readers { get; set; }

        public DateTime RentalDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public DateTime DueDate { get; set; }
    }
}
