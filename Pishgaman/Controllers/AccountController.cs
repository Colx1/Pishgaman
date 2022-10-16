using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pishgaman.Areas.Identity.Data;
using Pishgaman.Data;
using Pishgaman.JwtHelper;
using Pishgaman.Models;
using Pishgaman.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pishgaman.Controllers
{
    public class AccountController : Controller
    {
        public DBRepository<PishgamanDB, JwtStoredToken, int> dbJwt;

        private readonly UserManager<ApplicationUser> userManager;
        public IConfiguration Configuration { get; }

        private readonly ITokenService _tokenService;
        private string generatedToken = null;

        public AccountController(UserManager<ApplicationUser> _userManager, IConfiguration configuration, ITokenService tokenService, DBRepository<PishgamanDB, JwtStoredToken, int> _db)
        {
            userManager = _userManager;
            Configuration = configuration;
            _tokenService = tokenService;
            dbJwt = _db;
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> LoginConfirm(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return RedirectToAction("Error");

            var userapp = await userManager.FindByNameAsync(username);
            if (userapp == null)
                return RedirectToAction("Error");

            var roles = await userManager.GetRolesAsync(userapp);

            generatedToken = _tokenService.BuildToken(Configuration["Jwt:Key"].ToString(),
                Configuration["Jwt:Issuer"].ToString(), userapp, roles.ToList());

            if (generatedToken != null)
            {
                HttpContext.Session.SetString("Token", generatedToken);

                JwtStoredToken jwtStoredToken = new JwtStoredToken()
                {
                    UserApp = userapp,
                    Token = generatedToken
                };

                dbJwt.Insert(jwtStoredToken);
                dbJwt.Save();

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Error");
        }

        public IActionResult Logout()
        {
            var token = HttpContext.Session.GetString("Token");
            if (token != null)
                HttpContext.Session.Remove("Token");

            return RedirectToAction("Login");
        }

    }

    [ApiController]
    [Route("api/[controller]")]
    public class AccountApiController : Controller
    {
        public DBRepository<PishgamanDB, JwtStoredToken, int> dbJwt;

        private readonly UserManager<ApplicationUser> userManager;

        public AccountApiController(DBRepository<PishgamanDB, JwtStoredToken, int> dbJwt, UserManager<ApplicationUser> userManager)
        {
            this.dbJwt = dbJwt;
            this.userManager = userManager;
        }

        [HttpPost("[action]/{userName}")]
        public async Task<IActionResult> BlockJwtTokensByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return BadRequest();

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
                return NotFound("User not found");

            var usertokens = dbJwt.Get(filter: x => x.UserId == user.Id);
            usertokens.ForEach(x =>
            {
                x.Blocked = true;
            });

            dbJwt.Update(usertokens);
            dbJwt.Save();

            return Ok($"All tokens til now by User: {user.UserName} - ID: {user.Id} restricted.");
        }
    }
}
