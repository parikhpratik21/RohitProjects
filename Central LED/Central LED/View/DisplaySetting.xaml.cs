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
using System.Windows.Shapes;

namespace Central_LED
{
    /// <summary>
    /// Interaction logic for DisplaySetting.xaml
    /// </summary>
    public partial class DisplaySetting : Window
    {
        public DisplaySetting(DisplaySettingViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void btnAddDisplay_Click(object sender, RoutedEventArgs e)
        {
            DisplaySettingViewModel viewModel = DataContext as DisplaySettingViewModel;
            if(viewModel != null)
            {
                if(viewModel.DisplayControlList == null)
                {
                    viewModel.DisplayControlList = new List<DisplayControlViewModel>();
                }

                DisplayControlViewModel dataViewModel = new DisplayControlViewModel()
                {
                    DataColumns = 2,
                    IP = string.Empty,
                    LineList = DisplayControlViewModel.GenerateLine(12),
                    Lines = 12,
                    Name = "Display " + (viewModel.DisplayControlList.Count + 1).ToString(),                    
                };

                viewModel.DisplayControlList.Add(dataViewModel);
                viewModel.DisplayControlList = new List<DisplayControlViewModel>(viewModel.DisplayControlList);
                viewModel.SelectedDisplayControl = dataViewModel;
            }
        }

        private void btnRemoveDisplay_Click(object sender, RoutedEventArgs e)
        {
             DisplaySettingViewModel viewModel = DataContext as DisplaySettingViewModel;
             if (viewModel != null && viewModel.SelectedDisplayControl != null && viewModel.DisplayControlList != null)
             {
                 viewModel.DisplayControlList.Remove(viewModel.SelectedDisplayControl);
                 viewModel.DisplayControlList = new List<DisplayControlViewModel>(viewModel.DisplayControlList);
                 viewModel.SelectedDisplayControl = viewModel.DisplayControlList.FirstOrDefault();
             }
        }

        private void btnSaveDisplay_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
