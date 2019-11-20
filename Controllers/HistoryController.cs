using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LJGHistoryService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LJGHistoryService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly List<EmploymentItem> employmentItems = new List<EmploymentItem>() {
                new EmploymentItem() { Id = 1, CompanyName = "Solicitors Regulation Authority", StartDate = DateTime.Parse("01/11/2018"), EndDate = DateTime.Parse("01/03/2019"), Location = "Birmingham", TypeOfEmployment = EmploymentType.Contract },
                new EmploymentItem() { Id = 2, CompanyName = "ERGO", StartDate = DateTime.Parse("01/02/2017"), EndDate = DateTime.Parse("01/11/2018"), Location = "Birmingham", TypeOfEmployment = EmploymentType.Contract  }
            };


        // GET: api/History
        [HttpGet]
        public IEnumerable<EmploymentItem> Get()
        {
            //var employmentItems = new List<EmploymentItem>() {
            //    new EmploymentItem() { Id = 1, CompanyName = "Solicitors Regulation Authority", StartDate = DateTime.Parse("01/11/2018"), EndDate = DateTime.Parse("01/03/2019"), Location = "Birmingham", TypeOfEmployment = EmploymentType.Contract },
            //    new EmploymentItem() { Id = 2, CompanyName = "ERGO", StartDate = DateTime.Parse("01/02/2017"), EndDate = DateTime.Parse("01/11/2018"), Location = "Birmingham", TypeOfEmployment = EmploymentType.Contract  }
            //};

            return employmentItems;
        }

        // GET: api/History/5
        [HttpGet("{id}", Name = "Get")]
        public EmploymentItem Get(int id)
        {
            return employmentItems.Where(x => x.Id == id).Single();

            //return "value";
        }

        // POST: api/History
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

       
    }
}
