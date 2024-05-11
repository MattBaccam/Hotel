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
    /// Interaction logic for GuestsWindow.xaml
    /// </summary>
    public partial class GuestsWindow : Window
    {
        public Guest _guest;
        private List<Guest> _guests;
        public GuestsWindow(List<Guest> guests)
        {
            InitializeComponent();
            _guests = guests;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gridRoomAvailableGuests.ItemsSource = _guests;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if(_guest != null)
            {
                this.DialogResult = true;
            }
            else
            {
                var userChoice = MessageBox.Show("Are you sure you want to exit window without a guest selected?", "Cancel guest selection", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if(userChoice == MessageBoxResult.OK)
                {
                    this.DialogResult = false;
                }
            }
        }

        private void gridRoomAvailableGuests_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _guest = gridRoomAvailableGuests.SelectedItem as Guest;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_guest != null)
            {
                var userChoice = MessageBox.Show("Are you sure you want to exit window with a guest selected?", "Cancel guest selection", MessageBoxButton.OKCancel, MessageBoxImage.Question);
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
    }
}
