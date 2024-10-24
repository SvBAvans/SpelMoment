using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SpelMoment.Domain.Interfaces;

namespace SpelMoment.Components;

public class HeaderViewComponent : ViewComponent
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly IUserRepository userRepository;

    public HeaderViewComponent(UserManager<IdentityUser> userManager, IUserRepository userRepository)
    {
        this.userManager = userManager;
        this.userRepository = userRepository;
    }
    
    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var idUser = await userManager.FindByNameAsync(User.Identity.Name);
            if (idUser != null) {
                var user = userRepository.GetByIdentityId(idUser.Id);
                ViewBag.FirstName = user.FirstName;
            }
        }
        
        return View();
    }
}