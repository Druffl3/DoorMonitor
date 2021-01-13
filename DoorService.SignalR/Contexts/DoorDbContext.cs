using DoorService.SignalR.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoorService.SignalR.Contexts
{
    public class DoorDbContext : DbContext
    {
        public DoorDbContext(DbContextOptions<DoorDbContext> options) : base(options) { }

        /// <summary>
        /// Database set of the Door entity
        /// </summary>
        public DbSet<Door> DoorTable { get; set; }
    }
}
