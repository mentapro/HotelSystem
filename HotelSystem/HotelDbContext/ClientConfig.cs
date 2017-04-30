using System.Data.Entity.ModelConfiguration;
using HotelSystem.Model;

namespace HotelSystem.HotelDbContext
{
    public class ClientConfig : EntityTypeConfiguration<Client>
    {
        public ClientConfig()
        {
            Property(client => client.Account).IsOptional().HasMaxLength(20);
            HasRequired(client => client.Room).WithMany(room => room.Clients).WillCascadeOnDelete(true);
            
            ToTable("Clients");
        }
    }
}