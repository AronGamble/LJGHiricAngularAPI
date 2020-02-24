using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJGHistoryService.Models
{
    public class EmploymentItem
    {
        public int Id  { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EmploymentType TypeOfEmployment { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public string NiceDate { get; set; }
    }
}
