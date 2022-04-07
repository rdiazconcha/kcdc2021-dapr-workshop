using DaprHospital.Patient.Api.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DaprHospital.Patient.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddPatientDb(Configuration);
        services.AddControllers()
                .AddDapr();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "DaprHospital.Patient.Api", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DaprHospital.Patient.Api v1"));
        }

        app.UseRouting();
        app.UseAuthorization();
        app.EnsurePatientDbIsCreated();
        app.UseCloudEvents();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapSubscribeHandler();
            endpoints.MapControllers();
        });
    }
}
