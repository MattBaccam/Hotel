using DataObjects;
using LogicLayer;
using LogicLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScenicHotel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IEmployeeManager _employeeManager = new EmployeeManager();
        private IReservationManager _reservationManager = new ReservationManager();
        private IRoomManager _roomManager = new RoomManager();
        private IGuestManager _guestManager = new GuestManager();
        private IRoomChargeManager _roomChargeManager = new RoomChargeManager();
        private ReservationVM _selectedReservation = null;
        private RoomVM _selectedRoom = null;
        private EmployeeVM _loggedInUser = null;
        private List<TabItem> _menuTabs = null;
        private int _currentTab;
        private List<Pantry> _cart = new List<Pantry>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var tab in tabsetMain.Items)
            {
                ((TabItem)tab).Visibility = Visibility.Collapsed;
            }
            tabContainer.Visibility = Visibility.Collapsed;    
            buttonMenu.Visibility = Visibility.Collapsed;
            reservationGrid.Visibility = Visibility.Collapsed;
            pantryGrid.Visibility = Visibility.Collapsed;
            viewRoomsGrid.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (buttonEmployeeLoginLogout.Content.ToString() == "Log In")
            {
                var email = textBoxEmployeeEmail.Text;
                var password = passwordBoxEmployeePassword.Password;
                try
                {
                    _loggedInUser = _employeeManager.GetEmployeeVM(_employeeManager.LoginEmployee(email, password).EmployeeID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Username or password incorrect", ex.InnerException.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (password == "P@ssw0rd")
                {
                    try
                    {
                        var passwordWindow = new PasswordChangeWindow(_loggedInUser.EmployeeID);
                        var result = passwordWindow.ShowDialog();
                        if (result == true)
                        {
                            MessageBox.Show("Password updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            UpdateUiForUser();
                            return;
                        }
                        else 
                        {
                            MessageBox.Show("You must change your password to continue.", "Logging out", MessageBoxButton.OK, MessageBoxImage.Error);
                            UpdateUiForLogout();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Invalid input, you must change your password to continue.", ex.InnerException.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                        UpdateUiForLogout();
                    }
                }
                else
                {
                    UpdateUiForUser();
                }
            }
            else
            {
                UpdateUiForLogout();
            }
        }

        private void UpdateUiForUser()
        {
            ResetPantryUI();
            ClearReservationUI();
            ClearViewRoomsTab();
            labelEmployeeLoggedIn.Content = $"Welcome {_loggedInUser.FirstName} {_loggedInUser.LastName}";
            labelEmployeeEmail.Visibility = Visibility.Hidden;
            labelEmployeePassword.Visibility = Visibility.Hidden;
            textBoxEmployeeEmail.Visibility = Visibility.Hidden;
            textBoxEmployeeEmail.Text = string.Empty;
            passwordBoxEmployeePassword.Visibility = Visibility.Hidden;
            passwordBoxEmployeePassword.Password = string.Empty;
            buttonEmployeeLoginLogout.Content = "Log Out";
            _menuTabs = new List<TabItem>();

            buttonMenu.Visibility = Visibility.Visible;
            tabContainer.Visibility = Visibility.Visible;

            switch (_loggedInUser.Position.PositionTitle)
            {
                case "Front Desk Agent":
                    tabViewRooms.Visibility = Visibility.Collapsed;
                    tabReservations.Visibility = Visibility.Visible;
                    tabPantry.Visibility = Visibility.Visible;
                    reservationGrid.Visibility = Visibility.Visible;
                    pantryGrid.Visibility = Visibility.Visible;
                    _menuTabs.Add(tabReservations);
                    _menuTabs.Add(tabPantry);
                    break;
                case "Housekeeper":
                    tabReservations.Visibility = Visibility.Collapsed;
                    tabPantry.Visibility = Visibility.Collapsed;
                    tabViewRooms.Visibility = Visibility.Visible;
                    viewRoomsGrid.Visibility = Visibility.Visible;
                    _menuTabs.Add(tabViewRooms);
                    break;
                default:
                    break;
            }
        }

        private void UpdateUiForLogout()
        {
            tabContainer.Visibility = Visibility.Hidden;
            buttonMenu.Visibility = Visibility.Hidden;
            labelEmployeeLoggedIn.Content = string.Empty;
            labelEmployeeEmail.Visibility = Visibility.Visible;
            labelEmployeePassword.Visibility = Visibility.Visible;
            textBoxEmployeeEmail.Visibility = Visibility.Visible;
            passwordBoxEmployeePassword.Visibility = Visibility.Visible;
            buttonEmployeeLoginLogout.Content = "Log In";
            foreach (var tab in tabsetMain.Items)
            {
                ((TabItem)tab).Visibility = Visibility.Collapsed;
            }
            tabContainer.Visibility = Visibility.Collapsed;
            buttonMenu.Visibility = Visibility.Collapsed;
            reservationGrid.Visibility = Visibility.Collapsed;
            pantryGrid.Visibility = Visibility.Collapsed;
            viewRoomsGrid.Visibility = Visibility.Collapsed;
            _loggedInUser = null;
            _selectedReservation = null;
            _menuTabs = new List<TabItem>();
        }
        private void tabsetMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabsetMain.SelectedIndex != _currentTab)
            {
                _selectedReservation = null;
                ClearReservationUI();
                ResetPantryUI();
                ResetReservationContentUI();
            }
            _currentTab = tabsetMain.SelectedIndex;
        }

        #region Reservation Tab
        private void buttonMenu_Click(object sender, RoutedEventArgs e)
        {
            if (_menuTabs[0].Visibility == Visibility.Visible) // Just check if individual tabs are not visible since you cant check the whole tabContainer
            {
                foreach (var tab in _menuTabs)
                {
                    tab.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                foreach (var tab in _menuTabs)
                {
                    tab.Visibility = Visibility.Visible;
                }
            }
        }

        private void checkBoxDueIn_Checked(object sender, RoutedEventArgs e)
        {
            if (_selectedReservation != null && buttonReservationEdit.Content == "Save")
            {
                var userChoice = MessageBox.Show($"Are you sure you want to discard changes to selected reservation {_selectedReservation.Name}", "Discard changes", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if(userChoice == MessageBoxResult.OK)
                {
                    ResetReservationContentUI();
                    LockReservationTextBoxes();
                    buttonReservationEdit.Content = "Edit";
                }
            }
            else
            {
                if (checkBoxDueOut.IsChecked == true)
                {
                    checkBoxDueOut.IsChecked = false;
                }
                if (checkBoxOut.IsChecked == true)
                {
                    checkBoxOut.IsChecked = false;
                }
                if (checkBoxCanceled.IsChecked == true)
                {
                    checkBoxCanceled.IsChecked = false;
                }
                try
                {
                    ClearReservationUI();
                    gridReservationDisplaySearch.ItemsSource = _reservationManager.GetReservationsByStatus("Due In");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not find reservations under \"Due In\"", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void checkBoxDueOut_Checked(object sender, RoutedEventArgs e)
        {
            if (_selectedReservation != null && buttonReservationEdit.Content == "Save")
            {
                var userChoice = MessageBox.Show($"Are you sure you want to discard changes to selected reservation {_selectedReservation.Name}", "Discard changes", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (userChoice == MessageBoxResult.OK)
                {
                    ResetReservationContentUI();
                    LockReservationTextBoxes();
                    buttonReservationEdit.Content = "Edit";
                }
            }
            else
            {
                if (checkBoxDueIn.IsChecked == true)
                {
                    checkBoxDueIn.IsChecked = false;
                }
                if (checkBoxOut.IsChecked == true)
                {
                    checkBoxOut.IsChecked = false;
                }
                if (checkBoxCanceled.IsChecked == true)
                {
                    checkBoxCanceled.IsChecked = false;
                }

                try
                {
                    ClearReservationUI();
                    gridReservationDisplaySearch.ItemsSource = _reservationManager.GetReservationsByStatus("Due Out");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not find reservations under  \"Due Out\"", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void checkBoxOut_Checked(object sender, RoutedEventArgs e)
        {
            if (_selectedReservation != null && buttonReservationEdit.Content == "Save")
            {
                var userChoice = MessageBox.Show($"Are you sure you want to discard changes to selected reservation {_selectedReservation.Name}", "Discard changes", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (userChoice == MessageBoxResult.OK)
                {
                    ResetReservationContentUI();
                    LockReservationTextBoxes();
                    buttonReservationEdit.Content = "Edit";
                }
            }
            else
            {
                if (checkBoxDueIn.IsChecked == true)
                {
                    checkBoxDueIn.IsChecked = false;
                }
                if (checkBoxDueOut.IsChecked == true)
                {
                    checkBoxDueOut.IsChecked = false;
                }
                if (checkBoxCanceled.IsChecked == true)
                {
                    checkBoxCanceled.IsChecked = false;
                }
                try
                {
                    gridReservationDisplaySearch.ItemsSource = _reservationManager.GetReservationsByStatus("Out");
                }
                catch (Exception ex)
                {
                    ClearReservationUI();
                    MessageBox.Show("Could not find reservations under \"Out\"", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void checkBoxCanceled_Checked(object sender, RoutedEventArgs e)
        {
            if (_selectedReservation != null && buttonReservationEdit.Content == "Save")
            {
                var userChoice = MessageBox.Show($"Are you sure you want to discard changes to selected reservation {_selectedReservation.Name}", "Discard changes", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (userChoice == MessageBoxResult.OK)
                {
                    ResetReservationContentUI();
                    LockReservationTextBoxes();
                    buttonReservationEdit.Content = "Edit";
                }
            }
            else
            {
                if (checkBoxDueIn.IsChecked == true)
                {
                    checkBoxDueIn.IsChecked = false;
                }
                if (checkBoxDueOut.IsChecked == true)
                {
                    checkBoxDueOut.IsChecked = false;
                }
                if (checkBoxOut.IsChecked == true)
                {
                    checkBoxOut.IsChecked = false;
                }

                try
                {
                    gridReservationDisplaySearch.ItemsSource = _reservationManager.GetReservationsByStatus("Canceled");
                }
                catch (Exception ex)
                {
                    ClearReservationUI();
                    MessageBox.Show("Could not find reservations under \"Canceled\"", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void gridReservationDisplaySearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(buttonReservationEdit.Content != "Save")
            {
                if (gridReservationDisplaySearch.SelectedItem != null)
                {
                    _selectedReservation = gridReservationDisplaySearch.SelectedItem as ReservationVM;

                    textboxReservationEditContactFirstName.Text = _selectedReservation.Guest.FirstName;
                    textboxReservationEditContactLastName.Text = _selectedReservation.Guest.LastName;
                    textboxReservationEditContactPhoneNumber.Text = _selectedReservation.Guest.Phone;
                    textboxReservationEditContactEmail.Text = _selectedReservation.Guest.Email;
                    textBoxReservationCheckIn.Text = _selectedReservation.CheckIn.ToShortDateString();
                    textBoxReservationCheckOut.Text = _selectedReservation.CheckOut.ToShortDateString();
                    textBoxReservationRoom.Text = _selectedReservation.Room.RoomID.ToString();
                    textBoxReservationRoomType.Text = _selectedReservation.Room.RoomType.RoomTypeID;
                    textBoxReservationAdultAmount.Text = _selectedReservation.AdultAmount.ToString();
                    textBoxReservationChildAmount.Text = _selectedReservation.ChildAmount.ToString();
                    textBoxReservationRoomRate.Text = _selectedReservation.Room.RoomType.RoomPrice.ToString();

                    var roomStatuses = new List<string>()
                    {
                      "Inspected", "Dirty", "Out", "DND"
                    };
                    comboBoxReservationRoomStatus.ItemsSource = roomStatuses;
                    comboBoxReservationRoomStatus.Text = _selectedReservation.Room.RoomStatus;
                    textBoxReservationPaid.Text = _selectedReservation.Paid.ToString();
                }
            }
        }

        private void buttonReservationEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedReservation != null)
            {
                if (buttonReservationEdit.Content.ToString() == "Edit")
                {
                    var userChoice = MessageBox.Show("Are you sure you want to enter edit mode for the selected reservation", "Reservation edit", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if(userChoice == MessageBoxResult.OK)
                    {
                        UnlockReservationTextBoxes();
                        buttonReservationEdit.Content = "Save";
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    try
                    {
                        //validate the ui for saving
                        if (textboxReservationEditContactFirstName.Text != string.Empty &&
                            textboxReservationEditContactLastName.Text != string.Empty &&
                            textboxReservationEditContactPhoneNumber.Text != string.Empty &&
                            textboxReservationEditContactEmail.Text != string.Empty &&
                            textBoxReservationCheckIn.Text != string.Empty &&
                            textBoxReservationCheckOut.Text != string.Empty &&
                            (textBoxReservationAdultAmount.Text != string.Empty &&
                            int.TryParse(textBoxReservationAdultAmount.Text, out int adultAmountResult)) &&
                            (textBoxReservationAdultAmount.Text != string.Empty &&
                            int.TryParse(textBoxReservationAdultAmount.Text, out int childAmountResult)))
                        {//start of if statement
                            var newGuest = new Guest()
                            {
                                FirstName = textboxReservationEditContactFirstName.Text,
                                LastName = textboxReservationEditContactLastName.Text,
                                Phone = textboxReservationEditContactPhoneNumber.Text,
                                Email = textboxReservationEditContactEmail.Text
                            };
                            var reservation = new Reservation()
                            {
                                ReservationID = _selectedReservation.ReservationID,
                                GuestID = _selectedReservation.GuestID,
                                RoomID = int.Parse(textBoxReservationRoom.Text),
                                CheckIn = DateTime.Parse(textBoxReservationCheckIn.Text),
                                CheckOut = DateTime.Parse(textBoxReservationCheckOut.Text),
                                AdultAmount = int.Parse(textBoxReservationAdultAmount.Text),
                                ChildAmount = int.Parse(textBoxReservationAdultAmount.Text)
                            };
                            var room = new Room()
                            {
                                RoomID = Convert.ToInt32(textBoxReservationRoom.Text),
                                RoomTypeID = textBoxReservationRoomType.Text,
                                RoomAvailability = _selectedReservation.Room.RoomAvailability,
                                RoomStatus = comboBoxReservationRoomStatus.Text
                            };
                            var failedPortions = string.Empty;
                            try//save room status
                            {
                                failedPortions = _roomManager.SaveRoomStatus(_selectedReservation.Room.RoomID, _selectedReservation.Room.RoomStatus, comboBoxReservationRoomStatus.Text) ? "" : "Failed to save room information\n";
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Failed to save room", ex.ToString());
                            }
                            try//save guest
                            {
                                failedPortions = _guestManager.SaveGuestInfo(newGuest, _selectedReservation.Guest) ? "" : "Failed to save guest information\n";
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Failed to save the guest information", ex.ToString());
                            }
                            try//save reservation
                            {
                                failedPortions = _reservationManager.SaveReservation(reservation, _selectedReservation) ? "" : "Failed to save reservation information\n";
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Failed to save the reservation information", ex.ToString());
                            }
                            if (failedPortions == string.Empty)
                            {
                                MessageBox.Show("Successfully saved the changes", "Reservation changes saved", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            }//verify all saves worked
                            else
                            {
                                MessageBox.Show($"Failed to save some if not all changes \n {failedPortions}", "Reservation changes fail", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            buttonReservationEdit.Content = "Edit";
                            LockReservationTextBoxes();
                            ResetReservationContentUI();
                        }
                        else
                        {
                            MessageBox.Show("No rooms available", "No rooms for service");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to load rooms", ex.ToString());
                    }
                }
            }
        }

        private void buttonReservationComments_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedReservation != null)
            {
                var commentWindow = new ReservationCommentWindow(_selectedReservation.ReservationID, _selectedReservation.Comments);
                commentWindow.ShowDialog();
                if(commentWindow.DialogResult == true)
                {
                    MessageBox.Show("Comments successfully saved", "Saved changes", MessageBoxButton.OK, MessageBoxImage.Information);
                    ResetReservationContentUI();
                }
                else
                {
                    MessageBox.Show("Comments not saved", "Canceled changes", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void buttonReservationSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime? checkIn = null;
                DateTime? checkOut = null;
                gridReservationDisplaySearch.ItemsSource = null;
                if (!string.IsNullOrEmpty(textBoxReservationSearchCheckIn.Text))
                {
                    checkIn = DateTime.Parse(textBoxReservationSearchCheckIn.Text).Date;
                }
                if (!string.IsNullOrEmpty(textBoxReservationSearchCheckOut.Text))
                {
                    checkOut = DateTime.Parse(textBoxReservationSearchCheckOut.Text).Date;
                }
                gridReservationDisplaySearch.ItemsSource = _reservationManager.GetReservationsViaSearch(textBoxReservationSearchFirstName.Text, textBoxReservationSearchLastName.Text, checkIn, checkOut);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not find any under that search", ex.ToString());
            }
        }

        private void buttonReservationSearchCheckIn_Click(object sender, RoutedEventArgs e)
        {
            var calendarWindow = new CalendarWindow(DateTime.Now.Date);
            calendarWindow.ShowDialog();
            if (calendarWindow.DialogResult == true)
            {
                textBoxReservationSearchCheckIn.Text = DateTime.Parse(calendarWindow._date.ToString()).ToShortDateString();
            }
        }

        private void buttonReservationSearchCheckOut_Click(object sender, RoutedEventArgs e)
        {
            var calendarWindow = new CalendarWindow(DateTime.Now.Date);
            calendarWindow.ShowDialog();
            if (calendarWindow.DialogResult == true)
            {
                textBoxReservationSearchCheckOut.Text = DateTime.Parse(calendarWindow._date.ToString()).ToShortDateString();
                if (textBoxReservationSearchCheckIn.Text != string.Empty && DateTime.Parse(calendarWindow._date.ToString()).Date < DateTime.Parse(textBoxReservationSearchCheckIn.Text) || DateTime.Parse(calendarWindow._date.ToString()).Date == DateTime.Parse(textBoxReservationSearchCheckIn.Text).Date)
                {
                    MessageBox.Show("Invalid check out date selected", "Date selection canceled", MessageBoxButton.OK, MessageBoxImage.Error);
                    textBoxReservationSearchCheckOut.Text = string.Empty;
                }
            }
        }

        private void buttonReservationCheckInCalendar_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedReservation != null )
            {
                var calendarWindow = new CalendarWindow(DateTime.Now);
                calendarWindow.ShowDialog();
                if (calendarWindow.DialogResult == true)
                {
                    if (calendarWindow._date < _selectedReservation.CheckOut && DateTime.Parse(calendarWindow._date.ToString()).Date > _selectedReservation.CheckIn.Date)
                    {
                        if (CheckAvailableRooms(DateTime.Parse(textBoxReservationCheckIn.Text).Date, DateTime.Parse(textBoxReservationCheckOut.Text).Date))
                        {
                            MessageBox.Show("Successfully selected a valid reschedule date", "Selection Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            textBoxReservationCheckIn.Text = DateTime.Parse(calendarWindow._date.ToString()).ToShortDateString();
                            textBoxReservationCheckOut.Text = _selectedReservation.CheckOut.ToShortDateString();
                        }
                    }
                    else if (DateTime.Parse(calendarWindow._date.ToString()).Date < _selectedReservation.CheckOut.Date && DateTime.Parse(calendarWindow._date.ToString()).Date < _selectedReservation.CheckIn.Date)
                    {
                        if (CheckAvailableRooms(DateTime.Parse(calendarWindow._date.ToString()).Date, DateTime.Parse(textBoxReservationCheckIn.Text).AddDays(-1).Date))
                        {
                            MessageBox.Show("Successfully selected a valid reschedule date", "Selection Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            textBoxReservationCheckIn.Text = DateTime.Parse(calendarWindow._date.ToString()).ToShortDateString();
                            textBoxReservationCheckOut.Text = _selectedReservation.CheckOut.ToShortDateString();
                        }
                    }
                    else
                    {
                        if (CheckAvailableRooms(DateTime.Parse(calendarWindow._date.ToString()).Date, DateTime.Parse(calendarWindow._date.ToString()).AddDays(1).Date))
                        {
                            MessageBox.Show("Successfully selected a valid reschedule date", "Selection Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            textBoxReservationCheckIn.Text = DateTime.Parse(calendarWindow._date.ToString()).ToShortDateString();
                            textBoxReservationCheckOut.Text = DateTime.Parse(calendarWindow._date.ToString()).AddDays(1).ToShortDateString();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Canceled room selection phase", "Canceled room selection", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonReservationCheckOutCalendar_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedReservation != null)
            {
                var calendarWindow = new CalendarWindow(DateTime.Now.Date);
                calendarWindow.ShowDialog();
                if (calendarWindow.DialogResult == true)
                {
                    if (DateTime.Parse(calendarWindow._date.ToString()).Date < DateTime.Parse(textBoxReservationCheckIn.Text).Date || DateTime.Parse(calendarWindow._date.ToString()).Date == DateTime.Parse(textBoxReservationCheckIn.Text).Date)
                    {
                        MessageBox.Show("Invalid check out date selected", "Date selection canceled", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (DateTime.Parse(calendarWindow._date.ToString()).Date > DateTime.Parse(textBoxReservationCheckOut.Text).Date)
                        {
                            if (CheckAvailableRooms(DateTime.Parse(textBoxReservationCheckOut.Text).AddDays(1).Date, DateTime.Parse(calendarWindow._date.ToString()).Date))
                            {
                                textBoxReservationCheckOut.Text = DateTime.Parse(calendarWindow._date.ToString()).ToShortDateString();
                            }
                        }
                        else
                        {
                            if (CheckAvailableRooms(DateTime.Parse(textBoxReservationCheckIn.Text).Date, DateTime.Parse(textBoxReservationCheckOut.Text).Date))
                            {
                                textBoxReservationCheckOut.Text = DateTime.Parse(calendarWindow._date.ToString()).ToShortDateString();
                            }
                        }
                    }
                }
            }
        }

        private void buttonReservationRoomCharges_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedReservation != null)
            {
                if (!_roomChargeManager.CheckForRoomCharges(_selectedReservation.ReservationID))
                {
                    MessageBox.Show("The selected guest does not have any room charges");
                }
                else
                {
                    var list = _selectedReservation.RoomCharges.RoomChargeItems.Where(i => i.Active == true);
                    if (list.Count() > 0)
                    {
                        var roomChargeWindow = new RoomChargeWindow(_selectedReservation, _loggedInUser.EmployeeID);
                        roomChargeWindow.ShowDialog();
                        ResetReservationContentUI();
                    }
                }
            }
        }

        private void buttonReservationCheckIn_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedReservation != null && (_selectedReservation.ReservationStatus != "Due Out" && _selectedReservation.ReservationStatus != "Out"))
            {
                try
                {
                    if (_selectedReservation.CheckIn.Date > DateTime.Now)
                    {
                        var userChoice = MessageBox.Show($"Are you sure you want to check in {_selectedReservation.Name} early?", "Early check In", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        if (userChoice == MessageBoxResult.OK)
                        {
                            var checkOut = _selectedReservation.CheckIn.AddDays(-1);
                            if (CheckAvailableRooms(DateTime.Now.Date, checkOut.Date))
                            {
                                try
                                {
                                    if(_reservationManager.SaveReservationForCheckIn(_selectedReservation.ReservationID, DateTime.Now.Date, _selectedReservation.CheckIn))
                                    {
                                        if(_roomManager.UpdateRoomAvailability(_selectedReservation.RoomID, false, _selectedReservation.Room.RoomAvailability))
                                        {
                                            MessageBox.Show($"Successfully checked in {_selectedReservation.Name}", "Check in success", MessageBoxButton.OK, MessageBoxImage.Information);
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Failed to check in {_selectedReservation.Name}", ex.ToString());
                                }
                                ResetReservationContentUI();
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Check in canceled for {_selectedReservation.Name}", "Check in canceled", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                        }
                    }
                    else if (_selectedReservation.CheckIn.Date == DateTime.Now.Date)
                    {
                        var userChoice = MessageBox.Show($"Are you sure you want to check in {_selectedReservation.Name}?", "Check In", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        if (userChoice == MessageBoxResult.OK)
                        {
                            try
                            {
                                if(_reservationManager.SaveReservationForCheckIn(_selectedReservation.ReservationID, DateTime.Now.Date, _selectedReservation.CheckIn.Date))
                                {
                                    if (_roomManager.UpdateRoomAvailability(_selectedReservation.RoomID, false,_selectedReservation.Room.RoomAvailability))
                                    {
                                        MessageBox.Show($"Successfully checked in {_selectedReservation.Name}", "Check in success", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Failed to check in {_selectedReservation.Name}", ex.ToString());
                            }
                            ResetReservationContentUI();
                        }
                        else
                        {
                            MessageBox.Show($"Check in canceled for {_selectedReservation.Name}", "Check in canceled", MessageBoxButton.OK, MessageBoxImage.Warning);
                            ResetReservationContentUI();
                        }
                    }
                    else
                    {
                        var userChoice = MessageBox.Show($"Are you sure you want to check in {_selectedReservation.Name} late?", "Late check in", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        if (userChoice == MessageBoxResult.OK)
                        {
                            try
                            {
                                if(_reservationManager.SaveReservationForCheckIn(_selectedReservation.ReservationID, DateTime.Now.Date, _selectedReservation.CheckIn))
                                {
                                    if (_roomManager.UpdateRoomAvailability(_selectedReservation.RoomID, false, _selectedReservation.Room.RoomAvailability))
                                    {
                                        MessageBox.Show($"Successfully checked in {_selectedReservation.Name}", "Check in success", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Failed to check in {_selectedReservation.Name}", ex.ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Check in canceled for {_selectedReservation.Name}", "Check in canceled", MessageBoxButton.OK, MessageBoxImage.Warning);
                            ResetReservationContentUI();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to find if the room is already taken in house", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonReservationCheckOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedReservation != null && (_selectedReservation.ReservationStatus != "Due In" && _selectedReservation.ReservationStatus != "Out"))
                {
                    if(_selectedReservation.CheckOut.Date > DateTime.Now.Date)
                    {
                        var userChoice = MessageBox.Show("Are you sure you want to check out the selected guest early?", "Early check out", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        if (userChoice == MessageBoxResult.OK)
                        {
                            var receiptWindow = new ReceiptWindow(_selectedReservation);
                            receiptWindow.ShowDialog();
                            if (receiptWindow.DialogResult == true)
                            {
                                try
                                {
                                    _reservationManager.SaveReservationForCheckOut(_selectedReservation.ReservationID, DateTime.Now.Date, _selectedReservation.CheckOut.Date);
                                    MessageBox.Show($"Successfully checked out {_selectedReservation.Name}", "Check out success", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ResetReservationContentUI();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("failed to save reservation for check out", ex.ToString());
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Check out canceled for {_selectedReservation.Name}", "Check out canceled", MessageBoxButton.OK, MessageBoxImage.Warning);
                                ResetReservationContentUI();
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Check out canceled for {_selectedReservation.Name}", "Check out canceled", MessageBoxButton.OK, MessageBoxImage.Warning);
                            ResetReservationContentUI();
                        }
                    }
                    else if(_selectedReservation != null && _selectedReservation.CheckOut == DateTime.Now.Date)
                    {
                        var receiptWindow = new ReceiptWindow(_selectedReservation);
                        receiptWindow.ShowDialog();
                        if (receiptWindow.DialogResult == true)
                        {
                            try
                            {
                                _reservationManager.SaveReservationForCheckOut(_selectedReservation.ReservationID, DateTime.Now.Date, _selectedReservation.CheckOut.Date);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("failed to save reservation for check out", ex.ToString());
                            }
                            MessageBox.Show($"Successfully checked out {_selectedReservation.Name}", "Check out success", MessageBoxButton.OK, MessageBoxImage.Information);
                            ResetReservationContentUI();
                        }
                        else
                        {
                            MessageBox.Show($"Check out canceled for {_selectedReservation.Name}", "Check out canceled", MessageBoxButton.OK, MessageBoxImage.Warning);
                            ResetReservationContentUI();
                        }
                    }
                    else if (_selectedReservation != null && _selectedReservation.CheckOut.Date < DateTime.Now.Date)
                    {
                        var userChoice = MessageBox.Show("This user has a late check out and will be charged the extended days", "Late check out", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        if(userChoice == MessageBoxResult.OK)
                        {
                            var receiptWindow = new ReceiptWindow(_selectedReservation);
                            receiptWindow.ShowDialog();
                            if (receiptWindow.DialogResult == true)
                            {
                                try
                                {
                                    _reservationManager.SaveReservationForCheckOut(_selectedReservation.ReservationID, DateTime.Now.Date, _selectedReservation.CheckOut.Date);
                                    MessageBox.Show($"Successfully checked out {_selectedReservation.Name}", "Check out success", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("failed to save reservation for check out", ex.ToString());
                                }
                                ResetReservationContentUI();
                            }
                            else
                            {
                                MessageBox.Show($"Check out canceled for {_selectedReservation.Name}", "Check out canceled", MessageBoxButton.OK, MessageBoxImage.Warning);
                                ResetReservationContentUI();
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Check out canceled for {_selectedReservation.Name}", "Check out canceled", MessageBoxButton.OK, MessageBoxImage.Warning);
                            ResetReservationContentUI();
                        }
                    }
                    if (_selectedReservation.CheckIn.Date != _selectedReservation.CheckOut.Date)
                    {
                        _roomManager.SaveRoomStatus(_selectedReservation.RoomID, _selectedReservation.Room.RoomStatus, "Dirty");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Check out failed", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ResetReservationContentUI()
        {
            if(_selectedReservation != null)
            {
                try
                {
                    checkBoxDueIn.IsChecked = false;
                    checkBoxDueOut.IsChecked = false;
                    checkBoxOut.IsChecked = false;
                    _selectedReservation = _reservationManager.GetReservationVM(_selectedReservation.ReservationID);
                    textboxReservationEditContactFirstName.Text = _selectedReservation.Guest.FirstName;
                    textboxReservationEditContactLastName.Text = _selectedReservation.Guest.LastName;
                    textboxReservationEditContactPhoneNumber.Text = _selectedReservation.Guest.Phone;
                    textboxReservationEditContactEmail.Text = _selectedReservation.Guest.Email;
                    textBoxReservationCheckIn.Text = _selectedReservation.CheckIn.ToShortDateString();
                    textBoxReservationCheckOut.Text = _selectedReservation.CheckOut.ToShortDateString();
                    textBoxReservationAdultAmount.Text = _selectedReservation.AdultAmount.ToString();
                    textBoxReservationChildAmount.Text = _selectedReservation.ChildAmount.ToString();
                    textBoxReservationRoom.Text = _selectedReservation.Room.RoomID.ToString();
                    textBoxReservationRoomType.Text = _selectedReservation.Room.RoomType.RoomTypeID;
                    textBoxReservationRoomRate.Text = _selectedReservation.Room.RoomType.RoomPrice.ToString();
                    textBoxReservationPaid.Text = _selectedReservation.Paid.ToString();
                    comboBoxReservationRoomStatus.Text = _selectedReservation.Room.RoomStatus;
                    checkBoxDueIn.IsChecked = false;
                    checkBoxDueOut.IsChecked = false;
                    checkBoxOut.IsChecked = false;
                    gridReservationDisplaySearch.ItemsSource = null;
                    buttonReservationCancel.Visibility = Visibility.Hidden;
                    textBoxReservationRoom.IsEnabled = false;

                    textboxReservationEditContactFirstName.Text = _selectedReservation.Guest.FirstName;
                    textboxReservationEditContactLastName.Text = _selectedReservation.Guest.LastName;
                    textboxReservationEditContactPhoneNumber.Text = _selectedReservation.Guest.Phone;
                    textboxReservationEditContactEmail.Text = _selectedReservation.Guest.Email;
                    textBoxReservationCheckIn.Text = _selectedReservation.CheckIn.ToShortDateString();
                    textBoxReservationCheckOut.Text = _selectedReservation.CheckOut.ToShortDateString();
                    textBoxReservationRoom.Text = _selectedReservation.Room.RoomID.ToString();
                    textBoxReservationRoomType.Text = _selectedReservation.Room.RoomType.RoomTypeID;
                    textBoxReservationAdultAmount.Text = _selectedReservation.AdultAmount.ToString();
                    textBoxReservationChildAmount.Text = _selectedReservation.ChildAmount.ToString();
                    textBoxReservationRoomRate.Text = _selectedReservation.Room.RoomType.RoomPrice.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not refresh the reservation screen", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                    ClearReservationUI();
                }
            }
            else
            {
                ClearReservationUI();
            }
        }

        private void ClearReservationUI()
        {
            checkBoxCanceled.IsChecked = false;
            textboxReservationEditContactFirstName.Text = string.Empty;
            textboxReservationEditContactLastName.Text = string.Empty;
            textboxReservationEditContactPhoneNumber.Text = string.Empty;
            textboxReservationEditContactEmail.Text = string.Empty;
            textBoxReservationCheckIn.Text = string.Empty;
            textBoxReservationCheckOut.Text = string.Empty;
            textBoxReservationRoom.Text = string.Empty;
            textBoxReservationRoomType.Text = string.Empty;
            textBoxReservationRoomRate.Text = string.Empty;
            textBoxReservationPaid.Text = string.Empty;
            textBoxReservationAdultAmount.Text = string.Empty;
            textBoxReservationChildAmount.Text = string.Empty;
            comboBoxReservationRoomStatus.Text = string.Empty;
            textBoxReservationSearchFirstName.Text = string.Empty;
            textBoxReservationSearchLastName.Text = string.Empty;
            textBoxReservationSearchCheckIn.Text = string.Empty;
            textBoxReservationSearchCheckOut.Text = string.Empty;
            gridReservationDisplaySearch.ItemsSource = null;
        }

        private void LockReservationTextBoxes()
        {
            textboxReservationEditContactFirstName.IsReadOnly = true;
            textboxReservationEditContactLastName.IsReadOnly = true;
            textboxReservationEditContactPhoneNumber.IsReadOnly = true;
            textboxReservationEditContactEmail.IsReadOnly = true;
            textBoxReservationAdultAmount.IsReadOnly = true;
            textBoxReservationChildAmount.IsReadOnly = true;
            comboBoxReservationRoomStatus.IsEnabled = false;
            buttonReservationCheckInCalendar.IsEnabled = false;
            buttonReservationCheckOutCalendar.IsEnabled = false;
            buttonReservationCheckInCalendar.IsEnabled = false;
            buttonReservationCheckOutCalendar.IsEnabled = false;
            buttonReservationCancel.Visibility = Visibility.Hidden;
            textBoxReservationRoom.IsEnabled = false;
        }

        private void UnlockReservationTextBoxes()
        {
            textboxReservationEditContactFirstName.IsReadOnly = false;
            textboxReservationEditContactLastName.IsReadOnly = false;
            textboxReservationEditContactPhoneNumber.IsReadOnly = false;
            textboxReservationEditContactEmail.IsReadOnly = false;
            textBoxReservationAdultAmount.IsReadOnly = false;
            textBoxReservationChildAmount.IsReadOnly = false;
            comboBoxReservationRoomStatus.IsEnabled = true;
            buttonReservationCheckInCalendar.IsEnabled = true;
            buttonReservationCheckOutCalendar.IsEnabled = true;
            buttonReservationCheckInCalendar.IsEnabled = _selectedReservation.ReservationStatus == "Out" ? false : true;
            buttonReservationCheckOutCalendar.IsEnabled = _selectedReservation.ReservationStatus == "Out" ? false : true;
            buttonReservationCancel.Visibility = Visibility.Visible;
            textBoxReservationRoom.IsEnabled = true;
        }

        private bool CheckAvailableRooms(DateTime newCheckIn, DateTime newCheckOut)
        {
            var result = false;
            try
            {
                var roomList = _roomManager.GetRoomsAvailable(newCheckIn, newCheckOut);
                
                if (roomList.Count != 0)
                {
                    if (roomList.Where(r => r.RoomID == _selectedReservation.RoomID).Count() > 0 && _selectedReservation.CheckIn.Date > DateTime.Now.Date)
                    {
                        var userChoice = MessageBox.Show("The room is available for early check in", "Early check in", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                        if(userChoice == MessageBoxResult.OK)
                        {
                            //textBoxReservationCheckIn.Text = newCheckIn;
                            //textBoxReservationCheckOut.Text = _selectedReservation.CheckOut;
                            return result = true;
                        }
                        else
                        {
                            return result = false;
                        }
                    }
                    else
                    {
                        var viewRoomsAvailable = new ViewRoomsAvailable(roomList);
                        viewRoomsAvailable.ShowDialog();
                        if (viewRoomsAvailable.DialogResult == true)
                        {
                            if (_selectedReservation.RoomID == viewRoomsAvailable._selectedRoom.RoomID)
                            {
                                //textBoxReservationCheckIn.Text = newCheckIn;
                                //textBoxReservationCheckOut.Text = newCheckOut;
                                return result = true;
                            }
                            else
                            {
                                if (_selectedReservation.CheckIn.Date < newCheckIn.Date)
                                {
                                    //textBoxReservationCheckIn.Text = newCheckIn;
                                    //textBoxReservationCheckOut.Text = newCheckOut;
                                    textBoxReservationRoom.Text = viewRoomsAvailable._selectedRoom.RoomID.ToString();
                                    textBoxReservationRoomType.Text = viewRoomsAvailable._selectedRoom.RoomTypeID;
                                    textBoxReservationRoomRate.Text = viewRoomsAvailable._selectedRoom.RoomType.RoomPrice.ToString();
                                    comboBoxReservationRoomStatus.Text = viewRoomsAvailable._selectedRoom.RoomStatus;
                                    return result = true;
                                }
                                else
                                {
                                    //textBoxReservationCheckIn.Text = newCheckIn;
                                    //textBoxReservationCheckOut.Text = newCheckOut;
                                    textBoxReservationRoom.Text = viewRoomsAvailable._selectedRoom.RoomID.ToString();
                                    textBoxReservationRoomType.Text = viewRoomsAvailable._selectedRoom.RoomTypeID;
                                    textBoxReservationRoomRate.Text = viewRoomsAvailable._selectedRoom.RoomType.RoomPrice.ToString();
                                    comboBoxReservationRoomStatus.Text = viewRoomsAvailable._selectedRoom.RoomStatus;
                                    return result = true;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Canceled room selection phase", "Canceled room selection", MessageBoxButton.OK, MessageBoxImage.Error);
                            return result = false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No rooms available", "No rooms for service", MessageBoxButton.OK, MessageBoxImage.Error);
                    return result = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not select rooms available for the reservations dates", ex.ToString());
            }
            return result;
        }

        private void buttonReservationCancel_Click(object sender, RoutedEventArgs e)
        {
            var userChoice = MessageBox.Show("Are you sure you want to exit edit mode?", "Close edit mode", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if(userChoice == MessageBoxResult.OK)
            {
                LockReservationTextBoxes();
                ResetReservationContentUI();
                buttonReservationEdit.Content = "Edit";
            }
        }

        private void textBoxReservationRoom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_selectedReservation != null)
                {
                    if (CheckAvailableRooms(DateTime.Parse(textBoxReservationCheckIn.Text).Date, DateTime.Parse(textBoxReservationCheckOut.Text).Date))
                    {
                        MessageBox.Show("Room has been successfully changed", "Room change success");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to search for available rooms for this reservation", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonReservationCancelReservation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(_selectedReservation != null && _selectedReservation.ReservationStatus == "Due In")
                {
                    var userChoice = MessageBox.Show($"Are you sure you want to cancel reservation for {_selectedReservation.Name}?", "Cancel reservation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if(userChoice == MessageBoxResult.OK)
                    {
                        if (_reservationManager.SaveReservationForCancel(_selectedReservation.ReservationID, _selectedReservation.ReservationStatus))
                        {
                            MessageBox.Show("Successfully canceled", "Cancel success");
                            ResetReservationContentUI();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not cancel reservation", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonReservationCreateReservation_Click(object sender, RoutedEventArgs e)
        {
            var reservationForm = new ReservationFormWindow();
            reservationForm.ShowDialog();
            if(reservationForm.DialogResult == true)
            {
                MessageBox.Show("Successfully created a reservation", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetReservationContentUI();
            }
            else
            {
                MessageBox.Show("Failed creation of a reservation", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Pantry Sales Tab
        private void buttonPantrySearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var reservationList = new List<ReservationVM>();
                if(!string.IsNullOrWhiteSpace(textBoxPantryBoxFirstName.Text) &&
                   !string.IsNullOrWhiteSpace(textBoxPantryBoxLastName.Text) &&
                   !string.IsNullOrWhiteSpace(textBoxPantryRoomNumber.Text))
                {
                    reservationList.Add(_reservationManager.GetReservationsByNameAndRoomID(textBoxPantryBoxFirstName.Text, textBoxPantryBoxLastName.Text, int.Parse(textBoxPantryRoomNumber.Text)));
                }
                else if (!string.IsNullOrWhiteSpace(textBoxPantryBoxFirstName.Text) &&
                   !string.IsNullOrWhiteSpace(textBoxPantryBoxLastName.Text) &&
                   string.IsNullOrWhiteSpace(textBoxPantryRoomNumber.Text))
                {
                    reservationList = _reservationManager.GetReservationsViaSearch(textBoxPantryBoxFirstName.Text, textBoxPantryBoxLastName.Text);
                }
                else if (!string.IsNullOrWhiteSpace(textBoxPantryBoxFirstName.Text) &&
                   string.IsNullOrWhiteSpace(textBoxPantryBoxLastName.Text) &&
                   string.IsNullOrWhiteSpace(textBoxPantryRoomNumber.Text))
                {
                    reservationList = _reservationManager.GetReservationsViaSearch(textBoxPantryBoxFirstName.Text);
                }
                else if (string.IsNullOrWhiteSpace(textBoxPantryBoxFirstName.Text)&&
                   !string.IsNullOrWhiteSpace(textBoxPantryBoxLastName.Text)&&
                   string.IsNullOrWhiteSpace(textBoxPantryRoomNumber.Text))
                {
                    reservationList = _reservationManager.GetReservationsViaSearch(textBoxPantryBoxLastName.Text);
                }
                else if (string.IsNullOrWhiteSpace(textBoxPantryBoxFirstName.Text) &&
                   string.IsNullOrWhiteSpace(textBoxPantryBoxLastName.Text) &&
                   !string.IsNullOrWhiteSpace(textBoxPantryRoomNumber.Text))
                {
                    reservationList.Add(_reservationManager.GetReservationByRoomID(int.Parse(textBoxPantryRoomNumber.Text)));
                }
                else
                {
                    MessageBox.Show("Missing fields", "Missing information", MessageBoxButton.OK);
                }
                reservationList.RemoveAll(res => res.ReservationStatus != "Due Out");
                gridPantryDisplayGuest.ItemsSource = reservationList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No current reservation found", ex.Message.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonPantryRoomCharge_Click(object sender, RoutedEventArgs e)
        {
            var userChoice = MessageBox.Show("Are you sure you want to make the current sale?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (userChoice == MessageBoxResult.OK)
            {
                if (_selectedReservation != null && gridPantryDisplaySales.ItemsSource != null)
                {
                    if (_selectedReservation.RoomCharges.RoomChargeItems == null)
                    {
                        try
                        {
                            _cart.ForEach(i =>
                            {
                                var pantryItem = _roomChargeManager.GetPantryItem(i.PantryID);
                                if (pantryItem.ItemAmount - i.ItemAmount > 0)
                                {
                                    i.ItemAmount = pantryItem.ItemAmount - i.ItemAmount;
                                    i.ItemPrice = i.ItemAmount * pantryItem.ItemPrice;
                                }
                                else
                                {
                                    MessageBox.Show($"The item {i.PantryID} will go out of inventory after this sale because of insuffecient quantities, Sale canceled", "Inventory warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                                    return;
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to retrieve pantry item \n Cannot proccess sale", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        try
                        {
                            _roomChargeManager.CreateRoomCharge(_selectedReservation.ReservationID);
                            try
                            {
                                if (_roomChargeManager.PostRoomCharges(_roomChargeManager.GetRoomCharge(_selectedReservation.ReservationID).RoomChargeID, _cart))
                                {
                                    var logList = new List<SalesLog>();
                                    _cart.ForEach(c => logList.Add(new SalesLog()
                                    {
                                        EmployeeID = _loggedInUser.EmployeeID,
                                        GuestID = _selectedReservation.GuestID,
                                        TimeOfSale = DateTime.Now,
                                        PantryID = c.PantryID,
                                        SoldPrice = c.ItemPrice,
                                        ItemAmount = c.ItemAmount
                                    }));
                                    try
                                    {
                                        logList.ForEach(l => _roomChargeManager.CreateSalesLog(l));
                                        MessageBox.Show("Successfully created room charge & posted room charges");
                                        ResetPantryUI();
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Failed to post to the log", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Failed to post room charges", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                                ResetPantryUI();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to create room charge", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                            ResetPantryUI();
                            return;
                        }
                    }
                    else
                    {
                        try
                        {
                            _cart.ForEach(i =>
                            {
                                var pantryItem = _roomChargeManager.GetPantryItem(i.PantryID);
                                if (pantryItem.ItemAmount - i.ItemAmount > 0)
                                {
                                    i.ItemAmount = pantryItem.ItemAmount - i.ItemAmount;
                                    i.ItemPrice = i.ItemAmount * pantryItem.ItemPrice;
                                }
                                else
                                {
                                    MessageBox.Show($"The item {i.PantryID} will go out of inventory after this sale because of insuffecient quantities, Sale canceled", "Inventory warning", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                                    return;
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to retrieve pantry item \n Cannot proccess sale", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        try
                        {
                            var logList = new List<SalesLog>();
                            _cart.ForEach(c => logList.Add(new SalesLog()
                            {
                                EmployeeID = _loggedInUser.EmployeeID,
                                GuestID = _selectedReservation.GuestID,
                                TimeOfSale = DateTime.Now,
                                PantryID = c.PantryID,
                                SoldPrice = c.ItemPrice,
                                ItemAmount = c.ItemAmount
                            }));
                            try
                            {
                                logList.ForEach(l => _roomChargeManager.CreateSalesLog(l));
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Failed to log the sale into the system", "Log fail", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            _roomChargeManager.PostRoomCharges(_roomChargeManager.GetRoomCharge(_selectedReservation.ReservationID).RoomChargeID, _cart);
                            MessageBox.Show("Successfully posted room charges");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to post room charges", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                            ResetPantryUI();
                            return;
                        }
                        ResetPantryUI();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Not all required information was supplied", "Need more info", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Discarded sale", "Canceled", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            }
        }

        private void buttonPantrySalesLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var salesLogWindow = new SalesLogWindow(_roomChargeManager.GetSalesLogList());
                salesLogWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed to retrieve sales log", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void gridPantryDisplayGuest_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _selectedReservation = gridPantryDisplayGuest.SelectedItem as ReservationVM;
        }

        private void SumItemPrices(int price)
        {
            if(labelPantryTotalPrice.Content != "")
            {
                labelPantryTotalPrice.Content = $"${int.Parse(labelPantryTotalPrice.Content.ToString().Substring(1)) + price}";
            }
            else
            {
                labelPantryTotalPrice.Content = $"${price}";
            }
        }

        private void buttonPantryCandy_Click(object sender, RoutedEventArgs e)
        {
            gridPantryDisplaySales.ItemsSource = null;
            try
            {
                var pantryItem = _roomChargeManager.GetPantryItem("Candy");
                if(pantryItem.ItemAmount > 0)
                {
                    gridPantryDisplaySales.ItemsSource = null;
                    if (_cart.Count < 1 || _cart.Where(p => p.PantryID == "Candy").Count() < 1)
                    {
                        _cart.Add(new Pantry()
                        {
                            PantryID = "Candy",
                            ItemPrice = pantryItem.ItemPrice,
                            ItemAmount = 1
                        });
                    }
                    else
                    {
                        var cartItem = _cart.First(p => p.PantryID == "Candy");
                        cartItem.ItemPrice += pantryItem.ItemPrice;
                        cartItem.ItemAmount++;
                    }
                    SumItemPrices(pantryItem.ItemPrice);
                    gridPantryDisplaySales.ItemsSource = _cart;
                }
                else
                {
                    MessageBox.Show("Candy is out of stock", "Out of stock", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to retrieve candy from inventory", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonPantryChips_Click(object sender, RoutedEventArgs e)
        {
            gridPantryDisplaySales.ItemsSource = null;
            try
            {
                var pantryItem = _roomChargeManager.GetPantryItem("Chips");
                if (pantryItem.ItemAmount > 0)
                {
                    gridPantryDisplaySales.ItemsSource = null;
                    if (_cart.Count < 1 || _cart.Where(p => p.PantryID == "Chips").Count() < 1)
                    {
                        _cart.Add(new Pantry()
                        {
                            PantryID = "Chips",
                            ItemPrice = pantryItem.ItemPrice,
                            ItemAmount = 1
                        });
                    }
                    else
                    {
                        var cartItem = _cart.First(p => p.PantryID == "Chips");
                        cartItem.ItemPrice += pantryItem.ItemPrice;
                        cartItem.ItemAmount++;
                    }
                    SumItemPrices(pantryItem.ItemPrice);
                    gridPantryDisplaySales.ItemsSource = _cart;
                }
                else
                {
                    MessageBox.Show("Chips is out of stock", "Out of stock", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to retrieve chips from inventory", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonPantryJuice_Click(object sender, RoutedEventArgs e)
        {
            gridPantryDisplaySales.ItemsSource = null;
            try
            {
                var pantryItem = _roomChargeManager.GetPantryItem("Juice");
                if (pantryItem.ItemAmount > 0)
                {
                    gridPantryDisplaySales.ItemsSource = null;
                    if (_cart.Count < 1 || _cart.Where(p => p.PantryID == "Juice").Count() < 1)
                    {
                        _cart.Add(new Pantry()
                        {
                            PantryID = "Juice",
                            ItemPrice = pantryItem.ItemPrice,
                            ItemAmount = 1
                        });
                    }
                    else
                    {
                        var cartItem = _cart.First(p => p.PantryID == "Juice");
                        cartItem.ItemPrice += pantryItem.ItemPrice;
                        cartItem.ItemAmount++;
                    }
                    SumItemPrices(pantryItem.ItemPrice);
                    gridPantryDisplaySales.ItemsSource = _cart;
                }
                else
                {
                    MessageBox.Show("Juice is out of stock", "Out of stock", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to retrieve juice from inventory", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonPantrySoda_Click(object sender, RoutedEventArgs e)
        {
            gridPantryDisplaySales.ItemsSource = null;
            try
            {
                var pantryItem = _roomChargeManager.GetPantryItem("Soda");
                if (pantryItem.ItemAmount > 0)
                {
                    gridPantryDisplaySales.ItemsSource = null;
                    if (_cart.Count < 1 || _cart.Where(p => p.PantryID == "Soda").Count() < 1)
                    {
                        _cart.Add(new Pantry()
                        {
                            PantryID = "Soda",
                            ItemPrice = pantryItem.ItemPrice,
                            ItemAmount = 1
                        });
                    }
                    else
                    {
                        var cartItem = _cart.First(p => p.PantryID == "Soda");
                        cartItem.ItemPrice += pantryItem.ItemPrice;
                        cartItem.ItemAmount++;
                    }
                    SumItemPrices(pantryItem.ItemPrice);
                    gridPantryDisplaySales.ItemsSource = _cart;
                }
                else
                {
                    MessageBox.Show("Soda is out of stock", "Out of stock", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to retrieve soda from inventory", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonPantrySnack_Click(object sender, RoutedEventArgs e)
        {
            gridPantryDisplaySales.ItemsSource = null;
            try
            {
                var pantryItem = _roomChargeManager.GetPantryItem("Snack");
                if (pantryItem.ItemAmount > 0)
                {
                    gridPantryDisplaySales.ItemsSource = null;
                    if (_cart.Count < 1 || _cart.Where(p => p.PantryID == "Snack").Count() < 1)
                    {
                        _cart.Add(new Pantry()
                        {
                            PantryID = "Snack",
                            ItemPrice = pantryItem.ItemPrice,
                            ItemAmount = 1
                        });
                    }
                    else
                    {
                        var cartItem = _cart.First(p => p.PantryID == "Snack");
                        cartItem.ItemPrice += pantryItem.ItemPrice;
                        cartItem.ItemAmount++;
                    }
                    SumItemPrices(pantryItem.ItemPrice);
                    gridPantryDisplaySales.ItemsSource = _cart;
                }
                else
                {
                    MessageBox.Show("Snack is out of stock", "Out of stock", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to retrieve snack from inventory", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonPantryWater_Click(object sender, RoutedEventArgs e)
        {
            gridPantryDisplaySales.ItemsSource = null;
            try
            {
                var pantryItem = _roomChargeManager.GetPantryItem("Water");
                if (pantryItem.ItemAmount > 0)
                {
                    gridPantryDisplaySales.ItemsSource = null;
                    if (_cart.Count < 1 || _cart.Where(p => p.PantryID == "Water").Count() < 1)
                    {
                        _cart.Add(new Pantry()
                        {
                            PantryID = "Water",
                            ItemPrice = pantryItem.ItemPrice,
                            ItemAmount = 1
                        });
                    }
                    else
                    {
                        var cartItem = _cart.First(p => p.PantryID == "Water");
                        cartItem.ItemPrice += pantryItem.ItemPrice;
                        cartItem.ItemAmount++;
                    }
                    SumItemPrices(pantryItem.ItemPrice);
                    gridPantryDisplaySales.ItemsSource = _cart;
                }
                else
                {
                    MessageBox.Show("Water is out of stock", "Out of stock", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to retrieve water from inventory", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonPantryInventory_Click(object sender, RoutedEventArgs e)
        {
           var inventoryWindow = new InventoryWindow(_roomChargeManager.GetPantryItems());
           inventoryWindow.ShowDialog();
        }

        private void ResetPantryUI()
        {
            gridPantryDisplaySales.ItemsSource = null;
            gridPantryDisplayGuest.ItemsSource = null;
            textBoxPantryBoxFirstName.Text = string.Empty;
            textBoxPantryBoxLastName.Text = string.Empty;
            textBoxPantryRoomNumber.Text = string.Empty;
            labelPantryTotalPrice.Content = string.Empty;
            _cart = new List<Pantry>();
        }

        #endregion

        #region View Rooms Tab

        private void checkBoxAll_Checked(object sender, RoutedEventArgs e)
        {
            if (_selectedRoom != null && buttonViewRoomsEdit.Content == "Save")
            {
                var userChoice = MessageBox.Show($"Are you sure you want to discard changes to selected room {_selectedRoom.RoomID}", "Discard changes", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (userChoice == MessageBoxResult.OK)
                {
                    ClearViewRoomsTab();
                    buttonReservationEdit.Content = "Edit";
                }
            }
            else
            {
                if (checkBoxInspected.IsChecked == true)
                {
                    checkBoxInspected.IsChecked = false;
                }
                if (checkBoxDirty.IsChecked == true)
                {
                    checkBoxDirty.IsChecked = false;
                }
                try
                {
                    ClearViewRoomsTab();
                    gridViewRoomsDisplay.ItemsSource =  _roomManager.GetAllRooms();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not find rooms under {"All"}", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void checkBoxInspected_Checked(object sender, RoutedEventArgs e)
        {
            if (_selectedRoom != null && buttonViewRoomsEdit.Content == "Save")
            {
                var userChoice = MessageBox.Show($"Are you sure you want to discard changes to selected room {_selectedRoom.RoomID}", "Discard changes", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (userChoice == MessageBoxResult.OK)
                {
                    ClearViewRoomsTab();
                    buttonReservationEdit.Content = "Edit";
                }
            }
            else
            {
                if (checkBoxAll.IsChecked == true)
                {
                    checkBoxAll.IsChecked = false;
                }
                if (checkBoxDirty.IsChecked == true)
                {
                    checkBoxDirty.IsChecked = false;
                }
                try
                {
                    ClearViewRoomsTab();
                    gridViewRoomsDisplay.ItemsSource = _roomManager.GetRoomsByStatus("Inspected");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not find rooms under {"Inspected"}", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void checkBoxDirty_Checked(object sender, RoutedEventArgs e)
        {
            if (_selectedRoom != null && buttonViewRoomsEdit.Content == "Save")
            {
                var userChoice = MessageBox.Show($"Are you sure you want to discard changes to selected room {_selectedRoom.RoomID}", "Discard changes", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (userChoice == MessageBoxResult.OK)
                {
                    ClearViewRoomsTab();
                    buttonReservationEdit.Content = "Edit";
                }
            }
            else
            {
                if (checkBoxAll.IsChecked == true)
                {
                    checkBoxAll.IsChecked = false;
                }
                if (checkBoxInspected.IsChecked == true)
                {
                    checkBoxInspected.IsChecked = false;
                }
                try
                {
                    ClearViewRoomsTab();
                    gridViewRoomsDisplay.ItemsSource = _roomManager.GetRoomsByStatus("Dirty");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Could not find rooms under {"Dirty"}", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void buttonViewRoomsEdit_Click(object sender, RoutedEventArgs e)
        {
            if(_selectedRoom != null)
            {
                if (buttonViewRoomsEdit.Content.ToString() == "Edit")
                {
                    buttonViewRoomsEdit.Content = "Save";
                    buttonViewRoomsCancel.Visibility = Visibility.Visible;
                    comboBoxViewRoomAvailability.IsEnabled = true;
                    textBoxViewRoomDescription.IsEnabled = true;
                    textBoxViewRoomPrice.IsReadOnly = false;
                    comboBoxViewRoomStaus.IsEnabled = true;
                    comboBoxViewRoomType.IsEnabled = true;
                    comboBoxReservationRoomStatus.IsEnabled = true;
                }
                else
                {
                    buttonViewRoomsEdit.Content = "Edit";
                    buttonViewRoomsCancel.Visibility = Visibility.Hidden;
                    try
                    {
                        _roomManager.UpdateRoom(new Room()
                        {
                            RoomID = int.Parse(textBoxViewRoomRoomID.Text),
                            RoomTypeID = comboBoxViewRoomType.Text,
                            RoomAvailability = bool.Parse(comboBoxViewRoomAvailability.Text),
                            RoomStatus = comboBoxViewRoomStaus.Text
                        }, _selectedRoom);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to update room", ex.ToString(), MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
                    try
                    {
                        _roomManager.UpdateRoomType(new RoomType()
                        {
                            RoomTypeID = comboBoxViewRoomType.Text,
                            RoomPrice = int.Parse(textBoxViewRoomPrice.Text),
                            RoomDescription = textBoxViewRoomDescription.Text
                        }, _selectedRoom.RoomType);
                        MessageBox.Show("Successfully saved changes", "Success" ,MessageBoxButton.OK, MessageBoxImage.Information);
                        gridViewRoomsDisplay.ItemsSource = null;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to update room type", ex.ToString(), MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void ClearViewRoomsTab()
        {
            checkBoxAll.IsChecked = false;
            checkBoxInspected.IsChecked = false;
            checkBoxDirty.IsChecked = false;
            gridViewRoomsDisplay.ItemsSource = null;
            comboBoxViewRoomAvailability.Text = string.Empty;
            textBoxViewRoomDescription.Text = string.Empty;
            textBoxViewRoomPrice.Text = string.Empty;
            textBoxViewRoomRoomID.Text = string.Empty;
            comboBoxViewRoomStaus.Text = string.Empty;
            comboBoxViewRoomType.Text = string.Empty;
            comboBoxReservationRoomStatus.Text = string.Empty;

            comboBoxViewRoomAvailability.IsEnabled = false;
            textBoxViewRoomDescription.IsEnabled = false;
            textBoxViewRoomRoomID.IsReadOnly = true;
            comboBoxViewRoomStaus.IsEnabled = false;
            comboBoxViewRoomType.IsEnabled = false;
            comboBoxReservationRoomStatus.IsEnabled = false;
        }
        private void gridViewRoomsDisplay_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (buttonViewRoomsEdit.Content != "Save")
            {
                if (gridViewRoomsDisplay.SelectedItem != null)
                {
                    _selectedRoom = gridViewRoomsDisplay.SelectedItem as RoomVM;

                    var roomStatuses = new List<string>()
                    {
                      "Inspected", "Dirty", "Out", "DND"
                    };
                    comboBoxViewRoomStaus.ItemsSource = roomStatuses;

                    var roomAvailability = new List<string>()
                    {
                      "True", "False"
                    };
                    comboBoxViewRoomAvailability.ItemsSource = roomAvailability;

                    var roomTypes = new List<string>()
                    {
                      "Single", "Double"
                    };
                    comboBoxViewRoomType.ItemsSource = roomTypes;

                    comboBoxViewRoomAvailability.Text = _selectedRoom.RoomAvailability.ToString();
                    textBoxViewRoomDescription.Text = _selectedRoom.RoomType.RoomDescription;
                    textBoxViewRoomPrice.Text = _selectedRoom.RoomPrice.ToString();
                    textBoxViewRoomRoomID.Text = _selectedRoom.RoomID.ToString();
                    comboBoxViewRoomStaus.Text = _selectedRoom.RoomStatus;
                    comboBoxViewRoomType.Text = _selectedRoom.RoomTypeID;
                }
            }
        }
        private void buttonViewRoomsCancel_Click(object sender, RoutedEventArgs e)
        {
            if(buttonViewRoomsEdit.Content == "Save")
            {
                buttonViewRoomsEdit.Content = "Edit";
                ClearViewRoomsTab();
            }
        }
        #endregion

    }
}
