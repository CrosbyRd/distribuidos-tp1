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
            
            context.SaveChanges();
            return app;
        }
    }
}