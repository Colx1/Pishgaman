using Pishgaman.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pishgaman.Models
{
    public class IpAddressBlacklist : IEntity<int>
    {
        public int Id { get; set; }

        public string IpAddress { get; set; }
    }
}
