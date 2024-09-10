using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace TasteTrailIdentity.Api.Common.Extensions.ServiceCollection;

public static class InitCorseMethod
{
    public static void InitCors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddCors(delegate (CorsOptions options)
        {
            options.AddPolicy("AllowAllOrigins", delegate (CorsPolicyBuilder builder)
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                //builder.WithOrigins("http://localhost", "http://localhost:5137", "http://20.218.160.138:80", "http://20.218.160.138").AllowAnyHeader().AllowAnyMethod();
            });
        });
    }
}
