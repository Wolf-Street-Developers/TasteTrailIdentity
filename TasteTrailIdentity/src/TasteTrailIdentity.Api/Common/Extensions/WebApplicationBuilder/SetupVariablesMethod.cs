namespace TasteTrailIdentity.Api.Common.Extensions.WebApplicationBuilder;

using System;
using Microsoft.AspNetCore.Builder;


public static class SetupVariablesMethod
{
    public static void SetupVariables(this WebApplicationBuilder builder)
    {
        var postgresConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING") ?? throw new SystemException("there is no var POSTGRES_CONNECTION_STRING");
        
        var azureConnectionString = Environment.GetEnvironmentVariable("BLOB_STORAGE_CONNECTION_STRING") ?? throw new SystemException("there is no var BLOB_STORAGE_CONNECTION_STRING");
        
        var jwt_key = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new SystemException("there is no var JWT_KEY");
        var jwt_life_time = Environment.GetEnvironmentVariable("JWT_LIFE_TIME_IN_MINUTES") ?? throw new SystemException("there is no var JWT_LIFE_TIME_IN_MINUTES");
        var jwt_issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? throw new SystemException("there is no var JWT_ISSUER");
        var jwt_audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? throw new SystemException("there is no var JWT_AUDIENCE");
        

        builder.Configuration["Jwt:Key"] = jwt_key;
        builder.Configuration["Jwt:LifeTimeInMinutes"] =  jwt_life_time;
        builder.Configuration["Jwt:Issuer"] = jwt_issuer;
        builder.Configuration["Jwt:Audience"] = jwt_audience;

        builder.Configuration["ConnectionStrings:SqlConnection"] = postgresConnectionString;
        
        builder.Configuration["ConnectionStrings:AzureBlobStorage"] = azureConnectionString;
        
    }
}
