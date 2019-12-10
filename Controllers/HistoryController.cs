using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LJGHistoryService.Models;
using LJGHistoryService.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace LJGHistoryService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {

        private readonly List<EmploymentItem> employmentItems = new List<EmploymentItem>() {

                new EmploymentItem() { Id = 1, CompanyName = "Solicitors Regulation Authority", StartDate = DateTime.Parse("01/11/2018").ToUniversalTime(), EndDate = DateTime.Parse("01/03/2019").ToUniversalTime(), Location = "Birmingham", TypeOfEmployment = EmploymentType.Contract },
                new EmploymentItem() { Id = 2, CompanyName = "ERGO", StartDate = DateTime.Parse("01/02/2017").ToUniversalTime(), EndDate = DateTime.Parse("01/11/2018").ToUniversalTime(), Location = "Birmingham", TypeOfEmployment = EmploymentType.Contract  }
            };

        [HttpGet]
        public async Task<IEnumerable<EmploymentItem>> Get()
        {

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=ljgwebsite;AccountKey=4+lge0bw2MN6o9Z4DssravCHaR1ZXuwN+1t26KM8Tb0w+gJeR90iQqFr6HQE/OCG+wjRrzx4+qU0eRLfjgtI6w==;EndpointSuffix=core.windows.net");

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("contracts");

            TableQuery<Contract> query = new TableQuery<Contract>()
                   .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "ljgwebsite"));

            var x = await table.ExecuteQuerySegmentedAsync(query, null);

            var empItems = new List<EmploymentItem>();

            foreach (var y in x)
            {
                EmploymentType e = y.TypeOfEmployment == "1" ? EmploymentType.Permanent : EmploymentType.Contract;
                empItems.Add(new EmploymentItem() { CompanyName = y.CompanyName, StartDate = y.StartDate, EndDate = y.EndDate, Id = y.Id, Location = y.Location, TypeOfEmployment = e });
            }

            return empItems;
        }


        [HttpGet("{id}", Name = "Get")]
        public EmploymentItem Get(int id)
        {
            return employmentItems.Where(x => x.Id == id).Single();
        }


        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

       
    }
}
