using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCoreLibrary.Services;
using ProductSales.Models;

namespace ProductSales.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly string _secretKey = "h1vRWE7ZzhX95/i2k+XEupo3qlF3QxJo0Pys3vBY9d0=\r\n";// generate and add to config
        private readonly string _issuer = "issuer";
        private readonly string _audence = "audience";
        private readonly ProductCoreContext _context;

        public AuthController(ProductCoreContext context)
        {
            _context = context;
        }
        [HttpPost]
        [Route("login")]

        public IActionResult LoginAsync(string username,string password)
        {
            // hash the password and perform auth logic
            var user =  _context.Users.FirstOrDefault(x=>x.UserName==username);


            if (user != null)
            {
                if (IsValidUser(password, user))
                {

                    var jwtService = new JwtService(_secretKey, _issuer, _audence);
                    var token = jwtService.GenerateToken(user.UserId.ToString(), username);
                    return Ok(new { token });
                }
            }
            return Unauthorized();
        }

        private static bool IsValidUser( string password, User user)
        {
            return user.PasswordHash == password;
        }
    }
}
