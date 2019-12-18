using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Appication.Activities;
using Persistence;
using FluentValidation.AspNetCore;
using API.Middleware;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // to add services to the container - amo o meu amor
        // this a DI contaier
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(opt => {
                opt.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });
                // resolt cors policy 
               services.AddCors(opt => {
                   opt.AddPolicy("CorsPolicy", policy =>{
                       policy.AllowAnyHeader()                      //
                             .AllowAnyMethod()                      // allow any method from fron the address setted in 
                             .WithOrigins("http://localhost:3000"); // withOrigin method <----
                   });
               }); 
               services.AddMediatR(typeof(List.Handler).Assembly); // here we need to tell meadiatr which assembly our hadler is alocated in 

            services.AddControllers()
            .AddFluentValidation(cfg =>
            {// specify the assembly where we are using the FluentValidator
               cfg.RegisterValidatorsFromAssemblyContaining<Create>(); 
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            if (env.IsDevelopment())
            {
              //  app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("CorsPolicy");


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
