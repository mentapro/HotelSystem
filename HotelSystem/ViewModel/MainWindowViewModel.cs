using System.Data.Entity;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HotelSystem.HotelDbContext;
using HotelSystem.Model;

namespace HotelSystem.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public Room SelectedRoom { get; set; }

        private string _currentRoomNumberText;

        public string CurrentRoomNumberText
        {
            get => _currentRoomNumberText;
            set
            {
                _currentRoomNumberText = value;
                RaisePropertyChanged();
            }
        }

        private int _currentRoomTypeIndex;

        public int CurrentRoomTypeIndex
        {
            get => _currentRoomTypeIndex;
            set
            {
                _currentRoomTypeIndex = value;
                RaisePropertyChanged();
            }
        }

        private HotelContext _context;

        public HotelContext Context
        {
            get => _context ?? (_context = new HotelContext("HotelDbConnectionString"));
            set
            {
                _context = value;
                RaisePropertyChanged();
            }
        }

        private RelayCommand _addRoomCommand;

        public ICommand AddRoomCommand => _addRoomCommand ??
                                          (_addRoomCommand = new RelayCommand(
                                              () => // Execute
                                              {
                                                  Context.Rooms.Add(new Room
                                                  {
                                                      Number = CurrentRoomNumberText,
                                                      Type = (RoomTypes)CurrentRoomTypeIndex
                                                  });
                                                  Context.SaveChanges();
                                              },
                                              () => // CanExecute
                                              {
                                                  if (string.IsNullOrEmpty(CurrentRoomNumberText) ||
                                                      CurrentRoomTypeIndex == -1)
                                                  {
                                                      return false;
                                                  }
                                                  return true;
                                              }));

        private RelayCommand _updateRoomCommand;

        public ICommand UpdateRoomCommand => _updateRoomCommand ??
                                          (_updateRoomCommand = new RelayCommand(
                                              () => // Execute
                                              {
                                                  SelectedRoom.Number = CurrentRoomNumberText;
                                                  SelectedRoom.Type = (RoomTypes) CurrentRoomTypeIndex;
                                                  Context.SaveChanges();
                                              },
                                              () => // CanExecute
                                              {
                                                  if (string.IsNullOrEmpty(CurrentRoomNumberText) ||
                                                      CurrentRoomTypeIndex == -1)
                                                  {
                                                      return false;
                                                  }
                                                  return true;
                                              }));

        private RelayCommand _deleteRoomCommand;

        public ICommand DeleteRoomCommand => _deleteRoomCommand ??
                                          (_deleteRoomCommand = new RelayCommand(
                                              () => // Execute
                                              {
                                                  Context.Rooms.Remove(SelectedRoom);
                                                  Context.SaveChanges();
                                              },
                                              () => // CanExecute
                                                  SelectedRoom != null));

        private RelayCommand<Room> _roomsGridSelectionChangedCommand;

        public RelayCommand<Room> RoomsGridSelectionChangedCommand => _roomsGridSelectionChangedCommand ??
                                                                      (_roomsGridSelectionChangedCommand =
                                                                          new RelayCommand<Room>(
                                                                              room => // Execute
                                                                              {
                                                                                  CurrentRoomNumberText = room.Number;
                                                                                  CurrentRoomTypeIndex = (int) room.Type;
                                                                              },
                                                                              room => // CanExecute
                                                                                  room != null));

        public MainWindowViewModel()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<HotelContext>());
            Context.Clients.Load();
            Context.Rooms.Load();
        }
    }
}