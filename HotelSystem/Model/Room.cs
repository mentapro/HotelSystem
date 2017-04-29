using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using HotelSystem.Annotations;

namespace HotelSystem.Model
{
    public enum RoomTypes
    {
        None,
        StandardRoom,
        BusinessClassRoom,
        JuniorSuite,
        PresidentialSuite
    }

    public class Room : INotifyPropertyChanged
    {
        private RoomTypes _type;
        private string _number;
        private int _roomId;
        private IList<Client> _clients;

        public int RoomId
        {
            get => _roomId;
            set
            {
                if (value == _roomId) return;
                _roomId = value;
                OnPropertyChanged();
            }
        }

        public string Number
        {
            get => _number;
            set
            {
                if (value == _number) return;
                _number = value;
                OnPropertyChanged();
            }
        }

        public RoomTypes Type
        {
            get => _type;
            set
            {
                if (value == _type) return;
                _type = value;
                OnPropertyChanged();
            }
        }

        public virtual IList<Client> Clients
        {
            get => _clients;
            set
            {
                if (Equals(value, _clients)) return;
                _clients = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Room()
        {
            _clients = new List<Client>();
        }
    }
}