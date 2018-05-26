using Central_LED.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Central_LED
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }        

        private void miDisplay_Click(object sender, RoutedEventArgs e)
        {
            DisplaySettingViewModel viewModel = new DisplaySettingViewModel();
            DisplaySetting displaySetting = new DisplaySetting(viewModel);
            displaySetting.ShowDialog();
        }

        private void miModbus_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
