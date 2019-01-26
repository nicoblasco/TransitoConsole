namespace ConsoleTransito
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("name=ApplicationDbContext")
        {
        }

        public virtual DbSet<CallCenterTurns> CallCenterTurns { get; set; }
        public virtual DbSet<Processes> Processes { get; set; }
        public virtual DbSet<ProcessLogs> ProcessLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Processes>()
                .HasMany(e => e.ProcessLogs)
                .WithRequired(e => e.Processes)
                .HasForeignKey(e => e.ProcessId)
                .WillCascadeOnDelete(false);
        }
    }
}
