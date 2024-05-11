using DataObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for RoomChargeWindow.xaml
    /// </summary>
    public partial class RoomChargeWindow : Window
    {
        private ReservationVM _reservation;
        private RoomChargeItemVM _selectedRoomCharge = null;
        private int _loggedInUserID;
        public RoomChargeWindow(ReservationVM reservation, int loggedInUserID)
        {
            InitializeComponent();
            _reservation = reservation;
            _loggedInUserID = loggedInUserID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_reservation.RoomCharges != null)
            {
                var roomChargeManager = new RoomChargeManager();
                gridRoomChargeDisplay.ItemsSource = roomChargeManager.GetActiveRoomChargeItems(_reservation.RoomCharges.RoomChargeID);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedRoomCharge != null)
            {
                var userChoice = MessageBox.Show($"Are you sure you want to refund the guest for {_selectedRoomCharge.PantryID}: ${_selectedRoomCharge.ItemPrice}");
                if(userChoice == MessageBoxResult.OK)
                {
                    try
                    {
                        var roomChargeManager = new RoomChargeManager();
                        var roomChargeList = new List<RoomChargeItemVM>();
                        if (roomChargeManager.RefundRoomCharge(_selectedRoomCharge.RoomChargeItemID, _selectedRoomCharge.RoomChargeID))
                        {
                            try
                            {
                                roomChargeList = roomChargeManager.GetActiveRoomChargeItems(_reservation.RoomCharges.RoomChargeID);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("No room charges", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            gridRoomChargeDisplay.ItemsSource = roomChargeList;
                            try
                            {
                                if (roomChargeManager.CreateSalesLog(new SalesLog()
                                {
                                    EmployeeID = _loggedInUserID,
                                    GuestID = _reservation.GuestID,
                                    TimeOfSale = DateTime.Now,
                                    PantryID = _selectedRoomCharge.PantryID,
                                    SoldPrice = _selectedRoomCharge.ItemPrice * -1,
                                    ItemAmount = _selectedRoomCharge.ItemAmount
                                }))
                                {
                                    MessageBox.Show("Successfully logged refund and removed it from guests receipt", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Failed to log refund in system", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Failed to mark item in receipt as refunded", "Failed to remove item from receipt", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to mark item in receipt as refunded", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("No sale selected", "Need sale selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void gridRoomChargeDisplay_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (gridRoomChargeDisplay.SelectedItem != null)
            {
                _selectedRoomCharge = gridRoomChargeDisplay.SelectedItem as RoomChargeItemVM;
            }
        }
    }
}
