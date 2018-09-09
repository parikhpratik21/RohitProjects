using Central_LED.Entity;
using Central_LED.Helper;
using Central_LED.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Central_LED
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            MainWindowViewModel viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;
            dataViewModel = viewModel;

            Uri iconUri = new Uri("pack://application:,,,/images.ico", UriKind.RelativeOrAbsolute);

            this.Icon = BitmapFrame.Create(iconUri);
        }
        #endregion

        #region Event Handler
        private void miDisplay_Click(object sender, RoutedEventArgs e)
        {
            DisplaySettingViewModel viewModel = new DisplaySettingViewModel();
            DisplaySetting displaySetting = new DisplaySetting(viewModel);
            displaySetting.ShowDialog();
         
            if (dataViewModel != null)
            {
                dataViewModel.SetDisplayData();
                LiveDisplay.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void miModbus_Click(object sender, RoutedEventArgs e)
        {
            StopSerialConnection();
            ModbusSettingViewModel viewModel = new ModbusSettingViewModel();
            ModbusSetting modbusSetting = new ModbusSetting(viewModel);
            modbusSetting.ShowDialog();
            InitializeSerialConnection();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LiveDisplay.Visibility = System.Windows.Visibility.Visible;           
            if (dataViewModel != null)
            {
                var selectedItem = dataList.SelectedItem as DisplayControlViewModel;
                if (selectedItem != null)
                {
                    dataViewModel.SelectedDisplayControl = selectedItem;
                    LiveDisplay.DisplayLiveData(selectedItem.DisplayData);                    
                }
            }
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {                
                if (dataViewModel != null && dataViewModel.ModbusData != null)
                {
                    System.Threading.Thread.Sleep(100);
                    byteBufferData = new byte[1000];
                    int dataToRead = serialPort.BytesToRead;
                    totalByteReceived = totalByteReceived + dataToRead;

                    serialPort.Read(byteBufferData, 0, dataToRead);
                   
                    if (totalByteReceived >=  9)
                    {
                        totalByteReceived = 0;
                        bool returnCRC = checkCRC(ref byteBufferData, byteBufferData[6] + 9);
                        if(returnCRC)
                        {
                            //process buffer data                        
                            _slaveId = Convert.ToInt32(byteBufferData[0]);
                            _modbusFunction = Convert.ToInt32(byteBufferData[1]);
                            _startingAddress = ConvertByteToInteger(byteBufferData, 2);
                            _dataLength = ConvertByteToInteger(byteBufferData, 4);
                            
                            byte[] messageToSend = createRespondMessage(byteBufferData);
                            System.Threading.Thread.Sleep(10);
                            serialPort.Write(messageToSend, 0, messageToSend.Length);
                            
                            Array.Copy(byteBufferData, copyByteBufferData, 1000);                            

                            newDataReceived = true;
                        }                       
                    }
                }
            }
            catch(Exception ex)
            {
                byteBufferData = new byte[1000];
                totalByteReceived = 0;               
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                isThreadAborted = true;
                dataProcessingThread.Abort();
            }
            catch (Exception ex) { }
        }

        #endregion

        #region Private Methods
        private  void InitializeSerialConnection()
        {
            try
            {
                if (dataViewModel != null && dataViewModel.ModbusData != null)
                {
                    serialPort = new SerialPort();
                    serialPort.PortName = dataViewModel.ModbusData.ComPort;
                    serialPort.Parity = Parity.None;
                    serialPort.DataBits = 8;
                    serialPort.BaudRate = 9600;
                    serialPort.DataReceived += serialPort_DataReceived;
                    serialPort.Open();

                    isThreadAborted = false;
                    dataProcessingThread = new Thread(new ThreadStart(ProcessData));
                    dataProcessingThread.Start();
                }
            }
            catch(Exception ex)
            { }
        }
       
        private void StopSerialConnection()
        {
            try
            {
                serialPort.DataReceived -= serialPort_DataReceived;
                serialPort.Close();
                isThreadAborted = true;
                dataProcessingThread.Abort();
                dataProcessingThread = null;
            }
            catch(Exception ex)
            { }
        }
        private void ProcessData()
        {
            while (isThreadAborted == false)
            {
                Thread.Sleep(100);
                try
                {
                    if (newDataReceived)
                    {
                        var copyBufferArray = new byte[1000];
                        Array.Copy(copyByteBufferData, 7, copyBufferArray, 0, 1000 - 7);
                      
                        newDataReceived = false;

                        if (dataViewModel != null && dataViewModel.DisplayControlList != null)
                        {
                            foreach (var displayControl in dataViewModel.DisplayControlList)
                            {
                                if (displayControl.LineList != null && displayControl.LineList.Any())
                                {
                                    int index = 0;
                                    foreach (var lineData in displayControl.LineList)
                                    {
                                        index = index + 1;
                                        if (lineData.LineData.Type == (int)LineType.Static)
                                        {
                                            if (lineData.LineData.StaticDataAddressList != null && lineData.LineData.StaticDataAddressList.Any())
                                            {
                                                bool isValueChange = false;
                                                List<float> DataColumnValues = new List<float>();

                                                foreach (var staticLine in lineData.LineData.StaticDataAddressList)
                                                {
                                                    if (staticLine.DataAddress >= _startingAddress && staticLine.DataAddress < _startingAddress + _dataLength)
                                                    {
                                                        int startingBufferIndex = (staticLine.DataAddress - _startingAddress) * 2;
                                                        if (startingBufferIndex <= 0)
                                                        {
                                                            startingBufferIndex = 0;
                                                        }
                                                        var parameterValue = ConvertByteToFloat(copyBufferArray, startingBufferIndex);
                                                        DataColumnValues.Add(parameterValue);
                                                        if (parameterValue != staticLine.DataValue)
                                                        {
                                                            isValueChange = true;
                                                            staticLine.DataValue = parameterValue;
                                                        }
                                                    }
                                                }

                                                if (isValueChange)
                                                {
                                                   
                                                    string valueText = "NKM_LINE" + index.ToString() + ":" + ((int)LineType.Static).ToString() + ",D";
                                                    //  + lineData.ParameterValue + "\r\n";

                                                    foreach (var staticLine in lineData.LineData.StaticDataAddressList)
                                                    {
                                                        float dataValue = staticLine.DataValue;
                                                        if (staticLine.DataValue > 9999)
                                                        {
                                                            dataValue = Convert.ToSingle(Math.Round(staticLine.DataValue));
                                                        }
                                                        else if(staticLine.DataValue.ToString().Length > 5)
                                                        {
                                                            if(staticLine.DataValue > 999)
                                                            {
                                                                dataValue = Convert.ToSingle(Math.Round(staticLine.DataValue));
                                                            }
                                                            else if (staticLine.DataValue > 99)
                                                            {
                                                                dataValue = Convert.ToSingle(Math.Round(staticLine.DataValue,1));
                                                            }
                                                            else
                                                            {
                                                                dataValue = Convert.ToSingle(Math.Round(staticLine.DataValue, 2));
                                                            }
                                                        }
                                                        staticLine.DataValue = dataValue;
                                                        valueText = valueText + "," + dataValue;
                                                    }

                                                    valueText = valueText + "\r\n";
                                                    SendCommandDataToController(displayControl.DisplayData, valueText);
                                                }                                                
                                            }                                                                                     
                                        }
                                        else if (lineData.DataAddress >= _startingAddress && lineData.DataAddress <= _startingAddress + _dataLength)
                                        {
                                            if (lineData.LineData.Type == (int)LineType.Scrolling)
                                            {
                                                if (lineData.DataAddress >= _startingAddress && lineData.DataAddress < _startingAddress + _dataLength)
                                                {
                                                    int startingBufferIndex = (lineData.DataAddress - _startingAddress) * 2;
                                                    if (startingBufferIndex <= 0)
                                                    {
                                                        startingBufferIndex = 0;
                                                    }
                                                    var scrollingValue = ConvertByteToFloat(copyBufferArray, startingBufferIndex);
                                                    if (scrollingValue != lineData.ScrollingValue)
                                                    {
                                                        lineData.ScrollingValue = scrollingValue;
                                                        int commandValue = 0;
                                                        if (lineData.ScrollingValue >= 0.8)
                                                        {
                                                            commandValue = 1;
                                                        }
                                                        string valueText = "NKM_LINE" + index.ToString() + ":" + ((int)LineType.Scrolling).ToString() + ",D,"
                                                            + commandValue.ToString() + "\r\n";

                                                        SendCommandDataToController(displayControl.DisplayData, valueText);
                                                    }
                                                }
                                            }
                                            else if (lineData.LineData.Type == (int)LineType.Blinking)
                                            {
                                                if (lineData.DataAddress >= _startingAddress && lineData.DataAddress < _startingAddress + _dataLength)
                                                {
                                                    int startingBufferIndex1 = (lineData.DataAddress - _startingAddress) * 2;
                                                    if (startingBufferIndex1 <= 0)
                                                    {
                                                        startingBufferIndex1 = 0;
                                                    }

                                                    int startingBufferIndex2 = (lineData.Blinking2DataAddress - _startingAddress) * 2;
                                                    if (startingBufferIndex2 <= 0)
                                                    {
                                                        startingBufferIndex2 = 0;
                                                    }

                                                    var blinkingValue1 = ConvertByteToFloat(copyBufferArray, startingBufferIndex1);
                                                    var blinkingValue2 = ConvertByteToFloat(copyBufferArray, startingBufferIndex2);
                                                    if (blinkingValue1 != lineData.BlinkingValue1 || blinkingValue2 != lineData.BlinkingValue2)
                                                    {
                                                        lineData.BlinkingValue1 = blinkingValue1;
                                                        lineData.BlinkingValue2 = blinkingValue2;
                                                        int commandValue1 = 0;
                                                        if (lineData.BlinkingValue1 >= 0.8)
                                                        {
                                                            commandValue1 = 1;
                                                        }
                                                        int commandValue2 = 0;
                                                        if (lineData.BlinkingValue2 >= 0.8)
                                                        {
                                                            commandValue2 = 1;
                                                        }

                                                        string valueText = "NKM_LINE" + index.ToString() + ":" + ((int)LineType.Blinking).ToString() + ",D,"
                                                            + commandValue1 + "," + commandValue2 + "\r\n";

                                                        SendCommandDataToController(displayControl.DisplayData, valueText);
                                                    }
                                                }
                                            }                                           
                                        }
                                    }
                                }
                            }

                            //update current display
                            UpdateCurrentDisplay();
                        }
                    }
                }
                catch(Exception ex)
                {

                }                
            }
        }

        private void UpdateCurrentDisplay()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
            {
                var selectedItem = dataList.SelectedItem as DisplayControlViewModel;
                if (selectedItem != null)
                {
                    LiveDisplay.UpdateLiveDisplay(selectedItem);
                }
            })); 
        }

        private void SendCommandDataToController(Display displayData, string data)
        {
            try
            {                                
                byte[] bb = new byte[1000];
                byte[] ba = new byte[1000];
                ASCIIEncoding asen = new ASCIIEncoding();              

                if (data != null || data != string.Empty)
                {
                    TcpClient tcpclnt = new TcpClient();
                    tcpclnt.Connect(displayData.IP, Convert.ToInt32(displayData.Port));
                    Stream stm = tcpclnt.GetStream();                 
                    bb = asen.GetBytes(data);
                    stm.WriteTimeout = 2000;
                    stm.Write(bb, 0, bb.Length);
                    tcpclnt.Close();
                }               
            }
            catch(Exception ex)
            {

            }
        }

        private byte[] createRespondMessage(byte[] _messageReceived)
        {
            byte[] respondMessage = new byte[8];
            respondMessage[0] = _messageReceived[0];// _slaveAddress;
            respondMessage[1] = _messageReceived[1];
            respondMessage[2] = _messageReceived[2];
            respondMessage[3] = _messageReceived[3];
            respondMessage[4] = _messageReceived[4];
            respondMessage[5] = _messageReceived[5];

            byte[] crcCalculation = calculateCRC(ref respondMessage, 6);
            respondMessage[6] = crcCalculation[0];
            respondMessage[7] = crcCalculation[1];
            return respondMessage;
        }

        private int ConvertByteToInteger(byte[] buffereArray, int startingIndex)
        {
            try
            {
                byte[] tempData = new byte[2];
                tempData[0] = buffereArray[startingIndex + 1];
                tempData[1] = buffereArray[startingIndex];
                return BitConverter.ToUInt16(tempData, 0);
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        private float ConvertByteToFloat(byte[] buffereArray, int startingIndex)
        {
            try
            {
                byte[] tempData = new byte[4];
                tempData[0] = buffereArray[startingIndex + 3];
                tempData[1] = buffereArray[startingIndex + 2];
                tempData[2] = buffereArray[startingIndex + 1];
                tempData[3] = buffereArray[startingIndex];
                return BitConverter.ToSingle(tempData, 0);
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        private bool checkCRC(ref byte[] messageToCheck, int numberOfBytes)
        {
            byte[] calculatedCRC;
            calculatedCRC = calculateCRC(ref messageToCheck, numberOfBytes - 2);
            if (calculatedCRC[0] == messageToCheck[numberOfBytes - 2] && calculatedCRC[1] == messageToCheck[numberOfBytes - 1]) return true;
            return false;
        }

        private byte[] calculateCRC(ref byte[] messageArray, int dataLength)
        {
            byte usCRCHi = 0xFF;
            byte usCRCLo = 0xFF;
            byte[] returnResult = { 0x00, 0x00, 0x00 };
            int index = 0;
            int messageIndex = 0;
            while (dataLength > 0)
            {
                index = usCRCLo ^ messageArray[messageIndex];
                usCRCLo = Convert.ToByte(usCRCHi ^ crcHi[index]);
                usCRCHi = crcLo[index];
                messageIndex++;
                dataLength--;
            }
            //0th item is crcLo
            returnResult[0] = usCRCLo;
            //1st item is crcHi
            returnResult[1] = usCRCHi;
            //2nd item is the total CRC16.
            //returnResult[2] = Convert.ToByte((usCRCHi << 8 | usCRCLo));
            return returnResult;
        }

        #endregion

        #region Fields
        MainWindowViewModel dataViewModel;
        Thread dataProcessingThread;
        SerialPort serialPort;
        bool newDataReceived = false;
        bool isThreadAborted = false;
        int _slaveId;
        int _startingAddress;
        int _dataLength;
        int _modbusFunction;

        int totalByteReceived = 0;       
        static byte[] byteBufferData = new byte[1000];
        static byte[] copyByteBufferData = new byte[1000];

        static byte[] crcHi = {
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
        0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
        0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
        0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81,
        0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
        0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
        0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
        0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01,
        0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
        0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01,
        0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81,
        0x40
        };

        static byte[] crcLo = {
        0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7, 0x05, 0xC5, 0xC4,
        0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09,
        0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD,
        0x1D, 0x1C, 0xDC, 0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
        0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32, 0x36, 0xF6, 0xF7,
        0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A,
        0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE,
        0x2E, 0x2F, 0xEF, 0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
        0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1, 0x63, 0xA3, 0xA2,
        0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
        0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB,
        0x7B, 0x7A, 0xBA, 0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
        0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0, 0x50, 0x90, 0x91,
        0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C,
        0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E, 0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88,
        0x48, 0x49, 0x89, 0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
        0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83, 0x41, 0x81, 0x80,
        0x40
        };
        #endregion        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeSerialConnection();
        }       

        private void MiAboutUs_Click(object sender, RoutedEventArgs e)
        {
            Central_LED.View.AboutUs frmAboutUs = new View.AboutUs();
            frmAboutUs.ShowDialog();
        }
    }
}
