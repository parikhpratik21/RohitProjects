using Central_LED.DAL;
using Central_LED.Entity;
using Central_LED.Helper;
using Central_LED.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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

            Uri iconUri = new Uri("pack://application:,,,/images.ico", UriKind.RelativeOrAbsolute);

            this.Icon = BitmapFrame.Create(iconUri);
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
                    LineList = DisplayControlViewModel.GenerateLine(12,2),
                    Lines = 12,
                    DisplayName = "Display " + (viewModel.DisplayControlList.Count + 1).ToString(),                    
                };

                dataViewModel.DisplayData.LineList = new List<Entity.Line>();
                foreach(var lineData in dataViewModel.LineList)
                {
                    dataViewModel.DisplayData.LineList.Add(lineData.LineData);
                }

                dataViewModel.IsModified = true;
                viewModel.DisplayControlList.Add(dataViewModel);
                viewModel.DisplayControlList = new List<DisplayControlViewModel>(viewModel.DisplayControlList);
                viewModel.SelectedDisplayControl = dataViewModel;
            }
        }

        private void btnRemoveDisplay_Click(object sender, RoutedEventArgs e)
        {
             var confirmationResult = MessageBox.Show("Are you sure you want to remove selected display data?","Central LED System",MessageBoxButton.YesNo);
             if (confirmationResult == MessageBoxResult.Yes)
             {
                 DisplaySettingViewModel viewModel = DataContext as DisplaySettingViewModel;
                 if (viewModel != null && viewModel.SelectedDisplayControl != null && viewModel.DisplayControlList != null)
                 {
                     DbConnection dbConnection = new DbConnection();
                     var result = dbConnection.RemoveDisplay(viewModel.SelectedDisplayControl.DisplayData);
                     if (result == true)
                     {
                         viewModel.DisplayControlList.Remove(viewModel.SelectedDisplayControl);
                         viewModel.DisplayControlList = new List<DisplayControlViewModel>(viewModel.DisplayControlList);
                         viewModel.SelectedDisplayControl = viewModel.DisplayControlList.FirstOrDefault();
                         MessageBox.Show("Data removed successfully", "Central LED System");
                     }
                     else
                     {
                         MessageBox.Show("Error while removing display data, Please try again", "Central LED System",MessageBoxButton.OK,MessageBoxImage.Error);
                     }
                 }
             }
        }

        private void btnSaveDisplay_Click(object sender, RoutedEventArgs e)
        {
            DisplaySettingViewModel viewModel = DataContext as DisplaySettingViewModel;
            if (viewModel != null && viewModel.DisplayControlList != null)
            {
                var modifiedDisplay = viewModel.DisplayControlList.Where(data => data.IsModified == true || data.LineList.Any(lineData => lineData.IsModified == true)).ToList();

                DbConnection dbConnection = new DbConnection();
                foreach(var displayData in modifiedDisplay)
                {
                    var result = dbConnection.CreateOrUpdateDisplay(displayData.DisplayData);
                    if(result == false)
                    {
                        MessageBox.Show("Error while saving display data, Please try again", "Central LED System");
                        return;
                    }
                }
                MessageBox.Show("Data saved successfully", "Central LED System");

                //set current display data to the controller
                SetSelectedDisplayDataToController();                
            }            
        }

        private void SetSelectedDisplayDataToController()
        {
            DisplaySettingViewModel viewModel = DataContext as DisplaySettingViewModel;
            bool result;
            if (viewModel != null && viewModel.SelectedDisplayControl != null)
           { 
                int index = 0;
                foreach (var lineData in viewModel.SelectedDisplayControl.LineList)
                {
                    index = index + 1;
                    if (viewModel.SelectedDisplayControl.SelectedLine != null && lineData.LineName == viewModel.SelectedDisplayControl.SelectedLine.LineName)
                    {
                        if (lineData.LineData.Type == (int)LineType.Static)
                        {
                            string commandText = "NKM_LINE" + index.ToString() + ":" + ((int)LineType.Static).ToString() + ",C,"
                                + lineData.ParameterName + "," + lineData.Unit + "\r\n";

                            result = SendCommandDataToController(viewModel.SelectedDisplayControl.DisplayData, commandText);

                            if (result == false)
                            {
                                MessageBox.Show("Error while sending data to controller, Please try again.", "Centrl LED", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        else if (lineData.LineData.Type == (int)LineType.Scrolling)
                        {
                            string commandText = "NKM_LINE" + index.ToString() + ":" + ((int)LineType.Scrolling).ToString() + ",C,"
                                   + lineData.CommandCharacterType + "," + lineData.CommandScrollingType + "," + lineData.SelectedSpeed +
                                   "," + lineData.ScrollingMessage + "\r\n";

                            result = SendCommandDataToController(viewModel.SelectedDisplayControl.DisplayData, commandText);

                            if (result == false)
                            {
                                MessageBox.Show("Error while sending data to controller, Please try again.", "Centrl LED", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        else if (lineData.LineData.Type == (int)LineType.Blinking)
                        {
                            string commandText = "NKM_LINE" + index.ToString() + ":" + ((int)LineType.Blinking).ToString() + ",C,"
                                     + lineData.SelectedSpeed + "," + lineData.CommandScrollingType + "," + lineData.BlinkingMessage1
                                     + "," + lineData.CommandScrollingType2 + "," + lineData.BlinkingMessage2 + "\r\n";

                            result = SendCommandDataToController(viewModel.SelectedDisplayControl.DisplayData, commandText);

                            if (result == false)
                            {
                                MessageBox.Show("Error while sending data to controller, Please try again.", "Centrl LED", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        else if (lineData.LineData.Type == (int)LineType.Blank)
                        {
                            string commandText = "NKM_LINE" + index.ToString() + ":" + ((int)LineType.Blank).ToString() + ",C" + "\r\n";

                            result = SendCommandDataToController(viewModel.SelectedDisplayControl.DisplayData, commandText);

                            if (result == false)
                            {
                                MessageBox.Show("Error while sending data to controller, Please try again.", "Centrl LED", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }

                        break;
                    }
                }

                MessageBox.Show("Data successfully sent to Controller", "Central LED System");
            }
        }

        private bool SendCommandDataToController(Display displayData, string command)
        {
            try
            {

                byte[] bb = new byte[1000];
                byte[] ba = new byte[1000];
                ASCIIEncoding asen = new ASCIIEncoding();

                if (command != null || command != string.Empty)
                {
                    TcpClient tcpclnt = new TcpClient();
                    tcpclnt.Connect(displayData.IP, Convert.ToInt32(displayData.Port));
                    Stream stm = tcpclnt.GetStream();
                    ba = asen.GetBytes(command);
                    stm.WriteTimeout = 1000;
                    stm.Write(ba, 0, ba.Length);

                    tcpclnt.Close();
                }             
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplaySettingViewModel viewModel = DataContext as DisplaySettingViewModel;
            if (viewModel != null && viewModel.DisplayControlList != null)
            {
                viewModel.SelectedDisplayControl = viewModel.DisplayControlList.FirstOrDefault();
            }
        }
    }
}
