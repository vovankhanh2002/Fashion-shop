using Microsoft.AspNetCore.Mvc;

namespace BanDoWeb.Components
{
    public class Categories:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
