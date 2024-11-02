using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    public interface IReadersController
    {
        Task<IActionResult> GetAllReaders();
        Task<IActionResult> GetReaderById(int id);
        Task<IActionResult> AddReader(Readers newReader);
        Task<IActionResult> UpdateReader(int id, Readers updatedReader);
        Task<IActionResult> DeleteReader(int id);
        Task<IActionResult> FilterReaders(DateTime? registeredAfter);
    }
}
