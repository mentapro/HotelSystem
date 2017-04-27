using System.Data.Entity;
using HotelSystem.Model;

namespace HotelSystem.HotelDbContext
{
    public class HotelContext : DbContext
    {
        public HotelContext()
        {
            
        }

        public HotelContext(string connectionString)
            : base(connectionString)
        {
            
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new PersonConfig());
            modelBuilder.Configurations.Add(new ClientConfig());
            modelBuilder.Configurations.Add(new RoomConfig());
        }
    }
}