using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    public interface IGenresController
    {
        Task<IActionResult> GetAllGenres();
        Task<IActionResult> AddGenre(Genres_Books newGenre);
        Task<IActionResult> UpdateGenre(int id, Genres_Books updatedGenre);
        Task<IActionResult> DeleteGenre(int id);
    }
}
