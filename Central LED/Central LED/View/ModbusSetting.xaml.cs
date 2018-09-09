using Central_LED.ViewModel;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using System.Windows.Threading;

namespace Central_LED
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ModbusSetting : Window
    {
        public ModbusSetting(ModbusSettingViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            _viewModel = viewModel;

            Uri iconUri = new Uri("pack://application:,,,/images.ico", UriKind.RelativeOrAbsolute);

            this.Icon = BitmapFrame.Create(iconUri);
        }        
     
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (btnConnect.Content.ToString() == "Connect")
            {
                UpdateDisplayOnConnect();
                InitializeSerialConnection();
                btnConnect.Content = "Disconnect";
            }
            else if (btnConnect.Content.ToString() == "Disconnect")
            {
                StopSerialConnection();
                if(_viewModel != null)
                {
                    _viewModel.FirstAddressData = new List<AddressDataViewModel>();
                    _viewModel.SecondAddressData = new List<AddressDataViewModel>();
                }
                btnConnect.Content = "Connect";
            }
        }

        private void UpdateDisplayOnConnect()
        {
            _viewModel.FirstAddressData = new List<AddressDataViewModel>();
            _viewModel.SecondAddressData = new List<AddressDataViewModel>();

            int displayDataLength = _viewModel.Length / 2;
            for (int index = 0; index < displayDataLength; index++)
            {
                AddressDataViewModel newData = new AddressDataViewModel();
                newData.Address = _viewModel.Address + index;
                newData.Value = "0";
                _viewModel.FirstAddressData.Add(newData);

                AddressDataViewModel newData1 = new AddressDataViewModel();
                newData1.Address = _viewModel.Address + index;
                newData1.Value = "0";
                _viewModel.FirstAddressData.Add(newData1);
            }

            for (int index = 0; index < displayDataLength; index++)
            {
                AddressDataViewModel newData = new AddressDataViewModel();
                newData.Address = _viewModel.Address + index + displayDataLength;
                newData.Value = "0";
                _viewModel.SecondAddressData.Add(newData);

                AddressDataViewModel newData1 = new AddressDataViewModel();
                newData1.Address = _viewModel.Address + index + displayDataLength;
                newData1.Value = "0";
                _viewModel.SecondAddressData.Add(newData1);
            }

            _viewModel.FirstAddressData = new List<AddressDataViewModel>(_viewModel.FirstAddressData);
            _viewModel.SecondAddressData = new List<AddressDataViewModel>(_viewModel.SecondAddressData);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                var validateResult = _viewModel.ValidateSetting();
                if (validateResult == null)
                {
                    bool result = _viewModel.SaveMoubusSettings();
                    if (result)
                    {
                        MessageBox.Show("Data saved successfully", "Central LED");
                    }
                    else
                    {
                        MessageBox.Show("Error while saving moudbus data, Please try again", "Cetral LED", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show(validateResult, "Cetral LED", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void InitializeSerialConnection()
        {
            try
            {
                if (_viewModel != null)
                {
                    serialPort = new SerialPort();
                    serialPort.PortName = _viewModel.ComPort;
                    serialPort.Parity = Parity.None;
                    serialPort.DataBits = 8;
                    serialPort.BaudRate = 9600;
                    serialPort.StopBits = StopBits.One;
                    serialPort.DataReceived += serialPort_DataReceived;
                    serialPort.Open();               
                }
            }
            catch (Exception ex)
            { }
        }

        private void StopSerialConnection()
        {
            try
            {
                serialPort.DataReceived -= serialPort_DataReceived;
                serialPort.Close();
            }
            catch (Exception ex)
            { }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopSerialConnection();
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (_viewModel != null)
                {
                    byteBufferData = new byte[1000];
                    System.Threading.Thread.Sleep(100);
                    int dataToRead = serialPort.BytesToRead;
                    totalByteReceived = dataToRead;

                    serialPort.Read(byteBufferData, 0, dataToRead);
                  
                    if (totalByteReceived >=  9)
                    {
                        totalByteReceived = 0;                       
                        bool returnCRC = checkCRC(ref byteBufferData, byteBufferData[6] + 9);
                        if (returnCRC)
                        {
                            //process buffer data                        
                            _slaveId = Convert.ToInt32(byteBufferData[0]);
                            _modbusFunction = Convert.ToInt32(byteBufferData[1]);
                            _startingAddress = ConvertByteToInteger(byteBufferData, 2);
                            _dataLength = Convert.ToInt32(byteBufferData[5]);

                            byte[] messageToSend = createRespondMessage(byteBufferData);
                            System.Threading.Thread.Sleep(10);
                            serialPort.Write(messageToSend, 0, messageToSend.Length);

                            Array.Copy(byteBufferData, copyByteBufferData, 1000);                            

                            UpdateCurrentDisplay();                            
                        }
                    }                    
                }
            }
            catch (Exception ex)
            {
                byteBufferData = new byte[1000];
                totalByteReceived = 0;
                //oldBufferLength = 0;
            }
        }

        private void UpdateCurrentDisplay()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
            {
                if (_viewModel != null)
                {                    
                    for (int index = _startingAddress; index < _startingAddress + _dataLength; index++)
                    {                        
                        var relatedDataList = _viewModel.FirstAddressData.Where(data => data.Address == index).ToList();
                        if(relatedDataList != null && relatedDataList.Count > 1)
                        {
                            var relatedData1 = relatedDataList[0];
                            relatedData1.Value = byteBufferData[(relatedData1.Address - _startingAddress) * 2 + 7].ToString();

                            var relatedData2 = relatedDataList[1];
                            relatedData2.Value = byteBufferData[(relatedData2.Address - _startingAddress) * 2 + 7 + 1].ToString();
                        }
                        else
                        {
                            var relatedDataList2 = _viewModel.SecondAddressData.Where(data => data.Address == index).ToList();
                            if(relatedDataList2 != null && relatedDataList2.Count > 1)
                            {
                                var relatedData1 = relatedDataList2[0];
                                relatedData1.Value = byteBufferData[(relatedData1.Address - _startingAddress) * 2 + 7].ToString();

                                var relatedData2 = relatedDataList2[1];
                                relatedData2.Value = byteBufferData[(relatedData2.Address - _startingAddress) * 2 + 7 + 1].ToString();
                            }
                        }                       
                    }                   
                    _viewModel.FirstAddressData = new List<AddressDataViewModel>(_viewModel.FirstAddressData);
                    _viewModel.SecondAddressData = new List<AddressDataViewModel>(_viewModel.SecondAddressData);
                }
               
            }));
        }

        private int ConvertByteToInteger(byte[] buffereArray, int startingIndex)
        {
            try
            {
                byte[] tempData = new byte[2];
                tempData[0] = buffereArray[startingIndex + 1];
                tempData[1] = buffereArray[startingIndex];
                var data = BitConverter.ToUInt16(tempData, 0);
                return (int)data;
            }
            catch (Exception ex)
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
        #region Field        
        private ModbusSettingViewModel _viewModel;

        private int _slaveId;
        private int _modbusFunction;
        private int _startingAddress;
        private int _dataLength;
        SerialPort serialPort;

        int totalByteReceived = 0;
        //int oldBufferLength = 0;
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

       
    }
}
