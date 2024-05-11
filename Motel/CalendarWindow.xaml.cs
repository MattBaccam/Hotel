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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScenicHotel
{
    /// <summary>
    /// Interaction logic for CalendarWindow.xaml
    /// </summary>
    public partial class CalendarWindow : Window
    {
        public DateTime? _date;
        public CalendarWindow(DateTime date)
        {
            InitializeComponent();
            _date = date;
            labelCalendarDate.Content = date;
            calendar.DisplayDate = date;
        }

        private void buttonCalendarSave_Click(object sender, RoutedEventArgs e)
        {
            var userChoice = MessageBox.Show("Are you sure you want to save the current changes", "Save confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (userChoice == MessageBoxResult.OK)
            {
                this.DialogResult = true;
                this.Close();
            }
        }

        private void buttonCalendarDiscard_Click(object sender, RoutedEventArgs e)
        {
            var userChoice = MessageBox.Show("Are you sure you want to discard the current changes", "Cancel confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (userChoice == MessageBoxResult.OK)
            {
                this.DialogResult = false;
                this.Close();
            }
        }

        private void calendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        { 
            if (calendar.SelectedDate != null)
            {
                _date = calendar.SelectedDate;
                labelCalendarDate.Content = _date;
            }
        }
    }
}
