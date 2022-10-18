using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

//[Route("api/[controller]s")]
public abstract class BaseController : Controller
{
    public BaseController()
    {
    }
}