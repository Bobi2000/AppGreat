using AppGreat.Data;
using Microsoft.AspNetCore.Mvc;

namespace AppGreat.Controllers
{
    public class BaseController : Controller
    {
        protected readonly AppGreatDbContext Context;

        public BaseController(AppGreatDbContext context)
        {
            this.Context = context;
        }
    }
}
