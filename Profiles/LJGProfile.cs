using AutoMapper;
using LJGHistoryService.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJGHistoryService.Profiles
{
    public class LJGProfile : Profile
    {
        public LJGProfile()
        {
            CreateMap<Contract, Models.EmploymentItem>().ForMember(
                dest => dest.NiceDate,
                opt => opt.MapFrom(src => $"{src.StartDate.ToShortDateString()} - {src.EndDate.ToShortDateString()}")
                );
        }
    }
}
