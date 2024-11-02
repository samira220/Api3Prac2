using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    public class Books
    {
        [Key]
        public int Id_Books { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        [Required]
        [ForeignKey("Genres_Books")] 
        public string Genre_ID {  get; set; }
        public Genres_Books Genres { get; set; }
        public int PublicationYear {  get; set; }
        public int AvailableCopies { get; set;}
        public DateTime DateAdded { get; set; }
        public DateTime? Year { get; set; }
    }
}
