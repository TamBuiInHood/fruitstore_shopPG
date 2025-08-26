using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controller
{
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}
