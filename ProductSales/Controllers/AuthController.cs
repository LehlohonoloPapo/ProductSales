using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProductCoreLibrary.Services;
using ProductCoreLibrary.Extensions;
using ProductSales.Models;
namespace ProductSales.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly string _secretKey;// generate and add to config
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IConfiguration _configuration;
        private readonly ProductCoreContext _context;

        public AuthController(ProductCoreContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _secretKey = Configurations.GetAppsettings(_configuration["Secrets:secretKey"]??"");
            _audience= Configurations.GetAppsettings(_configuration["Secrets:audience"] ?? "");
            _issuer= Configurations.GetAppsettings(_configuration["Secrets:issuer"] ?? "");
        }
     
        [HttpPost]
        [Route("login")]

        public async Task<IActionResult> LoginAsync(string username,string password)
        {
            // hash the password and perform auth logic
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserName == username);


            if (user != null && user.Role!=null)
            {
                if (IsValidUser(password, user))
                {

                    var jwtService = new JwtService(_secretKey, _issuer, _audience);
                    var token = jwtService.GenerateToken(user.UserId.ToString(), user.Role.RoleName, username);
                    return Ok(new { token });
                }
            }
            return Unauthorized();
        }

      
        [HttpPost]
        [Route("register")]
        // ReSharper disable once InvalidXmlDocComment
        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        public async Task<IActionResult> Register([FromBody] RegisterModel user)
        {

            var userRole = user.Role;
            if (userRole == Guid.Empty)// if there is no role assign a defualt User Role
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User");
                if (role != null)
                {
                    userRole = role.RoleId;
                }
                else
                {
                    //throw new Exception("No role available");
                    return BadRequest(new Exception("No role available"));
                }
            }

            if(!ModelState.IsValid || user.Password==null)
                return BadRequest();

            if(_context.Users.Any(u=>u.UserName== user.UserName))
            {
                return BadRequest("Username already exists");
            }

            //Create new user

            var newUser = new User
            {
                UserName = user.UserName,
                PasswordHash = Hashing.CreatePasswordHash(user.Password),
                UserId = Guid.NewGuid(),
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId= userRole
            };
            //add user to context
            _context.Users.Add(newUser);

            //Save changes
            await _context.SaveChangesAsync();

            return Ok("Registration Successful");
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            // Clear the user's authentication cookies
           // await HttpContext.SignOutAsync();

            // Perform any additional logout logic if needed

            // Return a success message or appropriate response
            return Ok("Logout successful");
        }

        private static bool IsValidUser( string password, User user)
        {
            return user.PasswordHash == Hashing.CreatePasswordHash(password);
        }
    }
}
