using Pishgaman.Areas.Identity.Data;
using Pishgaman.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pishgaman.Models
{
    public class JwtStoredToken : IEntity<int>
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser UserApp { get; set; }

        public string Token { get; set; }

        public bool Blocked { get; set; } = false;
    }
}
