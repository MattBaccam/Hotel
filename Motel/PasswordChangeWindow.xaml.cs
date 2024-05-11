using DataObjects;
using LogicLayer;
using LogicLayerInterfaces;
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
    /// Interaction logic for PasswordChangeWindow.xaml
    /// </summary>
    public partial class PasswordChangeWindow : Window
    {
        private int _employeeID;
        public PasswordChangeWindow(int employeeID)
        {
            _employeeID = employeeID;
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            EmployeeManager _employeeManager = new EmployeeManager();

            string oldPassword = textBoxOldPassword.Password;
            string newPassword = textBoxNewPassword.Password;
            string retypePassword = textBoxConfirmPassword.Password;

            if (newPassword == "newuser")
            {
                MessageBox.Show("Nice try. Now really change your password from the default", "Busted", MessageBoxButton.OK, MessageBoxImage.Hand);
                textBoxOldPassword.Password = "";
                textBoxNewPassword.Password = "";
                textBoxConfirmPassword.Password = "";
                textBoxOldPassword.Focus();
                return;
            }

            if (newPassword == oldPassword)
            {
                MessageBox.Show("Nice try. Now really change your password", "Busted", MessageBoxButton.OK, MessageBoxImage.Hand);
                textBoxOldPassword.Password = "";
                textBoxNewPassword.Password = "";
                textBoxConfirmPassword.Password = "";
                textBoxOldPassword.Focus();
                return;
            }
            if(Helpers.IsValidPassword(newPassword))
            {
                try
                {
                    if (_employeeManager.ResetPassword(_employeeID, textBoxOldPassword.Password, textBoxNewPassword.Password))
                    {
                        this.DialogResult = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    this.DialogResult = false;
                }
            }
            else
            {
                MessageBox.Show("Invalid password", "Length must be greater than 7", MessageBoxButton.OK, MessageBoxImage.Hand);
                textBoxOldPassword.Password = "";
                textBoxNewPassword.Password = "";
                textBoxConfirmPassword.Password = "";
                textBoxOldPassword.Focus();
                return;
            }
        }
    }
}
