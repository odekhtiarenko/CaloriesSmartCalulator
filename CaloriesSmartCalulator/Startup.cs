using CaloriesSmartCalulator.DAL;
using CaloriesSmartCalulator.Data;
using CaloriesSmartCalulator.Data.Entities;
using CaloriesSmartCalulator.Handlers.CommandHandlers;
using CaloriesSmartCalulator.Handlers.QueryHandlers;
using CaloriesSmartCalulator.HostedService;
using CaloriesSmartCalulator.MapperProfile;
using CaloriesSmartCalulator.ServiceClients;
using CaloriesSmartCalulator.Validators;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CaloriesSmartCalulator
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
            services.AddControllersWithViews()
                    .AddFluentValidation(config =>
                    {
                        config.RegisterValidatorsFromAssemblyContaining<CalculateMealCaloriesRequestValidator>();
                    });


            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddMediatR(typeof(CreateCaloriesCalculationCommandHandler).Assembly,
                                typeof(GetCaloriesCalculationTaskQueryHandler).Assembly);

            services.AddDbContext<CaloriesCalulatorDBContext>(options =>
                 options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Transient);

            services.AddScoped<IRepository<CaloriesCalculationTask>, CaloriesCalculationTaskRepository>();
            services.AddScoped<IRepository<CaloriesCalculationTaskItem>, CaloriesCalculationTaskItemRepository>();

            services.AddScoped<ICaloriesServiceClient, CaloriesServiceClient>();

            services.AddHostedService<CaloriesCalculationHostedService>();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CaloriesSmartCalculator", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CaloriesSmartCalculatorTest v1"));

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<CaloriesCalulatorDBContext>();
                if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                    context.Database.Migrate();

            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
