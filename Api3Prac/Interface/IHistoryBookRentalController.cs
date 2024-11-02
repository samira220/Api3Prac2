using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    public interface IHistoryBookRentalController
    {
        Task<IActionResult> RentBook(History_Book_Rental newRental);
        Task<IActionResult> ReturnBook(int id);
        Task<IActionResult> GetRentalHistoryForReader(int readerId);
        Task<IActionResult> GetRentalHistoryForBook(int bookId);
        Task<IActionResult> GetCurrentRentals();
    }
}
