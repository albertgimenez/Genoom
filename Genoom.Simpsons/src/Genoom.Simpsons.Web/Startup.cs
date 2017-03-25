using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Genoom.Simpsons.Repository;

namespace Genoom.Simpsons.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Standard MVC
            services.AddMvc();

            // Swagger documentation API
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Genoom Simpsons Tree", Version = "v1" });
            });

            // The database provider (strategy) to use to access the data.
            services.AddSingleton<IPeopleRepository>(Support.PeopleRepositoryFactory.Create(Configuration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            // Swagger API doc
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Genoom Simpsons Tree v1");
            });

            // The default routes, by default if does not exist (404) we want to provide a nice response.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Error404",
                    template: "{*url}",
                    defaults: new { controller = "Error", action = "Handle404" }
                );
            });
        }
    }
}
