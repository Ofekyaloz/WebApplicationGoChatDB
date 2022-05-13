using Microsoft.AspNetCore.Mvc;

namespace WebApplicationGoChat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class transfer : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("from,to,content")] string from, string to, string content)
        {

            return RedirectToAction(nameof(Index));
        }
    }

}
