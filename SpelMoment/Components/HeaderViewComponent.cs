using Microsoft.AspNetCore.Mvc;

namespace SpelMoment.Components;

public class HeaderViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}