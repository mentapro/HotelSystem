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
    public class RoomsTabViewModel : ViewModelBase
    {
        private Room _selectedRoom;
        private IList<Room> _filteredRoomList;

        public HotelContext Context { get; }
        public Room RoomInfo { get; set; } = new Room();
        public Room RoomFilter { get; set; } = new Room();
        public int RoomFreedomFilterIndex { get; set; }

        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                RaisePropertyChanged();
            }
        }

        public IList<Room> FilteredRoomList
        {
            get => _filteredRoomList;
            set
            {
                _filteredRoomList = value;
                RaisePropertyChanged();
            }
        }

        public RoomsTabViewModel(HotelContext context)
        {
            Context = context;
            Context.Rooms.Load();
        }
        
        #region Commands

        private RelayCommand _addRoomCommand;
        private RelayCommand _updateRoomCommand;
        private RelayCommand _deleteRoomCommand;
        private RelayCommand<object> _resetFilterRoomCommand;
        private RelayCommand _roomsGridSelectionChangedCommand;
        private RelayCommand _roomsFilterChangedCommand;

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

        public ICommand DeleteRoomCommand =>
            _deleteRoomCommand ??
            (_deleteRoomCommand = new RelayCommand(
                () =>
                {
                    Context.Rooms.Remove(SelectedRoom);
                    Context.SaveChanges();
                },
                () => SelectedRoom != null));

        public RelayCommand<object> ResetFilterRoomCommand =>
            _resetFilterRoomCommand ??
            (_resetFilterRoomCommand = new RelayCommand<object>(
                parameters =>
                {
                    if (parameters is Tuple<TextBox, ComboBox, ComboBox> tuple)
                    {
                        tuple.Item1.Text = string.Empty;
                        tuple.Item2.SelectedIndex = 0;
                        tuple.Item3.SelectedIndex = 0;
                    }
                },
                parameters =>
                {
                    if (parameters == null) return false;
                    if (parameters is Tuple<TextBox, ComboBox, ComboBox> tuple)
                    {
                        if (string.IsNullOrEmpty(tuple.Item1.Text)
                            && (tuple.Item2 == null || new List<int> {-1, 0}.IndexOf(tuple.Item2.SelectedIndex) != -1)
                            && (tuple.Item3 == null || new List<int> {-1, 0}.IndexOf(tuple.Item3.SelectedIndex) != -1))
                            return false;
                        return true;
                    }
                    return false;
                }));

        public ICommand RoomsGridSelectionChangedCommand =>
            _roomsGridSelectionChangedCommand ??
            (_roomsGridSelectionChangedCommand =
                new RelayCommand(
                    () =>
                    {
                        RoomInfo.Number = SelectedRoom.Number;
                        RoomInfo.Type = SelectedRoom.Type;
                    },
                    () => SelectedRoom != null));

        public ICommand RoomsFilterChangedCommand =>
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
    }
}