using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pishgaman.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pishgaman.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        //[HttpPost("[action]/{username}/{password}")]
        //public async Task<IActionResult> CreateAccount(string username, string password)
        //{
        //    ApplicationUser userapp = new ApplicationUser()
        //    {
        //        UserName = username,
        //    };

        //    var result = await userManager.CreateAsync(userapp, password);
        //    if (result.Succeeded)
        //        return Ok("User successfully created");
        //    return BadRequest("Bad request");
        //}
    }
}
