using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DaprHospital.Patient.Api.Infrastructure
{
    public class PatientDbContext : DbContext
    {
        public DbSet<Domain.Entities.Patient> Patients { get; set; }
        public PatientDbContext(DbContextOptions<PatientDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Domain.Entities.Patient>().HasKey(x => x.Id);
            modelBuilder.Entity<Domain.Entities.Patient>().OwnsOne(x => x.BloodType);
        }
    }

    public static class PatientDbContextExtensions
    {
        public static void AddPatientDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PatientDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Patient"));
            });
        }
        public static void EnsurePatientDbIsCreated(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<PatientDbContext>();
            context.Database.EnsureCreated();
            context.Database.CloseConnection();
        }
    }
}