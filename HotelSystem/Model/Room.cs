using System.ComponentModel;
using System.Runtime.CompilerServices;
using HotelSystem.Annotations;

namespace HotelSystem.Model
{
    public enum RoomTypes
    {
        StandardRoom = 0,
        BusinessClassRoom = 1,
        JuniorSuite = 2,
        PresidentialSuite = 3
    }

    public class Room : INotifyPropertyChanged
    {
        private RoomTypes _type;
        private string _number;
        private int _roomId;

        public int RoomId
        {
            get { return _roomId; }
            set
            {
                if (value == _roomId) return;
                _roomId = value;
                OnPropertyChanged();
            }
        }

        public string Number
        {
            get { return _number; }
            set
            {
                if (value == _number) return;
                _number = value;
                OnPropertyChanged();
            }
        }

        public RoomTypes Type
        {
            get { return _type; }
            set
            {
                if (value == _type) return;
                _type = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}