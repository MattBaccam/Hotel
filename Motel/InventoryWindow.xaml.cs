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
    /// Interaction logic for InventoryWindow.xaml
    /// </summary>
    public partial class InventoryWindow : Window
    {
        private List<Pantry> _pantry = null;
        public InventoryWindow(List<Pantry> pantry)
        {
            InitializeComponent();
            _pantry = pantry;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var candy = _pantry.FirstOrDefault(p => p.PantryID == "Candy");
            var chips = _pantry.FirstOrDefault(p => p.PantryID == "Chips");
            var snack = _pantry.FirstOrDefault(p => p.PantryID == "Snack");
            var water = _pantry.FirstOrDefault(p => p.PantryID == "Water");
            var juice = _pantry.FirstOrDefault(p => p.PantryID == "Juice");
            var soda = _pantry.FirstOrDefault(p => p.PantryID == "Soda");

            textBoxCandyPrice.Text = candy.ItemPrice.ToString();
            textBoxCandyQuantity.Text = candy.ItemAmount.ToString();

            textBoxChipsPrice.Text = chips.ItemPrice.ToString();
            textBoxChipsQuantity.Text = chips.ItemAmount.ToString();

            textBoxSnackPrice.Text = snack.ItemPrice.ToString();
            textBoxSnackQuantity.Text = snack.ItemAmount.ToString();

            textBoxWaterPrice.Text = water.ItemPrice.ToString();
            textBoxWaterQuantity.Text = water.ItemAmount.ToString();

            textBoxJuicePrice.Text = juice.ItemPrice.ToString();
            textBoxJuiceQuantity.Text = juice.ItemAmount.ToString();

            textBoxSodaPrice.Text = soda.ItemPrice.ToString();
            textBoxSodaQuantity.Text = soda.ItemAmount.ToString();
        }

        private void buttonInventoryEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (buttonInventoryEdit.Content == "Save")
                {
                    var roomChargeManager = new RoomChargeManager();
                    var newItemList = new List<Pantry>()
                    {
                    new Pantry()
                    {
                        PantryID = "Candy"
                        , ItemPrice = int.Parse(textBoxCandyPrice.Text)
                        , ItemAmount = int.Parse(textBoxCandyQuantity.Text)
                    },
                    new Pantry()
                    {
                        PantryID = "Chips"
                        , ItemPrice = int.Parse(textBoxChipsPrice.Text)
                        , ItemAmount = int.Parse(textBoxChipsQuantity.Text)
                    },
                    new Pantry()
                    {
                        PantryID = "Snack"
                        , ItemPrice = int.Parse(textBoxSnackPrice.Text)
                        , ItemAmount = int.Parse(textBoxSnackQuantity.Text)
                    },
                    new Pantry()
                    {
                        PantryID = "Water"
                        , ItemPrice = int.Parse(textBoxWaterPrice.Text)
                        , ItemAmount = int.Parse(textBoxWaterQuantity.Text)
                    },
                    new Pantry()
                    {
                        PantryID = "Juice"
                        , ItemPrice = int.Parse(textBoxJuicePrice.Text)
                        , ItemAmount = int.Parse(textBoxJuiceQuantity.Text)
                    },
                    new Pantry()
                    {
                        PantryID = "Soda"
                        , ItemPrice = int.Parse(textBoxSodaPrice.Text)
                        , ItemAmount = int.Parse(textBoxSodaQuantity.Text)
                    }
                    };
                    roomChargeManager.UpdateInventory(newItemList, _pantry);
                    MessageBox.Show("Changes successfully saved", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LockUi(true);
                    buttonInventoryEdit.Content = "Edit";
                }
                else
                {
                    LockUi(false);
                    buttonInventoryEdit.Content = "Save";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save inventory changes", ex.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonInventoryDiscard_Click(object sender, RoutedEventArgs e)
        {
            var userChoice = MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (userChoice == MessageBoxResult.OK)
            {
                this.Close();
            }
        }

        private void LockUi(bool status)
        {
            textBoxCandyPrice.IsReadOnly = status;
            textBoxCandyQuantity.IsReadOnly = status;

            textBoxChipsPrice.IsReadOnly = status;
            textBoxChipsQuantity.IsReadOnly = status;

            textBoxSnackPrice.IsReadOnly = status;
            textBoxSnackQuantity.IsReadOnly = status;

            textBoxWaterPrice.IsReadOnly = status;
            textBoxWaterQuantity.IsReadOnly = status;

            textBoxJuicePrice.IsReadOnly = status;
            textBoxJuiceQuantity.IsReadOnly = status;

            textBoxSodaPrice.IsReadOnly = status;
            textBoxSodaQuantity.IsReadOnly = status;
        }
    }
}
