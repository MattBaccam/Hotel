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
    /// Interaction logic for ReservationCommentWindow.xaml
    /// </summary>
    public partial class ReservationCommentWindow : Window
    {
        private string _oldComments;
        private string _newComments;
        private int _reservationID;
        public ReservationCommentWindow(int reservationID, string comments)
        {
            InitializeComponent();
            _oldComments = comments;
            _reservationID = reservationID;
            textBoxComments.Text = _oldComments;
        }

        private void buttonDiscard_Click(object sender, RoutedEventArgs e)
        {
            var userChoice = MessageBox.Show("Are you sure you want to discard the current changes", "Cancel confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (userChoice == MessageBoxResult.OK)
            {
                this.DialogResult = false;
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userChoice = MessageBox.Show("Are you sure you want to save the current changes", "Save confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (userChoice == MessageBoxResult.OK)
                {
                    var reservationManager = new ReservationManager();
                    _newComments = textBoxComments.Text;
                    if (!(_newComments.Equals(_oldComments)))
                    {
                        reservationManager.SaveReservationComments(_reservationID, _newComments);
                        this.DialogResult = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Failed to save the comments of the reservation");
            }
        }
    }
}
