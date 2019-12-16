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

        //private readonly List<EmploymentItem> employmentItems = new List<EmploymentItem>() {

        //        new EmploymentItem() { Id = 1, CompanyName = "Solicitors Regulation Authority", StartDate = DateTime.Parse("01/11/2018").ToUniversalTime(), EndDate = DateTime.Parse("01/03/2019").ToUniversalTime(), Location = "Birmingham", TypeOfEmployment = EmploymentType.Contract },
        //        new EmploymentItem() { Id = 2, CompanyName = "ERGO", StartDate = DateTime.Parse("01/02/2017").ToUniversalTime(), EndDate = DateTime.Parse("01/11/2018").ToUniversalTime(), Location = "Birmingham", TypeOfEmployment = EmploymentType.Contract  }
        //    };

        private readonly string storageString = "DefaultEndpointsProtocol=https;AccountName=ljgwebsite;AccountKey=4+lge0bw2MN6o9Z4DssravCHaR1ZXuwN+1t26KM8Tb0w+gJeR90iQqFr6HQE/OCG+wjRrzx4+qU0eRLfjgtI6w==;EndpointSuffix=core.windows.net";

        [HttpGet]
        public async Task<IEnumerable<EmploymentItem>> Get()
        {

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("contracts");

            TableQuery<Contract> query = new TableQuery<Contract>()
                   .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "ljgwebsite"));

            var x = await table.ExecuteQuerySegmentedAsync(query, null);

            var empItems = new List<EmploymentItem>();

            foreach (var y in x)
            {
                EmploymentType e = y.TypeOfEmployment == "1" ? EmploymentType.Permanent : EmploymentType.Contract;
                empItems.Add(new EmploymentItem() { CompanyName = y.CompanyName, StartDate = y.StartDate, EndDate = y.EndDate, Id = y.Id, Location = y.Location, TypeOfEmployment = e, Description = y.Description });
            }

            return empItems;
        }


        [HttpGet("{id}", Name = "Get")]
        public async Task<EmploymentItem> Get(int id)
        {

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference("contracts");

            var cond1 = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "ljgwebsite");
            var cond2 = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id.ToString());

            TableQuery<Contract> query = new TableQuery<Contract>()
                   .Where(TableQuery.CombineFilters(cond1, TableOperators.And, cond2)).Take(1);

            var x = await table.ExecuteQuerySegmentedAsync(query, null);
            EmploymentItem contractResult = null;

            foreach (var y in x)
            {
                EmploymentType e = y.TypeOfEmployment == "1" ? EmploymentType.Permanent : EmploymentType.Contract;
                contractResult  = new EmploymentItem() { CompanyName = y.CompanyName, StartDate = y.StartDate, EndDate = y.EndDate, Id = y.Id, Location = y.Location, TypeOfEmployment = e };
            }

            return contractResult;
        }


        [HttpPost]
        public void Post([FromBody] string value)
        {
        }


    }
}
