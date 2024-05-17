using Microsoft.AspNetCore.Mvc;

namespace BanDoWeb.Components
{
    public class Featured: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
