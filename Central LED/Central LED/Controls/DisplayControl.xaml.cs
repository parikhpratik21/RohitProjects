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

namespace Central_LED.Controls
{
    /// <summary>
    /// Interaction logic for DisplayControl.xaml
    /// </summary>
    public partial class DisplayControl : UserControl
    {
        public DisplayControl()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_isUpdateLineEventInitialize == false)
            {
                var displayViewModel = DataContext as DisplayControlViewModel;
                if (displayViewModel != null)
                {
                    displayViewModel.OnUpdateSelectedLine += UpdateSelectedLine;
                    _isUpdateLineEventInitialize = true;
                }
            }

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
            if (DataContext == null)
            {
                MainGrid.Visibility = System.Windows.Visibility.Collapsed;
            }

            var displayViewModel = DataContext as DisplayControlViewModel;
            if(displayViewModel != null)
            {
                displayViewModel.OnUpdateSelectedLine += UpdateSelectedLine;
                _isUpdateLineEventInitialize = true;
            }
        }

        private void UpdateSelectedLine()
        {
            var displayViewModel = DataContext as DisplayControlViewModel;
            if (displayViewModel != null)
            {
                lineSettingControl.UpdateSelectedLine(displayViewModel.SelectedLine);
            }
        }

        #region Field
        private bool _isUpdateLineEventInitialize;
        #endregion
    }
}
