using GalaSoft.MvvmLight;
using HotelSystem.HotelDbContext;

namespace HotelSystem.ViewModel
{
    public class RoomsTabViewModel : ViewModelBase
    {
        public HotelContext Context { get; }

        public RoomsTabViewModel(HotelContext context)
        {
            Context = context;
        }
    }
}