using Microsoft.AspNetCore.Mvc;

namespace SpelMoment.Components;

public class FooterViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}