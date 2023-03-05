using Microsoft.EntityFrameworkCore;
using backend.Models;
public class TicTacContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cell>().HasData(
                    new Cell{Id = 1, CellNumber = 1, SessionId = 1},
                    new Cell{Id = 2, CellNumber = 2, SessionId = 1},
                    new Cell{Id = 3, CellNumber = 3, SessionId = 1},
                    new Cell{Id = 4, CellNumber = 4, SessionId = 1},
                    new Cell{Id = 5, CellNumber = 5, SessionId = 1},
                    new Cell{Id = 6, CellNumber = 6, SessionId = 1},
                    new Cell{Id = 7, CellNumber = 7, SessionId = 1},
                    new Cell{Id = 8, CellNumber = 8, SessionId = 1},
                    new Cell{Id = 9, CellNumber = 9, SessionId = 1}
                );
                
        modelBuilder.Entity<Session>().HasData(
            new Session{
                Id = 1,
            }
        );
        
        modelBuilder.Entity<Session>().HasMany(p=>p.Cells).WithOne(p=>p.Session);

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data source=fieldContext.db");
    }
    public DbSet<Session> Sessions {get; set;} = null!;
    public DbSet<Cell> Cells {get; set;} = null!;
}