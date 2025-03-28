using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OscarChatPlatform.Domain.Entities;
using System.Reflection.Emit;

namespace OscarChatPlatform.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Connection> Connections { get; set; }
    public DbSet<RandomChat> RandomChats { get; set; }
    public DbSet<StandardChat> StandardChats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<ChatRoomQueue> ChatRoomQueues { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configurazione relazione molti-a-molti (senza tabella di join esplicita)
        builder.Entity<RandomChat>()
            .HasMany(rc => rc.Users)
            .WithMany(u => u.RandomChats);

        // Configurazione relazione Message -> RandomChat (con shadow property)
        builder.Entity<Message>()
            .HasOne(m => m.RandomChat)
            .WithMany(rc => rc.Messages)
            .HasForeignKey("RandomChatId") // Shadow property
            .OnDelete(DeleteBehavior.ClientSetNull); // Evita cicli di cancellazione

        // Configurazione relazione TerminatedByUser (con shadow property)
        builder.Entity<RandomChat>()
            .HasOne(rc => rc.TerminatedByUser)
            .WithMany()
            .HasForeignKey("TerminatedByUserId") // Shadow property
            .OnDelete(DeleteBehavior.SetNull); // Imposta a NULL se l'utente viene eliminato
    }
}
