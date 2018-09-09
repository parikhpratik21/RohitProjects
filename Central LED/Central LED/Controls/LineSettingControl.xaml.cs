using Central_LED.Helper;
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

                var dataViewModel = DataContext as LineControlViewModel;
                if (dataViewModel != null)
                {
                    dataViewModel.OnUpdateSelectedLine += UpdateSelectedLine;

                    if (dataViewModel != null && dataViewModel.SelectedLineType == (int)LineType.Static)
                    {
                        if (dataViewModel.DataColumnList != null && dataViewModel.DataColumnList.Any())
                        {
                            dataViewModel.SelectedDataColumn = dataViewModel.DataColumnList.FirstOrDefault();
                        }
                    }
                }
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
        }

       
        internal void UpdateSelectedLine(ViewModel.LineControlViewModel lineControlViewModel)
        {
            if(lineControlViewModel != null && lineControlViewModel.SelectedLineType == (int)LineType.Scrolling)
            {
                try
                {
                    var updatedString = lineControlViewModel.LineName.Replace("Line ", "");
                    var updatedNumner = Convert.ToInt32(updatedString);
                   
                    if (updatedNumner % 2 == 0)
                    {
                        if (cmbCharacterSize.Items.Count == 2)
                        {
                            cmbCharacterSize.Items.RemoveAt(1);
                        }
                    }
                    else
                    {
                        if (cmbCharacterSize.Items.Count == 1)
                        {
                            cmbCharacterSize.Items.Add("Big");
                        }
                    }
                }
                catch(Exception ex)
                { }
            } 
            else if(lineControlViewModel != null && lineControlViewModel.SelectedLineType == (int)LineType.Static)
            {
                if (lineControlViewModel.DataColumnList != null && lineControlViewModel.DataColumnList.Any())
                {
                    lineControlViewModel.SelectedDataColumn = lineControlViewModel.DataColumnList.FirstOrDefault();
                }
            }
        }
    }
}
