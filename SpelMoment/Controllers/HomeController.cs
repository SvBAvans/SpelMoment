using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SpelMoment.Domain.Interfaces;
using SpelMoment.Domain.Model;
using SpelMoment.Models;

namespace SpelMoment.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMomentRepository _momentRepository;

    public HomeController(ILogger<HomeController> logger, IMomentRepository momentRepository)
    {
        _logger = logger;
        _momentRepository = momentRepository;
    }

    public IActionResult Index()
    {
        IEnumerable<Moment>? suggestedMoments = null;
        
        if (User.Identity != null && User.Identity.IsAuthenticated) {
            suggestedMoments = _momentRepository.GetAll().OrderBy(moment => moment.DateTime).Where(moment => moment.DateTime >  DateTime.Now);
        }

        return View(new HomeViewModel {SuggestedMoments = suggestedMoments});
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}