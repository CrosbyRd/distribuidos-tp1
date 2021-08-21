using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using back.Entities;
using back.Models;
using Microsoft.EntityFrameworkCore;

namespace back.Controllers
{
    public class HospitalController
    {
        private readonly UtiContext _utiContext;
        private readonly IMapper _mapper;

        public HospitalController(UtiContext utiContext, IMapper mapper)
        {
            _utiContext = utiContext;
            _mapper = mapper;
        }

        public async Task<int> CrearHospital(string nombre)
        {
            try
            {
                var newHospital = new Hospital
                {
                    Nombre = nombre
                };

                _utiContext.Hospitales.Add(newHospital);

                await _utiContext.SaveChangesAsync();

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public async Task<List<HospitalModel>> VerEstados()
        {
            try
            {
                var list = await _utiContext.Hospitales.ProjectTo<HospitalModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<int> EliminarHospital(CancellationToken ct)
        {
            try
            {
                _utiContext.Hospitales.RemoveRange(_utiContext.Hospitales);

                await _utiContext.SaveChangesAsync(ct);

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 2;
            }
        }
    }
}