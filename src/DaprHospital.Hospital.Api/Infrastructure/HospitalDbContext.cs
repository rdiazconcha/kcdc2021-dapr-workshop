using DaprHospital.Hospital.Api.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DaprHospital.Hospital.Api.Infrastructure;

public class HospitalDbContext : DbContext
{
    public DbSet<Inpatient> Inpatients { get; set; }
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Inpatient>().HasKey(x => x.Id);
        modelBuilder.Entity<Procedure>().HasKey(x => x.Id);
        modelBuilder.Entity<Procedure>().OwnsOne(x => x.Name);
        var navigation = modelBuilder.Entity<Inpatient>().Metadata.FindNavigation(nameof(Inpatient.Procedures));
        navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}

public static class HospitalDbContextExtensions
{
    public static void AddHospitalDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HospitalDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Hospital"));
        });
    }
    public static void EnsureHospitalDbIsCreated(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetService<HospitalDbContext>();
        context.Database.EnsureCreated();
        context.Database.CloseConnection();
    }
}
