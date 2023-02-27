using Microsoft.EntityFrameworkCore;

namespace DirectoryStructure
{
    public class DirectoryContext : DbContext
    {
        public DirectoryContext(DbContextOptions<DirectoryContext> options ) 
            : base(options) { Database.EnsureCreated(); }

        public DbSet<Models.Directory> Directories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "DirectoryDatabase");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Directory>()
                .HasOne(d => d.Parent)
                .WithMany(d => d.Children)
                .HasForeignKey(d => d.ParentDirectoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Models.Directory>()
                .HasData(
                    new Models.Directory()
                    {
                        Id = 1,
                        Name = "Creating Digital Images",
                        ParentDirectoryId = null
                    },
                    new Models.Directory()
                    {
                        Id = 2,
                        Name = "Resources",
                        ParentDirectoryId = 1
                    },
                    new Models.Directory()
                    {
                        Id = 3,
                        Name = "Evidence",
                        ParentDirectoryId = 1
                    },
                    new Models.Directory()
                    {
                        Id = 4,
                        Name = "Graphic Products",
                        ParentDirectoryId = 1
                    },
                    new Models.Directory()
                    {
                        Id = 5,
                        Name = "Primary Sources",
                        ParentDirectoryId = 2
                    },
                    new Models.Directory()
                    {
                        Id = 6,
                        Name = "Secondary Sources",
                        ParentDirectoryId = 2
                    },
                    new Models.Directory()
                    {
                        Id = 7,
                        Name = "Process",
                        ParentDirectoryId = 4
                    },
                    new Models.Directory()
                    {
                        Id = 8,
                        Name = "Final Product",
                        ParentDirectoryId = 4
                    });
        }
    }
}
