using eBookStoreApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace eBookStoreApp.Pages.Book
{
    [Authorize(Roles = "admin")]
    public class AddModel : PageModel
    {
        [BindProperty]
        public BusinessObject.Models.Book Book { get; set; } = null!;
        public List<BusinessObject.Models.Publisher> Publishers { get; set; } = new();

        public async Task<IActionResult> OnGet()
        {
            var client = new ClientService(HttpContext);
            var publishers = await client.Get<OdataList<BusinessObject.Models.Publisher>>("/odata/publishers");
            if (publishers == null) return NotFound();

            Publishers = publishers.Value;
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            Book.PulishedDate = Book.PulishedDate.ToUniversalTime();
            var client = new ClientService(HttpContext);
            var book = await client.Post<BusinessObject.Models.Book>("odata/books", Book);
            if (book == null) return NotFound();

            return RedirectToPage("/Book/View", new { book.Id });
        }
    }
}
