using System;
using AutoMapper;
using back.Entities;
using back.Models;

namespace back.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DateTime, string>().ConvertUsing(date => date.ToString("dd/MM/yyyy HH:mm"));
            CreateMap<Hospital, HospitalModel>();
            CreateMap<Cama, CamaModel>();
            CreateMap<Log, LogsModel>();
        }
    }
}