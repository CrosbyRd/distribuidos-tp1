using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using back.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace back
{
    internal static class StartupExtension
    {
         public static IApplicationBuilder SeedTestData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UtiContext>();

            if (context.Hospitales.Any())
            {
                return app;
            }

            var hospital1 = new Hospital
            {
                Nombre = "Materno Infantil de Trinidad"
            };

            context.Hospitales.Add(hospital1);
            
            var hospital2 = new Hospital
            {
                Nombre = "IPS Nanawa"
            };

            context.Hospitales.Add(hospital2);
            
            var hospital3 = new Hospital
            {
                Nombre = "Hospital del quemado"
            };

            context.Hospitales.Add(hospital3);
            
            var hospital4 = new Hospital
            {
                Nombre = "De la costa"
            };

            context.Hospitales.Add(hospital4);
            
            var hospital5 = new Hospital
            {
                Nombre = "Migone"
            };

            context.Hospitales.Add(hospital5);
            
            var hospital6 = new Hospital
            {
                Nombre = "IPS Central"
            };

            context.Hospitales.Add(hospital6);
            
            var hospital7 = new Hospital
            {
                Nombre = "Banco de ojos"
            };

            context.Hospitales.Add(hospital7);
            
            var hospital8 = new Hospital
            {
                Nombre = "Hospital de Itaugua"
            };

            context.Hospitales.Add(hospital8);
            
            var hospital9 = new Hospital
            {
                Nombre = "Materno Infantil de Loma Pyta"
            };

            context.Hospitales.Add(hospital9);

            var cama1 = new Cama
            {
                HospitalId = hospital1.Id,
                Orden = 1
            };
            context.Camas.Add(cama1);
            
            var cama2 = new Cama
            {
                HospitalId = hospital1.Id,
                Orden = 2,
                EstaOcupado = true
            };
            context.Camas.Add(cama2);
            
            var cama3 = new Cama
            {
                HospitalId = hospital2.Id,
                Orden = 1
            };
            context.Camas.Add(cama3);
            
            var cama4 = new Cama
            {
                HospitalId = hospital2.Id,
                Orden = 2,
                EstaOcupado = true
            };
            context.Camas.Add(cama4);
            
            
            var cama5 = new Cama
            {
                HospitalId = hospital3.Id,
                Orden = 1
            };
            context.Camas.Add(cama5);
            
            var cama6 = new Cama
            {
                HospitalId = hospital3.Id,
                Orden = 2,
                EstaOcupado = true
            };
            context.Camas.Add(cama6);
            var cama7 = new Cama
            {
                HospitalId = hospital4.Id,
                Orden = 1
            };
            context.Camas.Add(cama7);
            var cama8 = new Cama
            {
                HospitalId = hospital4.Id,
                Orden = 2,
                EstaOcupado = true
            };
            context.Camas.Add(cama8);
            var cama9 = new Cama
            {
                HospitalId = hospital5.Id,
                Orden = 1
            };
            context.Camas.Add(cama9);
            var cama10 = new Cama
            {
                HospitalId = hospital5.Id,
                Orden = 2,
                EstaOcupado = true
            };
            context.Camas.Add(cama10);
            var cama11 = new Cama
            {
                HospitalId = hospital6.Id,
                Orden = 1
            };
            context.Camas.Add(cama11);
            var cama12 = new Cama
            {
                HospitalId = hospital6.Id,
                Orden = 2,
                EstaOcupado = true
            };
            context.Camas.Add(cama12);
            var cama13 = new Cama
            {
                HospitalId = hospital7.Id,
                Orden = 1
            };
            context.Camas.Add(cama13);
            var cama14 = new Cama
            {
                HospitalId = hospital7.Id,
                Orden = 2,
                EstaOcupado = true
            };
            context.Camas.Add(cama14);
            var cama15 = new Cama
            {
                HospitalId = hospital8.Id,
                Orden = 1
            };
            context.Camas.Add(cama15);
            var cama16 = new Cama
            {
                HospitalId = hospital8.Id,
                Orden = 2,
                EstaOcupado = true
            };
            context.Camas.Add(cama16);
            var cama17 = new Cama
            {
                HospitalId = hospital9.Id,
                Orden = 1
            };
            context.Camas.Add(cama17);
            var cama18 = new Cama
            {
                HospitalId = hospital9.Id,
                Orden = 2,
                EstaOcupado = true
            };
            context.Camas.Add(cama18);
            context.SaveChanges();
            return app;
        }
    }
}