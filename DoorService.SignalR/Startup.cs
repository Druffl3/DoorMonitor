using DoorMonitor.Common;
using DoorService.SignalR.Contexts;
using DoorService.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DoorService.SignalR
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
            string localLiteDb = "Data Source=" + GetDataStorePath() + "\\DoorDataBase.db";
            services.AddDbContext<DoorDbContext>(opt => opt.UseSqlite(localLiteDb));
            //services.AddDbContext<DoorDbContext>(opt => opt.UseInMemoryDatabase("DoorDatabase"));
            services.AddControllers();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<DoorStatusHub>(CommonConstants.DoorHubPath);
            });
        }

        /// <summary>
        /// Returns the Project DataStore path.
        /// </summary>
        /// <returns></returns>
        public string GetDataStorePath()
        {
            string workingDirectory = Environment.CurrentDirectory;
            return workingDirectory + "\\DataStore";
        }
    }
}
