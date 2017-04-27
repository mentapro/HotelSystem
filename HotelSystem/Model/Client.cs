namespace HotelSystem.Model
{
    public class Client : Person
    {
        private string _account;

        public string Account
        {
            get { return _account; }
            set
            {
                if (value == _account) return;
                _account = value;
                OnPropertyChanged();
            }
        }
    }
}