using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using GlobalExceptionHandler.WebApi;
using System;
using API.Infrastructure.Models;
using System.Net;

namespace API.Infrastructure
{
    public static class GlobalExceptionHandler
    {
        public static void ApplyGlobalExceptionHandler(this IApplicationBuilder app, bool isDev)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            app.UseGlobalExceptionHandler(cfg =>
            {
            cfg.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;
            cfg.ResponseBody((ex, context) => JsonConvert.SerializeObject(
                new
                {
                    Message = "Something went wrong.",
                    StackTrace = isDev ? ex.ToString() : null,
                }, jsonSerializerSettings));

            cfg.Map<Exception>()
                .ToStatusCode(HttpStatusCode.InternalServerError)
                .WithBody((ex, context) =>
                    JsonConvert.SerializeObject(
                        new Result<Exception>()
                        {
                            IsSuccess = false,
                            Data = isDev ? ex : null
                        }));

            cfg.Map<UnauthorizedAccessException>()
                .ToStatusCode(HttpStatusCode.Unauthorized)
                .WithBody((ex, content) =>
                        JsonConvert.SerializeObject(
                            new Result<UnauthorizedAccessException>()
                            {
                                IsSuccess = false,
                                Data = isDev ? ex : null
                            }));
            });
        }
    }
}
