using LeadManagementSystem.Model.Models;
using Microsoft.EntityFrameworkCore;


namespace LeadManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<LeadModel> LeadModel { get; set; }
        public DbSet<LeadStatusModel> LeadStatusModel { get; set; }
        public DbSet<LeadResponseModel> LeadResponseModel { get; set; }
        public DbSet<LeadSourceModel> LeadSourceModels { get; set; }
        public DbSet<BrandModel> BrandModel { get; set; }
        public DbSet<GetCustomerStatusResponseModel> GetCustomerStatusResponseModel { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configure LeadSource
            modelBuilder.Entity<LeadSourceModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.BrandModel)
                      .WithMany(b => b.LeadSourceModel)
                      .HasForeignKey(e => e.BrandId);
            });
            modelBuilder.Entity<LeadStatusModel>(entity =>
            {

                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.LeadModel)
                      .WithMany(b => b.LeadStatusHistory)
                      .HasForeignKey(e => e.LeadId);
                
                entity.HasOne(lsh => lsh.LeadSource)
                     .WithMany(ls => ls.LeadStatusHistories)
                     .HasForeignKey(lsh => lsh.UMID)
                     .HasPrincipalKey(ls => ls.UMID) // Use UMID as the principal key
                     .IsRequired();
            });
            modelBuilder.Entity<LeadModel>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            // Configure BrandMaster
            modelBuilder.Entity<BrandModel>(entity =>
            {
                entity.HasKey(e => e.BrandId);
            });

            modelBuilder.Entity<GetCustomerStatusResponseModel>()
                .HasOne(g => g.LeadModel)               
                .WithMany(l => l.GetCustomerStatusResponses)  
                .HasForeignKey(g => g.LeadId);

            modelBuilder.Entity<LeadResponseModel>()
                .HasOne(lr => lr.LeadSources)       // Define the navigation property
                .WithMany(ls => ls.LeadResponses) // LeadSourceModel can have many LeadResponses
                .HasForeignKey(lr => lr.UMID)      // Foreign Key in LeadResponseModel
                .HasPrincipalKey(ls => ls.UMID);  // Principal Key in LeadSourceModel       

            base.OnModelCreating(modelBuilder);
        }
    }
}
