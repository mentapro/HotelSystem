using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using HotelSystem.Model;

namespace HotelSystem.Miscellaneous
{
    public class RoomTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (RoomTypes) value;
        }
    }

    public class ResetFilterClientParametersConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Tuple<TextBox, TextBox, DatePicker, TextBox, ComboBox>(values[0] as TextBox,
                values[1] as TextBox, values[2] as DatePicker, values[3] as TextBox, values[4] as ComboBox);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Can not convert target to Command Parameter");
        }
    }

    public class ResetFilterRoomParametersConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return new Tuple<TextBox, ComboBox, ComboBox>(values[0] as TextBox, values[1] as ComboBox, values[2] as ComboBox);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Can not convert target to Command Parameter");
        }
    }
}