using System;
using System.Windows;
using HotelSystem.Model;

namespace HotelSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RoomTypeCb.ItemsSource = Enum.GetNames(typeof(RoomTypes));
        }
    }
}