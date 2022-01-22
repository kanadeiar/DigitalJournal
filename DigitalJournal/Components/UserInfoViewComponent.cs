namespace DigitalJournal.Components;

[ViewComponent]
public class UserInfoViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        if (User.Identity?.IsAuthenticated == true)
            return View("UserInfo");
        return View();
    }
}

