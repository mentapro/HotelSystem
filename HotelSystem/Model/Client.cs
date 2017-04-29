namespace HotelSystem.Model
{
    public class Client : Person
    {
        private string _account;
        private Room _room;

        public string Account
        {
            get => _account;
            set
            {
                if (value == _account) return;
                _account = value;
                OnPropertyChanged();
            }
        }

        public virtual Room Room
        {
            get => _room;
            set
            {
                if (Equals(value, _room)) return;
                _room = value;
                OnPropertyChanged();
            }
        }
    }
}