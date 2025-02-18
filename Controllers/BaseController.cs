using Microsoft.AspNetCore.Mvc;

namespace Clubs.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected int? PlayerId
        {
            get
            {
                return HttpContext.Items["Player-ID"] as int?;
            }
        }
    }
}
