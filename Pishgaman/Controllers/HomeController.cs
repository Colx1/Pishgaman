using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pishgaman.Data;
using Pishgaman.JwtHelper;
using Pishgaman.Models;
using Pishgaman.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pishgaman.Controllers
{
    public class HomeController : Controller
    {
        public DBRepository<PishgamanDB, Person, int> db;
        public IConfiguration Configuration { get; }
        private readonly ITokenService _tokenService;

        public HomeController(DBRepository<PishgamanDB, Person, int> _db, ITokenService tokenService, IConfiguration configuration)
        {
            db = _db;
            _tokenService = tokenService;
            Configuration = configuration;
        }

        [Authorize]
        public IActionResult Index()
        {
            string token = HttpContext.Session.GetString("Token");

            if (token == null)
                return (RedirectToAction("Index"));

            if (!_tokenService.IsTokenValid(Configuration["Jwt:Key"].ToString(), Configuration["Jwt:Issuer"].ToString(), token))
                return RedirectToAction("Login","Account");

            return View();
        }

    }
}
