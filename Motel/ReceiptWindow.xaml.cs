using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for ReceiptWindow.xaml
    /// </summary>
    public partial class ReceiptWindow : Window
    {
        private ReservationVM _reservation;
        private DateTime _now;

        public ReceiptWindow(ReservationVM reservation)
        {
            InitializeComponent();
            _reservation = reservation;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var roomChargeManager = new RoomChargeManager();
            if(roomChargeManager.CheckForRoomCharges(_reservation.ReservationID))
            gridReceiptRoomChargeDisplay.ItemsSource = roomChargeManager.GetActiveRoomChargeItems(_reservation.RoomCharges.RoomChargeID);
            var dates = DateTime.Now - _reservation.CheckIn;
            if (dates.TotalDays != 0)
            {
                var price = _reservation.Room.RoomPrice * Math.Floor(dates.TotalDays);
                if (_reservation.RoomCharges.RoomChargeItems != null)
                {
                    foreach (var item in _reservation.RoomCharges.RoomChargeItems)
                    {
                        price += item.ItemPrice * item.ItemAmount;
                    }
                }
                Math.Floor(price);
                labelRoomType.Content += $" {_reservation.Room.RoomTypeID}";
                labelNightsStayed.Content += $" {Math.Floor(dates.TotalDays)}";
                labelTotalRoomPrice.Content += $" ${_reservation.Room.RoomPrice}";
                labelTotalReservationPrice.Content += $" ${price}";
            }
            else
            {
                var price = 0;
                if (_reservation.RoomCharges.RoomChargeItems != null)
                {
                    if(_reservation.RoomCharges.RoomChargeItems.Count() > 0)
                    {
                        foreach (var item in _reservation.RoomCharges.RoomChargeItems)
                        {
                            price += item.ItemPrice * item.ItemAmount;
                        }
                    }
                }
                labelRoomType.Content += $" {_reservation.Room.RoomTypeID}";
                labelNightsStayed.Content += $" {dates.TotalDays}";
                labelTotalRoomPrice.Content += $" ${_reservation.Room.RoomPrice}";
                labelTotalReservationPrice.Content += $" ${price}";
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            var userChoice = MessageBox.Show("Are you sure you want to save the current changes", "Save confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if(userChoice == MessageBoxResult.OK)
            {
                this.DialogResult = true;
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            var userChoice = MessageBox.Show("Are you sure you want to discard the current changes", "Cancel confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (userChoice == MessageBoxResult.OK)
            {
                this.DialogResult = false;
            }
        }
    }
}
