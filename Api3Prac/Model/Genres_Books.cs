using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class Genres_Books
    {
        [Key]
        public int Id_Genres_Books { get; set; }
        public string Name { get; set; }

    }
}
