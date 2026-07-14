using Microsoft.EntityFrameworkCore;
using ProgramInformationV2.Data.DataModels;
using System.Diagnostics;

namespace ProgramInformationV2.Data.DataContext {

    public class ProgramContext : DbContext {
        public ProgramContext() : base() {
            Debug.WriteLine($"Context created.");
        }

        public ProgramContext(DbContextOptions<ProgramContext> options) : base(options) {
            Debug.WriteLine($"Context created.");
        }

        public DbSet<CourseImportEntry> CourseImportEntries { get; set; }
        public DbSet<FacultyName> FacultyNames { get; set; }
        public DbSet<FieldSource> FieldSources { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<NoteTemplate> NoteTemplates { get; set; }
        public DbSet<SecurityEntry> SecurityEntries { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<TagSource> TagSources { get; set; }

        public override void Dispose() {
            Debug.WriteLine($"Context disposed.");
            base.Dispose();
        }

        public override ValueTask DisposeAsync() {
            Debug.WriteLine($"Context disposed async.");
            return base.DisposeAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            Debug.WriteLine($"Context starting initial setup.");
            Debug.WriteLine($"Context finishing initial setup.");
        }
    }
}