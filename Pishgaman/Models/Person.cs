using Pishgaman.Areas.Identity.Data;
using Pishgaman.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pishgaman.Models
{
    public class Person : IEntity<int>
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string NatCode { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Img { get; set; }

        public string AddedByUserID { get; set; }
        [ForeignKey(nameof(AddedByUserID))]
        public ApplicationUser AddedUserID { get; set; }
    }
}
