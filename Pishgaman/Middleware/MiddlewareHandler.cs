using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Pishgaman.Data;
using Pishgaman.Models;
using Pishgaman.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pishgaman.Middleware
{
    public class MiddlewareHandler
    {
        private readonly RequestDelegate _next;
        public IConfiguration Configuration { get; }

        public string[] blockedIps;

        public MiddlewareHandler(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            Configuration = configuration;

            blockedIps = Configuration.GetSection("BlockedIPAddresses").Get<string[]>();
        }

        public async Task Invoke(HttpContext context, DBRepository<PishgamanDB, JwtStoredToken, int> dbJwt)
        {
            if (blockedIps.Contains(context.Connection.RemoteIpAddress.ToString()))
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Sorry! Your IP Address is restricted by administrator!");
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var invalidToken = dbJwt.Get(filter: x => x.Blocked == true && x.Token == token).Any();

                if (invalidToken)
                {
                    context.Response.Clear();
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsync("Sorry! Your token is restricted by some reason!");
                }
            }

            //if (token != null)
            //    attachUserToContext(context, userService, token);

            await _next(context);
        }
    }
}
