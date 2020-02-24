using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LJGHistoryService.Models;
using LJGHistoryService.Repositories;
using LJGHistoryService.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace LJGHistoryService.Controllers
{
    [Route("api/contracts")]
    [ApiController]
    public class ContractsController : ControllerBase
    {

        private readonly string storageString;

        private IConfiguration config;
        private IContractRepository contractRepository;

        public ContractsController(IConfiguration _config, IContractRepository _contractRepository)
        {
            config = _config;
            storageString = config.GetSection("LJGConfig").GetSection("Storage").Value;
            contractRepository = _contractRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EmploymentItem>> GetContracts()
        {
            var contracts = contractRepository.GetAllContracts().Result;

            return Ok(contracts);
        }
        

        [HttpGet("{id:int}")]
        public ActionResult<EmploymentItem> GetContract(int id)
        {
            EmploymentItem contractResult = contractRepository.GetContracts(id).Result;

            if (contractResult == null)
            {
                return NotFound();
            }

            return Ok(contractResult);

        }

        [HttpPost]
        public void AddContract([FromBody] string value)
        {
        }

        [HttpPut]
        public void UpdateContract(int id)
        {
        }

        [HttpDelete]
        public void DeleteContract(int id)
        {
        }


        [HttpGet("{id}/References")]
        public async Task<EmploymentItem> GetReferences(int id)
        {
            // Replace this code, just a stub for testing

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

            return contractResult;
        }


        [HttpGet("{id}/References/{referenceId}")]
        public async Task<EmploymentItem> GetReferenceById(int id, int referenceId)
        {

            // Replace this code, just a stub for testing

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


            return contractResult;
        }


    }
}
