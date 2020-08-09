using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SquidLogParser.Data;
using SquidLogParser.AccessLog;
using SquidLogParser.Services;
using Microsoft.Extensions.Logging;

namespace SquidLogParser
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContextPool<SquidLogContext>(options =>
            {
                var connectionString = Environment.GetEnvironmentVariable("SQUID_LOG_PARSER_APP_CONNECTION_STRING") ?? 
                                       "server=localhost;database=squid_log;user=root;password=r00t";
                options.UseMySQL(connectionString);
            });

            services.AddSingleton<IAccessLog>(service => {

                var loggerFactory = service.GetService<ILoggerFactory>();
                var accessLogFile = Environment.GetEnvironmentVariable("SQUID_ACCESS_LOG_FILE_PATH") ??
                                    "/var/logs/squid/access.log";

                return new AccessLogFile(loggerFactory.CreateLogger<AccessLogFile>(), accessLogFile);
            });

            services.AddSingleton<IAccessLogParser>(service => {

                var loggerFactory = service.GetService<ILoggerFactory>();

                return new AccessLogParser(loggerFactory.CreateLogger<AccessLogParser>());
            });

            services.AddScoped<LogService>();

            services.AddRazorPages();
            services.AddServerSideBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
