using System;
using System.Data.Entity;
using System.Windows;
using HotelSystem.HotelDbContext;
using HotelSystem.Model;
using HotelSystem.ViewModel;

namespace HotelSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HotelContext Context { get; }

        public MainWindow()
        {
            InitializeComponent();
            RoomTypeCb.ItemsSource = RtCbFilter.ItemsSource = Enum.GetNames(typeof(RoomTypes));

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<HotelContext>());
            //Database.SetInitializer(new DropCreateDatabaseAlways<HotelContext>()); // set it if you want to recreate database
            Context = new HotelContext();
            //Fill(); // uncomment if you want to fill database with default values
            ClientsTab.DataContext = new ClientsTabViewModel(Context);
            RoomsTab.DataContext = new RoomsTabViewModel(Context);
        }

        private void Fill()
        {
            var rooms = new[]
            {
                new Room {Number = "1", Type = RoomTypes.StandardRoom},
                new Room {Number = "2", Type = RoomTypes.JuniorSuite},
                new Room {Number = "3", Type = RoomTypes.StandardRoom},
                new Room {Number = "4", Type = RoomTypes.PresidentialSuite},
                new Room {Number = "5", Type = RoomTypes.JuniorSuite},
                new Room {Number = "6", Type = RoomTypes.StandardRoom},
                new Room {Number = "7", Type = RoomTypes.PresidentialSuite}
            };

            var clients = new[]
            {
                new Client {FirstName = "Stanislav", LastName = "Herasymiuk", Birthdate = new DateTime(1995, 9, 2), Account = "stas_the_best", Room = rooms[1]},
                new Client {FirstName = "Bob", LastName = "Marley", Birthdate = new DateTime(1952, 3, 25), Account = "919191", Room = rooms[3]},
                new Client {FirstName = "Frank", LastName = "Sinatra", Birthdate = new DateTime(1957, 7, 3), Account = "100500", Room = rooms[3]},
                new Client {FirstName = "Phill", LastName = "Colson", Birthdate = new DateTime(1966, 12, 6), Account = "S.H.I.E.L.D.", Room = rooms[5]},
                new Client {FirstName = "Dayzee", LastName = "Skay", Birthdate = new DateTime(1989, 10, 30), Account = "Hydra", Room = rooms[4]},
                new Client {FirstName = "Elvis", LastName = "Presley", Birthdate = new DateTime(1960, 2, 17), Account = "YA krevedko", Room = rooms[6]}
            };

            Context.Rooms.AddRange(rooms);
            Context.Clients.AddRange(clients);
            Context.SaveChanges();
        }
    }
}