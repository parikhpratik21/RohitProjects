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

namespace Central_LED.Controls
{
    /// <summary>
    /// Interaction logic for LineSettingControl.xaml
    /// </summary>
    public partial class LineSettingControl : UserControl
    {
        public LineSettingControl()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(DataContext != null)
            {
                MainGrid.Visibility = System.Windows.Visibility.Visible; 
            }
            else
            {
                MainGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
