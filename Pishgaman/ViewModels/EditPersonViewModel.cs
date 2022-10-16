using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pishgaman.ViewModels
{
    public class EditPersonViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FatherName { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]){10}$", ErrorMessage = "Not a valid national code")]
        public string NatCode { get; set; }

        [Required]
        [RegularExpression(@"^(\+98|0)?9\d{9}$", ErrorMessage = "Not a valid phone number!")]
        public string PhoneNumber { get; set; }

        public IFormFile file { get; set; }
    }
}
