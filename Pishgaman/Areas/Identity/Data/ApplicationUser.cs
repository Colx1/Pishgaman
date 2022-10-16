using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Pishgaman.Models;

namespace Pishgaman.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ICollection<JwtStoredToken> JwtStoredToken { get; set; }

        public ICollection<Person> PAuthors { get; set; }
    }
}
