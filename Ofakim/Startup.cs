using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ofakim.Contracts;
using Ofakim.Jobs;
using Ofakim.Services;
using System;
namespace Ofakim
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddScoped<IWriteToFile, WriteFileService>();
            services.AddScoped<IReadFromFile, ReadFileService>();

            services.AddHangfire(c => c.UseMemoryStorage());
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseHangfireDashboard();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            #region Job Scheduling Tasks  
            //update file every 5 minutes
            recurringJobManager.AddOrUpdate<UpdateXmlFileTask>("1", x => x.UpdateXmlFile(), "*/5 * * * *", TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time"));
            #endregion  
        }
    }
}
