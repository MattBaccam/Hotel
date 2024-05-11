using DataObjects;
using LogicLayer;
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
    /// Interaction logic for ReservationFormWindow.xaml
    /// </summary>
    public partial class ReservationFormWindow : Window
    {
        private RoomManager _roomManager = new RoomManager();
        private ReservationManager _reservationManager = new ReservationManager();
        private GuestManager _guestManager = new GuestManager();
        private Guest _selectedGuest = null;
        public ReservationFormWindow()
        {
            InitializeComponent();
        }

        private void buttonCheckForFirst_Click(object sender, RoutedEventArgs e)
        {
            if(textBoxFirstName.Text != string.Empty)
            {
                try
                {
                    var guestList = _guestManager.GetGuestByFirstName(textBoxFirstName.Text);
                    if(guestList != null)
                    {
                        var userChoice = MessageBox.Show("Similar guest found", "Guest found", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                        if(userChoice == MessageBoxResult.OK)
                        {
                            var guestSearchWindow = new GuestsWindow(guestList);
                            guestSearchWindow.ShowDialog();
                            if (guestSearchWindow.DialogResult == true)
                            {
                                _selectedGuest = guestSearchWindow._guest;
                                textBoxFirstName.Text = _selectedGuest.FirstName;
                                textBoxLastName.Text = _selectedGuest.LastName;
                                textBoxPhone.Text = _selectedGuest.Phone;
                                textBoxEmail.Text = _selectedGuest.Email;
                            }
                            else
                            {
                                MessageBox.Show("Canceled guest selection", "Canceled guest selection", MessageBoxButton.OK);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not guests under given fields", ex.ToString());
                }
            }
        }

        private void buttonCheckForFirstAndLast_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxFirstName.Text != string.Empty)
            {
                try
                {
                    var guestList = _guestManager.GetGuestByFirstNameLastName(textBoxFirstName.Text, textBoxLastName.Text);
                    if (guestList != null)
                    {
                        var userChoice = MessageBox.Show("Similar guest found", "Guest found", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                        if (userChoice == MessageBoxResult.OK)
                        {
                            var guestSearchWindow = new GuestsWindow(guestList);
                            guestSearchWindow.ShowDialog();
                            if (guestSearchWindow.DialogResult == true)
                            {
                                _selectedGuest = guestSearchWindow._guest;
                                textBoxFirstName.Text = _selectedGuest.FirstName;
                                textBoxLastName.Text = _selectedGuest.LastName;
                                textBoxPhone.Text = _selectedGuest.Phone;
                                textBoxEmail.Text = _selectedGuest.Email;
                            }
                            else
                            {
                                MessageBox.Show("Canceled guest selection", "Canceled guest selection", MessageBoxButton.OK);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not guests under given fields", ex.ToString());
                }
            }
        }

        private void textBoxFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxFirstName.Text != string.Empty && textBoxLastName.Text == string.Empty)
            {
                try
                {
                    var guestList = _guestManager.GetGuestByFirstName(textBoxFirstName.Text);
                    if (guestList != null)
                    {
                        var userChoice = MessageBox.Show("Similar guest found", "Guest found", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                        if (userChoice == MessageBoxResult.OK)
                        {
                            var guestSearchWindow = new GuestsWindow(guestList);
                            guestSearchWindow.ShowDialog();
                            if (guestSearchWindow.DialogResult == true)
                            {
                                _selectedGuest = guestSearchWindow._guest;
                                textBoxFirstName.Text = _selectedGuest.FirstName;
                                textBoxLastName.Text = _selectedGuest.LastName;
                                textBoxPhone.Text = _selectedGuest.Phone;
                                textBoxEmail.Text = _selectedGuest.Email;
                            }
                            else
                            {
                                MessageBox.Show("Canceled guest selection", "Canceled guest selection", MessageBoxButton.OK);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not guests under given fields \nCreate new guest", ex.ToString());
                    return;
                }
            }
        }

        private void buttonCheckInCalendar_Click(object sender, RoutedEventArgs e)
        {
            var calendarWindow = new CalendarWindow(DateTime.Now.Date);
            calendarWindow.ShowDialog();
            if(calendarWindow.DialogResult == true && DateTime.Now.Date <= DateTime.Parse(calendarWindow._date.ToString()).Date)
            {
                if (textBoxCheckOut.Text == "")
                {
                    textBoxCheckIn.Text = DateTime.Parse(calendarWindow._date.ToString()).ToShortDateString();
                    textBoxNights.Text = "1";
                    textBoxCheckOut.Text = DateTime.Parse(calendarWindow._date.ToString()).AddDays(1).ToShortDateString();
                }
                else
                {
                        textBoxCheckIn.Text = DateTime.Parse(calendarWindow._date.ToString()).ToShortDateString();
                        textBoxNights.Text = DateTime.Parse(calendarWindow._date.ToString()).Subtract(DateTime.Parse(textBoxCheckOut.Text)).TotalDays.ToString();
                }
            }
            else
            {
                MessageBox.Show("Canceled room selection phase", "Canceled room selection", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCheckCheckOutCalendar_Click(object sender, RoutedEventArgs e)
        {
            var calendarWindow = new CalendarWindow(DateTime.Now.Date);
            calendarWindow.ShowDialog();
            if (calendarWindow.DialogResult == true)
            {
                if (DateTime.Parse(calendarWindow._date.ToString()).Date < DateTime.Now.Date || DateTime.Parse(calendarWindow._date.ToString()).Date < DateTime.Parse(textBoxCheckIn.Text).Date)
                {
                    MessageBox.Show("Invalid check out date selected", "Date selection canceled", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    textBoxNights.Text = DateTime.Parse(calendarWindow._date.ToString()).Subtract(DateTime.Parse(textBoxCheckIn.Text)).TotalDays.ToString();
                    textBoxCheckOut.Text = DateTime.Parse(calendarWindow._date.ToString()).ToShortDateString();
                }
            }
        }

        private void textBoxNights_LostFocus(object sender, RoutedEventArgs e)
        {
            int outInt;
            var asd = int.TryParse(textBoxNights.Text, out outInt);
            if (int.TryParse(textBoxNights.Text, out outInt))
            {
                if (textBoxCheckIn.Text == "" && textBoxCheckOut.Text == "")
                {
                    textBoxCheckIn.Text = DateTime.Now.ToShortDateString();
                    textBoxCheckOut.Text = DateTime.Now.AddDays(Convert.ToInt32(textBoxNights.Text)).ToShortDateString();
                }
                else
                {
                    textBoxCheckIn.Text = DateTime.Parse(textBoxCheckIn.Text).ToShortDateString();
                    textBoxCheckOut.Text = DateTime.Parse(textBoxCheckIn.Text).AddDays(int.Parse(textBoxNights.Text)).ToShortDateString();
                }
            }
            else
            {
                if (textBoxCheckIn.Text == "" && textBoxCheckOut.Text == "")
                {
                    textBoxNights.Text = "";
                }
                else
                {
                    textBoxCheckIn.Text = DateTime.Now.ToShortDateString();
                    textBoxNights.Text = DateTime.Parse(textBoxCheckOut.Text).Subtract(DateTime.Parse(textBoxCheckIn.Text)).TotalDays.ToString();
                    textBoxCheckOut.Text = DateTime.Now.AddDays(Convert.ToInt32(textBoxNights.Text)).ToShortDateString();
                }
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            if(textBoxFirstName.Text != string.Empty ||
                textBoxLastName.Text != string.Empty ||
                textBoxPhone.Text != string.Empty ||
                textBoxEmail.Text != string.Empty ||
                textBoxNights.Text != string.Empty || 
                textBoxAdults.Text != string.Empty ||
                textBoxChild.Text != string.Empty)
            {
                try
                {
                    var roomList = _roomManager.GetRoomsAvailable(DateTime.Parse(textBoxCheckIn.Text).Date, DateTime.Parse(textBoxCheckOut.Text).Date);
                    if(roomList != null)
                    {
                        roomList.RemoveAll(r => r.RoomStatus == "Dirty" || r.RoomStatus == "Out" || r.RoomStatus == "DND");
                        var viewRooms = new ViewRoomsAvailable(roomList);
                        viewRooms.ShowDialog();
                        if (viewRooms.DialogResult == true)
                        {
                            var guest = new Guest()
                            {
                                FirstName = textBoxFirstName.Text,
                                LastName = textBoxLastName.Text,
                                Phone = textBoxPhone.Text,
                                Email = textBoxEmail.Text
                            };
                            //create guest if not already selected
                            if (_selectedGuest == null)
                            {
                                try
                                {
                                    _guestManager.CreateGuest(guest);
                                    try
                                    {
                                        _selectedGuest = _guestManager.GetGuestByEmail(textBoxEmail.Text);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Failed to search for guest using email", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Failed to create guest", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            else
                            {
                                try
                                {
                                    _guestManager.SaveGuestInfo(guest, _selectedGuest);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Failed to update guest", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            //create reservation
                            try
                            {
                                _reservationManager.CreateReservation(new Reservation()
                                {
                                    GuestID = _selectedGuest.GuestID,
                                    RoomID = viewRooms._selectedRoom.RoomID,
                                    ReservationStatus = "Due In",
                                    CheckIn = DateTime.Parse(textBoxCheckIn.Text),
                                    CheckOut = DateTime.Parse(textBoxCheckOut.Text),
                                    Comments = textBoxComments.Text,
                                    AdultAmount = int.Parse(textBoxAdults.Text),
                                    ChildAmount = int.Parse(textBoxAdults.Text)
                                });
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Failed to create reservation", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Must select a room", "Room selection phase canceled", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("No rooms available");
                    }
                    this.DialogResult = true;
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void buttonClearSelectedGuest_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedGuest != null)
            {
                var userChoice = MessageBox.Show("Are you sure you want to remove the selected guest?", "Remove selected guest", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if(userChoice == MessageBoxResult.OK)
                {
                    _selectedGuest = null;
                }
            }
        }
    }
}
