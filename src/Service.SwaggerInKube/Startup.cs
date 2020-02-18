using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Swisschain.Sdk.Server.Common;
using Service.SwaggerInKube.Services;

namespace Service.SwaggerInKube
{
    public class Startup : SwisschainStartup
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureExt(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwaggerUI(options =>
            {
                var namespacesLine = Environment.GetEnvironmentVariable("SWAGGER_KUBE_NAMESPACES");
                var url = Environment.GetEnvironmentVariable("SWAGGER_KUBE_APIURL");
                var token = Environment.GetEnvironmentVariable("SWAGGER_KUBE_APITOKEN");

                if (!string.IsNullOrEmpty(namespacesLine) && !string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(token))
                {
                    foreach (var ns in namespacesLine.Split(';'))
                    {
                        var items = SwagerInKubeInfo.GetAllWaggers(
                            ns,
                            url,
                            token);

                        foreach (var item in items)
                        {
                            options.SwaggerEndpoint(item.Url, item.Name);
                        }
                    }
                }
            });
        }
    }
}
