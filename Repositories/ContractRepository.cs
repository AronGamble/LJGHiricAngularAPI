using AutoMapper;
using LJGHistoryService.Models;
using LJGHistoryService.Tables;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LJGHistoryService.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly string storageString;
        private readonly IConfiguration config;
        public IMapper _mapper { get; }

        public ContractRepository(IConfiguration _config, IMapper mapper)
        {
            config = _config ??
                throw new ArgumentNullException(nameof(_config));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            storageString = config.GetSection("LJGConfig").GetSection("Storage").Value;

        }



        public async Task<IEnumerable<EmploymentItem>> GetAllContracts()
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

            var employmentItems = _mapper.Map<IEnumerable<EmploymentItem>>(x);

            return employmentItems;
           
        }



        public async Task<EmploymentItem> GetContracts(int id)
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

            return contractResult;
        }

    }
}
