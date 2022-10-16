using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pishgaman.CustomDataAnnotation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pishgaman.ViewModels
{
    public class PersonViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        //[Remote(action: "VerifyName", controller: "BookList", AdditionalFields = nameof(LastName))]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        //[Remote(action: "VerifyName", controller: "BookList", AdditionalFields = nameof(FirstName))]
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

        [Required]
        //[ValidateFile]
        public IFormFile file { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Img.Length >= 2)
        //    {
        //        yield return new ValidationResult("Error!");
        //    }
        //}

        //[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
        //public class ValidateFileAttribute : ValidationAttribute
        //{
        //    public override bool IsValid(object value)
        //    {
        //        int MaxContentLength = 1024 * 1024 * 1; //1 MB
        //        string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf" };

        //        var file = value as IFormFile;

        //        if (file == null)
        //            return false;
        //        else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
        //        {
        //            ErrorMessage = "Please upload Your Photo of type: " + string.Join(", ", AllowedFileExtensions);
        //            return false;
        //        }
        //        else if (file.Length > MaxContentLength)
        //        {
        //            ErrorMessage = "Your Photo is too large, maximum allowed size is : " + (MaxContentLength / 1024).ToString() + "MB";
        //            return false;
        //        }
        //        else
        //            return true;
        //    }
        //}
    }
}
