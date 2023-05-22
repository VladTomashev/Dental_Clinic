using Microsoft.EntityFrameworkCore;

namespace Dental_Clinic.entity
{
    public class DataContext : DbContext
    {
        public DbSet<Patient> patients { get; set; }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<ClinicService> clinicServices { get; set; }
        public DbSet<Appointment> appointments { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<RefToken> refreshTokens { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(builder =>
            {
                builder.ToTable("appointments");
                builder.HasKey(a => a.id);

                builder.HasOne(a => a.patient)
                .WithMany(p => p.appointments)
                .HasForeignKey(a => a.patientId)
                .HasPrincipalKey(p => p.id);
                builder.Navigation(a => a.patient).AutoInclude();

                builder.HasOne(a => a.doctor)
                .WithMany(d => d.appointments)
                .HasForeignKey(a => a.doctorId)
                .HasPrincipalKey(d => d.id);
                builder.Navigation(a => a.doctor).AutoInclude();

                builder.HasOne(a => a.clinicService)
                .WithMany()
                .HasForeignKey(a => a.serviceId)
                .HasPrincipalKey(c => c.id);
                builder.Navigation(a => a.clinicService).AutoInclude();

            });

            modelBuilder.Entity<Patient>(builder =>
            {
                builder.ToTable("patients");
                builder.HasKey(p => p.id);
            });

            modelBuilder.Entity<Doctor>(builder =>
            {
                builder.ToTable("doctors");
                builder.HasKey(d => d.id);
            });

            modelBuilder.Entity<ClinicService>(builder =>
            {
                builder.ToTable("clinic_services");
                builder.HasKey(c => c.id);
            });

            modelBuilder.Entity<User>(builder =>
            {
                builder.ToTable("users");
                builder.HasKey(u => u.id);

                builder.Navigation(u => u.refreshToken).AutoInclude();
            });

            modelBuilder.Entity<RefToken>(builder =>
            {
                builder.ToTable("refresh_tokens");
                builder.HasKey(r => r.id);
                builder.HasOne(r => r.user)
                .WithOne(u => u.refreshToken)
                .HasForeignKey("RefToken");

                builder.Navigation(r => r.user).AutoInclude();

            });
        }
    }
}
