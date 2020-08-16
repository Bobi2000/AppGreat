using AppGreat.Data;
using AppGreat.Models;
using AppGreat.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace AppGreat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        private IConfiguration Config;

        public UsersController(AppGreatDbContext context, IConfiguration config)
            : base(context)
        {
            this.Config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel userCred)
        {
            var login = new User() { Username = userCred.Username, Password = userCred.Password };

            IActionResult respone = NotFound();

            var user = AuthenticateUser(login);

            if (user != null)
            {
                var authenticationService = new AuthenticationService();
                var tokenStr = authenticationService.GenerateJSONWebToken(user, this.Config);
                respone = Ok(new { token = tokenStr });
            }

            return respone;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterModel userCred)
        {
            var passwordHasher = new PasswordService();

            var newUser = new User() { Username = userCred.Username, Password = passwordHasher.HashPassword(userCred.Password), CurrencyCode = userCred.CurrencyCode };

            if (this.Context.Users.Where(u => u.Username == userCred.Username).FirstOrDefault() != null)
            {
                return BadRequest();
            }

            this.Context.Users.Add(newUser);
            await this.Context.SaveChangesAsync();

            return newUser;
        }

        private User AuthenticateUser(User login)
        {
            var passwordHasher = new PasswordService();

            var users = this.Context.Users.ToList();

            var currentUser = users
                .Where(u => u.Username == login.Username)
                .FirstOrDefault();

            if (passwordHasher.VerifyPassword(currentUser.Password, login.Password))
            {
                return currentUser;
            }

            return null;
        }
    }
}
