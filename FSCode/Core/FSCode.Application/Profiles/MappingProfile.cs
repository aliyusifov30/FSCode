using AutoMapper;
using FSCode.Application.DTOs.ReminderDTOs;
using FSCode.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSCode.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ReminderCreateDto, Reminder>();
        }
    }
}
