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
    /// Interaction logic for SalesLogWindow.xaml
    /// </summary>
    public partial class SalesLogWindow : Window
    {
        public SalesLogWindow(List<SalesLogVM> salesList)
        {
            InitializeComponent();
            gridSalesLog.ItemsSource = salesList;
        }
    }
}
