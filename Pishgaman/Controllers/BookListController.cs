using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pishgaman.Areas.Identity.Data;
using Pishgaman.Data;
using Pishgaman.Models;
using Pishgaman.Repositories;
using Pishgaman.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pishgaman.Controllers
{
    public class BookListController : Controller
    {
        public DBRepository<PishgamanDB, Person, int> db;

        public BookListController(DBRepository<PishgamanDB, Person, int> db)
        {
            this.db = db;
        }

        [Authorize(Roles = "Read")]
        public IActionResult ShowTable()
        {
            return View();
        }

        [Authorize(Roles = "Write")]
        public IActionResult AddPerson()
        {
            return View();
        }

        [Authorize(Roles = "Write")]
        public IActionResult EditPerson(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var p = db.Get(filter: x => x.Id == id).FirstOrDefault();

            EditPersonViewModel pvm = new EditPersonViewModel()
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                FatherName = p.FatherName,
                NatCode = p.NatCode,
                PhoneNumber = p.PhoneNumber
            };

            return View(pvm);
        }

        [AcceptVerbs("GET")]
        public IActionResult VerifyName(string firstName, string lastName)
        {
            if (db.Get(filter: x => x.FirstName == firstName && x.LastName == lastName).Any())
                return Json($"A user named {firstName} {lastName} already exists.");

            return Json(true);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class BookListApiController : Controller
    {
        public DBRepository<PishgamanDB, Person, int> db;
        private readonly UserManager<ApplicationUser> userManager;

        public BookListApiController(DBRepository<PishgamanDB, Person, int> db, UserManager<ApplicationUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Write")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddPerson([FromForm] PersonViewModel model)
        {
            var userapp = await userManager.FindByNameAsync(User.Identity.Name);
            if (userapp == null)
                return null;

            Person person = new Person()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                FatherName = model.FatherName,
                NatCode = model.NatCode,
                PhoneNumber = model.PhoneNumber,

                AddedByUserID = userapp.Id
            };

            var x = model.file;

            byte[] b = new byte[x.Length];
            x.OpenReadStream().Read(b, 0, b.Length);
            person.Img = b;

            db.Insert(person);
            db.Save();

            return RedirectPermanent("/Booklist/Showtable");
        }

        [Authorize(Roles = "Write")]
        [HttpPost("[action]")]
        public IActionResult EditPerson([FromForm] EditPersonViewModel model)
        {
            if (model.Id == 0)
                return NotFound();

            var person = db.Get(filter: x => x.Id == model.Id).FirstOrDefault();


            person.FirstName = model.FirstName;
            person.LastName = model.LastName;
            person.FatherName = model.FatherName;
            person.NatCode = model.NatCode;
            person.PhoneNumber = model.PhoneNumber;

            if (model.file != null)
            {
                var x = model.file;

                byte[] b = new byte[x.Length];
                x.OpenReadStream().Read(b, 0, b.Length);
                person.Img = b;
            }


            db.Update(person);
            db.Save();

            return RedirectPermanent("/Booklist/Showtable");
        }

        [Authorize(Roles = "Write")]
        [HttpGet("[action]/{id}")]
        public IActionResult DeletePerson(int id)
        {
            if (id == 0)
                return BadRequest();

            var p = db.Get(filter: x => x.Id == id).FirstOrDefault();
            if (p == null)
                return NotFound();

            db.Delete(p.Id);
            db.Save();

            return RedirectPermanent("/Booklist/Showtable");
        }

        [Authorize(Roles = "Read")]
        [HttpGet("[action]")]
        public IActionResult GetPartialTable(string filter = null, int? pageNum = null, int? pageCountRec = null)
        {
            List<Person> plist = new List<Person>();

            if (string.IsNullOrWhiteSpace(filter))
                plist = db.Get(includeProperties: "AddedUserID");

            else
                plist = db.Get(filter: x => x.FirstName.Contains(filter)
                || x.LastName.Contains(filter)
                || x.NatCode.Contains(filter));

            var count = plist.Count();

            if (pageNum.HasValue && pageCountRec.HasValue)
                plist = plist.Skip((pageNum.Value - 1) * pageCountRec.Value).Take(pageCountRec.Value).ToList();

            var test = (int)((pageCountRec.HasValue) ? Math.Ceiling(((double)count / (double)pageCountRec)) : 1);

            ViewData["PageCounts"] = count;
            ViewData["PageNums"] = (int)((pageCountRec.HasValue) ? Math.Ceiling((double)count / (double)pageCountRec) : 1);

            ViewData["SelectedPageCount"] = pageCountRec ?? plist.Count();
            ViewData["SelectedPageNum"] = pageNum ?? 1;

            return PartialView("/Views/BookList/_PartialShowTable.cshtml", plist);
        }
    }

}
