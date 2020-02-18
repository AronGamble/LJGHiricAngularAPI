using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LJGHistoryService.Models;
using LJGHistoryService.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace LJGHistoryService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {

        private readonly string storageString;

        private IConfiguration config;

        public HistoryController (IConfiguration _config)
        {
            config = _config;
            storageString = config.GetSection("LJGConfig").GetSection("Storage").Value;
        }

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
                empItems.Add(new EmploymentItem()
                {
                    CompanyName = y.CompanyName,
                    StartDate = y.StartDate,
                    EndDate = y.EndDate,
                    Id = int.Parse(y.RowKey),
                    Location = y.Location,
                    TypeOfEmployment = e,
                    Description = y.Description,
                    Detail = y.Detail                    
                });
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
                contractResult = new EmploymentItem()
                {
                    CompanyName = y.CompanyName,
                    StartDate = y.StartDate,
                    EndDate = y.EndDate,
                    Id = int.Parse(y.RowKey),
                    Location = y.Location,
                    TypeOfEmployment = e,
                    Detail = y.Detail
                };
            }

            System.Diagnostics.Trace.TraceInformation($"Information requested for {contractResult.CompanyName}");
            System.Diagnostics.Trace.TraceError($"False Error  {contractResult.CompanyName}");
            System.Diagnostics.Trace.TraceWarning($"False Warning  {contractResult.CompanyName}");

            return contractResult;
        }


        [HttpPost]
        public void Post([FromBody] string value)
        {
        }


    }
}
