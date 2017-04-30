using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HotelSystem.HotelDbContext;
using HotelSystem.Model;

namespace HotelSystem.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private Room _selectedRoom;

        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value; 
                RaisePropertyChanged();
            }
        }

        public Client SelectedClient { get; set; }
        
        public Room RoomInfo { get; set; } = new Room();
        public Room RoomFilter { get; set; } = new Room();
        public int RoomFreedomFilterIndex { get; set; }
        public Client ClientInfo { get; set; } = new Client();
        public Client ClientFilter { get; set; } = new Client();

        private IList<Client> _filteredClientList;

        public IList<Client> FilteredClientList
        {
            get => _filteredClientList;
            set
            {
                _filteredClientList = value;
                RaisePropertyChanged();
            }
        }

        private IList<Room> _filteredRoomList;

        public IList<Room> FilteredRoomList
        {
            get => _filteredRoomList;
            set
            {
                _filteredRoomList = value;
                RaisePropertyChanged();
            }
        }

        private HotelContext _context;

        public HotelContext Context
        {
            get => _context ?? (_context = new HotelContext());
            set => _context = value;
        }
        
        #region RoomCommands

        private RelayCommand _addRoomCommand;

        public ICommand AddRoomCommand =>
            _addRoomCommand ??
            (_addRoomCommand = new RelayCommand(
                () =>
                {
                    Context.Rooms.Add(new Room
                    {
                        Number = RoomInfo.Number,
                        Type = RoomInfo.Type
                    });
                    Context.SaveChanges();
                },
                () =>
                {
                    if (string.IsNullOrEmpty(RoomInfo.Number) || RoomInfo.Type == RoomTypes.None)
                    {
                        return false;
                    }
                    return true;
                }));

        private RelayCommand _updateRoomCommand;

        public ICommand UpdateRoomCommand =>
            _updateRoomCommand ??
            (_updateRoomCommand = new RelayCommand(
                () =>
                {
                    SelectedRoom.Number = RoomInfo.Number;
                    SelectedRoom.Type = RoomInfo.Type;
                    Context.SaveChanges();
                },
                () =>
                {
                    if (SelectedRoom == null) return false;
                    if (string.IsNullOrEmpty(RoomInfo.Number) || RoomInfo.Type == RoomTypes.None)
                    {
                        return false;
                    }
                    return true;
                }));

        private RelayCommand _deleteRoomCommand;

        public ICommand DeleteRoomCommand =>
            _deleteRoomCommand ??
            (_deleteRoomCommand = new RelayCommand(
                () =>
                {
                    Context.Rooms.Remove(SelectedRoom);
                    Context.SaveChanges();
                },
                () => SelectedRoom != null));

        private RelayCommand<object> _resetFilterRoomCommand;
        
        public RelayCommand<object> ResetFilterRoomCommand =>
            _resetFilterRoomCommand ??
            (_resetFilterRoomCommand = new RelayCommand<object>(
                parameters =>
                {
                    var tuple = parameters as Tuple<TextBox, ComboBox, ComboBox>;
                    tuple.Item1.Text = string.Empty;
                    tuple.Item2.SelectedIndex = 0;
                    tuple.Item3.SelectedIndex = 0;
                },
                parameters =>
                {
                    if (parameters == null) return false;
                    var tuple = parameters as Tuple<TextBox, ComboBox, ComboBox>;
                    if (string.IsNullOrEmpty(tuple.Item1.Text)
                        && (tuple.Item2 == null || new List<int> {-1, 0}.IndexOf(tuple.Item2.SelectedIndex) != -1)
                        && (tuple.Item3 == null || new List<int> {-1, 0}.IndexOf(tuple.Item3.SelectedIndex) != -1))
                    {
                        return false;
                    }
                    return true;
                }));

        private RelayCommand<Room> _roomsGridSelectionChangedCommand;

        public RelayCommand<Room> RoomsGridSelectionChangedCommand =>
            _roomsGridSelectionChangedCommand ??
            (_roomsGridSelectionChangedCommand =
                new RelayCommand<Room>(
                    room =>
                    {
                        RoomInfo.Number = room.Number;
                        RoomInfo.Type = room.Type;
                    },
                    room => room != null));

        private RelayCommand _roomsFilterChangedCommand;

        public RelayCommand RoomsFilterChangedCommand =>
            _roomsFilterChangedCommand ?? (_roomsFilterChangedCommand =
                new RelayCommand(() =>
                {
                    IEnumerable<Room> queryResult = Context.Rooms.Local;
                    if (!string.IsNullOrEmpty(RoomFilter.Number))
                    {
                        queryResult = queryResult.Where(room => room.Number.Contains(RoomFilter.Number));
                    }
                    if (RoomFilter.Type != RoomTypes.None)
                    {
                        queryResult = queryResult.Where(room => room.Type == RoomFilter.Type);
                    }
                    if (RoomFreedomFilterIndex == 1)
                    {
                        queryResult = queryResult.Where(room => room.Clients.Count == 0);
                    }
                    if (RoomFreedomFilterIndex == 2)
                    {
                        queryResult = queryResult.Where(room => room.Clients.Count > 0);
                    }
                    FilteredRoomList = queryResult?.ToList();
                }));

        #endregion

        #region ClientCommands

        private RelayCommand _addClientCommand;

        public ICommand AddClientCommand =>
            _addClientCommand ??
            (_addClientCommand = new RelayCommand(
                () =>
                {
                    Context.Clients.Add(new Client
                    {
                        FirstName = ClientInfo.FirstName,
                        LastName = ClientInfo.LastName,
                        Birthdate = ClientInfo.Birthdate,
                        Account = ClientInfo.Account,
                        Room = ClientInfo.Room
                    });
                    Context.SaveChanges();
                },
                () =>
                {
                    if (string.IsNullOrEmpty(ClientInfo.FirstName)
                        || string.IsNullOrEmpty(ClientInfo.LastName)
                        || ClientInfo.Room == null)
                    {
                        return false;
                    }
                    return true;
                }));

        private RelayCommand _updateClientCommand;

        public ICommand UpdateClientCommand =>
            _updateClientCommand ??
            (_updateClientCommand = new RelayCommand(
                () =>
                {
                    SelectedClient.FirstName = ClientInfo.FirstName;
                    SelectedClient.LastName = ClientInfo.LastName;
                    SelectedClient.Birthdate = ClientInfo.Birthdate;
                    SelectedClient.Account = ClientInfo.Account;
                    SelectedClient.Room = ClientInfo.Room;
                    Context.SaveChanges();
                },
                () =>
                {
                    if (SelectedClient == null) return false;
                    if (string.IsNullOrEmpty(ClientInfo.FirstName)
                        || string.IsNullOrEmpty(ClientInfo.LastName)
                        || ClientInfo.Room == null)
                    {
                        return false;
                    }
                    return true;
                }));

        private RelayCommand _deleteClientCommand;

        public ICommand DeleteClientCommand =>
            _deleteClientCommand ??
            (_deleteClientCommand = new RelayCommand(
                () =>
                {
                    Context.Clients.Remove(SelectedClient);
                    Context.SaveChanges();
                },
                () => SelectedClient != null));

        private RelayCommand<object> _resetFilterClientCommand;
        
        public RelayCommand<object> ResetFilterClientCommand =>
            _resetFilterClientCommand ??
            (_resetFilterClientCommand = new RelayCommand<object>(
                parameters =>
                {
                    var tuple = parameters as Tuple<TextBox, TextBox, DatePicker, TextBox, ComboBox>;
                    tuple.Item1.Text = string.Empty;
                    tuple.Item2.Text = string.Empty;
                    tuple.Item3.SelectedDate = null;
                    tuple.Item4.Text = string.Empty;
                    tuple.Item5.SelectedIndex = -1;
                },
                parameters =>
                {
                    if (parameters == null) return false;
                    var tuple = parameters as Tuple<TextBox, TextBox, DatePicker, TextBox, ComboBox>;
                    if (string.IsNullOrEmpty(tuple.Item1.Text)
                        && string.IsNullOrEmpty(tuple.Item2.Text)
                        && tuple.Item3.SelectedDate == null
                        && string.IsNullOrEmpty(tuple.Item4.Text)
                        && (tuple.Item5 == null || tuple.Item5.SelectedIndex == -1))
                    {
                        return false;
                    }
                    return true;
                }));

        private RelayCommand<Client> _clientsGridSelectionChangedCommand;

        public RelayCommand<Client> ClientsGridSelectionChangedCommand =>
            _clientsGridSelectionChangedCommand ??
            (_clientsGridSelectionChangedCommand =
                new RelayCommand<Client>(
                    client =>
                    {
                        ClientInfo.FirstName = client.FirstName;
                        ClientInfo.LastName = client.LastName;
                        ClientInfo.Birthdate = client.Birthdate;
                        ClientInfo.Account = client.Account;
                        ClientInfo.Room = client.Room;
                    },
                    client => client != null));

        private RelayCommand _clientsFilterChangedCommand;
        
        public RelayCommand ClientsFilterChangedCommand =>
            _clientsFilterChangedCommand ?? (_clientsFilterChangedCommand =
                new RelayCommand(() =>
                {
                    IEnumerable<Client> queryResult = Context.Clients.Local;
                    if (!string.IsNullOrEmpty(ClientFilter.FirstName))
                    {
                        queryResult = queryResult.Where(client => client.FirstName.Contains(ClientFilter.FirstName));
                    }
                    if (!string.IsNullOrEmpty(ClientFilter.LastName))
                    {
                        queryResult = queryResult.Where(client => client.LastName.Contains(ClientFilter.LastName));
                    }
                    if (ClientFilter.Birthdate != null)
                    {
                        queryResult = queryResult.Where(client => client.Birthdate == ClientFilter.Birthdate);
                    }
                    if (!string.IsNullOrEmpty(ClientFilter.Account))
                    {
                        queryResult = queryResult.Where(client => client.Account.Contains(ClientFilter.Account));
                    }
                    if (ClientFilter.Room != null)
                    {
                        queryResult = queryResult.Where(client => client.Room == ClientFilter.Room);
                    }
                    FilteredClientList = queryResult?.ToList();
                }));

        #endregion

        public MainWindowViewModel()
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<HotelContext>());
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<HotelContext>());
            Context.Clients.Load();
            Context.Rooms.Load();
            //Fill();
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
                new Client {FirstName = "Stanislav", LastName = "Herasymiuk", Account = "stas_the_best", Room = rooms[1]},
                new Client {FirstName = "Bob", LastName = "Marley", Account = "919191", Room = rooms[3]},
                new Client {FirstName = "Frank", LastName = "Sinatra", Account = "100500", Room = rooms[3]},
                new Client {FirstName = "Phill", LastName = "Colson", Account = "S.H.I.E.L.D.", Room = rooms[5]},
                new Client {FirstName = "Dayzee", LastName = "Skay", Account = "Hydra", Room = rooms[4]},
                new Client {FirstName = "Elvis", LastName = "Presley", Account = "YA krevedko", Room = rooms[6]}
            };

            Context.Rooms.AddRange(rooms);
            Context.Clients.AddRange(clients);
            Context.SaveChanges();
        }
    }
}