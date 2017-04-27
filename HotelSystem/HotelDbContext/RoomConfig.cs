using System.Data.Entity.ModelConfiguration;
using HotelSystem.Model;

namespace HotelSystem.HotelDbContext
{
    public class RoomConfig : EntityTypeConfiguration<Room>
    {
        public RoomConfig()
        {
            HasKey(room => room.RoomId);
            Property(room => room.Number).IsRequired().HasMaxLength(5);
            Property(room => room.Type).IsRequired();

            ToTable("Rooms");
        }
    }
}