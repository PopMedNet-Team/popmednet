using Microsoft.EntityFrameworkCore;

namespace PopMedNet.DMCS.Data.Model
{
    public class ModelContext : DbContext
    {
        public ModelContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<AuthenticationLog> AuthenticationLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DataMart> DataMarts { get; set; }
        public DbSet<UserDataMart> UserDataMarts { get; set; }
        public DbSet<RequestDataMart> RequestDataMarts { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<RequestDocument> RequestDocuments { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(eb => 
            {
                eb.HasMany(u => u.AuthenticationLogs).WithOne(ul => ul.User).HasForeignKey(ul => ul.UserID).OnDelete(DeleteBehavior.Restrict);
                eb.HasMany(u => u.DataMarts).WithOne(ud => ud.User).HasForeignKey(ud => ud.UserID).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DataMart>(eb =>
            {
                eb.Property(dm => dm.Name).HasMaxLength(255);
                eb.Property(dm => dm.Acronym).HasMaxLength(100);
                eb.Property(p => p.PmnTimestamp).IsConcurrencyToken();
                eb.Ignore(p => ((IDataMartMetadata)p).Timestamp);

                eb.HasMany(dm => dm.Users).WithOne(udm => udm.DataMart).HasForeignKey(udm => udm.DataMartID).OnDelete(DeleteBehavior.Restrict);
                eb.HasMany(dm => dm.Requests).WithOne(rdm => rdm.DataMart).HasForeignKey(rdm => rdm.DataMartID).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UserDataMart>(eb => {
                eb.HasKey(udm => new { udm.UserID, udm.DataMartID});
            });

            modelBuilder.Entity<Request>(eb =>
            {
                eb.HasKey(r => r.ID);
                eb.HasMany(r => r.Routes).WithOne(r => r.Request).HasForeignKey(r => r.RequestID).OnDelete(DeleteBehavior.Restrict);
                eb.Property(p => p.PmnTimestamp).IsConcurrencyToken();
            });

            modelBuilder.Entity<RequestDataMart>(eb =>
            {
                eb.HasKey(rdm => rdm.ID);
                eb.HasMany(rdm => rdm.Responses).WithOne(r => r.RequestDataMart).HasForeignKey(r => r.RequestDataMartID).OnDelete(DeleteBehavior.Restrict);
                eb.Property(p => p.PmnTimestamp).IsConcurrencyToken();
            });

            modelBuilder.Entity<Response>(eb =>
            {
                eb.HasKey(r => r.ID);
                eb.HasMany(r => r.Documents).WithOne(rdoc => rdoc.Response).HasForeignKey(rdoc => rdoc.ResponseID).OnDelete(DeleteBehavior.Restrict);
                eb.Property(p => p.PmnTimestamp).IsConcurrencyToken();
            });

            modelBuilder.Entity<RequestDocument>(eb =>
            {
                eb.HasKey(p => new { p.ResponseID, p.RevisionSetID });
            });

            modelBuilder.Entity<Document>(eb =>
            {
                eb.HasKey(r => r.ID);
                eb.Property(p => p.ID).ValueGeneratedNever();
                eb.Property(p => p.ItemID).ValueGeneratedNever();
                eb.Property(p => p.RevisionSetID).ValueGeneratedNever();
                eb.HasOne(p => p.UploadedBy).WithMany().HasForeignKey(p => p.UploadedByID).OnDelete(DeleteBehavior.SetNull);
                eb.Property(p => p.PmnTimestamp).IsConcurrencyToken();
                eb.Property(p => p.Name).HasMaxLength(255);
                eb.Property(p => p.Kind).HasMaxLength(50);
                eb.Property(p => p.MimeType).HasMaxLength(100);
                eb.Property(p => p.Version).HasMaxLength(50).HasDefaultValue("1.0.0.0");
                eb.Property(p => p.CreatedOn).HasDefaultValueSql("GETUTCDATE()");
                eb.Property(p => p.ItemID).IsRequired();
                eb.Property(p => p.RevisionSetID).IsRequired();
                eb.HasIndex(p => p.ID).IncludeProperties(p => new { p.RevisionSetID, p.Version });

                
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
