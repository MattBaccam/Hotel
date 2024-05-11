using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScenicHotel
{
    /// <summary>
    /// Interaction logic for ViewRoomsAvailable.xaml
    /// </summary>
    public partial class ViewRoomsAvailable : Window
    {
        public RoomVM _selectedRoom = null;
        private List<RoomVM> _rooms = null;
        public ViewRoomsAvailable(List<RoomVM> rooms)
        {
            InitializeComponent();
            _rooms = rooms;
            gridRoomAvailableDisplay.ItemsSource = _rooms;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedRoom != null)
            {
                var userChoice = MessageBox.Show($"Are you sure you want to select the current room {_selectedRoom.RoomTypeID}", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                if (userChoice == MessageBoxResult.OK)
                {
                    this.DialogResult = true;
                }
            }
            else
            {
                MessageBox.Show("You need to select something in order to save it");
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedRoom != null)
            {
                var userChoice = MessageBox.Show("Are you sure you want to exit this window", "Cancel", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                if (userChoice == MessageBoxResult.OK)
                {
                    this.DialogResult = false;
                }
            }
            else
            {
                this.DialogResult = false;
            }
        }

        private void gridRoomAvailableDisplay_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
             _selectedRoom = gridRoomAvailableDisplay.SelectedItem as RoomVM;
        }
    }
}
