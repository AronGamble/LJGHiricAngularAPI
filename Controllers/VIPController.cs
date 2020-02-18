using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LJGHistoryService.Models;
using LJGHistoryService.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace LJGHistoryService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VIPController : ControllerBase
    {
        private IConfiguration config;

        public VIPController(IConfiguration _config)
        {
            config = _config;
        }


        [HttpGet]
        public async Task<IEnumerable<EmploymentItem>> Get()
        {

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config.GetSection("LJGConfig").GetSection("Storage").Value);

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




        [HttpPost]
        public bool Post([FromBody] ContactItem contactItem)
        {
            var x = contactItem;

            return true;

        }


    }
}
