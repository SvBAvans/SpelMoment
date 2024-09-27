using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SpelMoment.Controllers;

public class BaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        // Grab the culture route parameter
        string culture = filterContext.RouteData.Values["culture"]?.ToString()
                         ?? "nl";
        // Set the action parameter just in case we didn't get one
        // from the route.
        filterContext.ActionArguments["culture"] = culture;
        var cultureInfo = CultureInfo.GetCultureInfo(culture);
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        // Because we've overwritten the ActionParameters, we
        // make sure we provide the override to the
        // base implementation.
        base.OnActionExecuting(filterContext);
    }
}