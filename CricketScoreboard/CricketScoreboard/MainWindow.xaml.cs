using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.IO.Ports;
namespace CricketScoreboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, INotifyPropertyChanged
    {
        #region "Declaration"
        private SerialPort _ComPort;
        private string _Title;
        private string _Total;
        private string _Overs;
        private string _OversBall;
        private string _Wickets;
        private string _FirstInnings;
        #endregion

        #region OnPropertyChange
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion

        #region "Property"
        public string TitleText
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                OnPropertyChanged("TitleText");
            }
        }
        public string Total
        {
            get
            {
                return _Total;
            }
            set
            {
                _Total = value;
                OnPropertyChanged("Total");
            }
        }
        public string Overs
        {
            get
            {
                return _Overs;
            }
            set
            {
                _Overs = value;
                OnPropertyChanged("Overs");
            }
        }
        public string OversBall
        {
            get
            {
                return _OversBall;
            }
            set
            {
                _OversBall = value;
                OnPropertyChanged("OversBall");
            }
        }
        
        public string Wickets
        {
            get
            {
                return _Wickets;
            }
            set
            {
                _Wickets = value;
                OnPropertyChanged("Wickets");
            }
        }
        public string FirstInnings
        {
            get
            {
                return _FirstInnings;
            }
            set
            {
                _FirstInnings = value;
                OnPropertyChanged("FirstInnings");
            }
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;
        }       

        //private void btnTitleSend_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        _ComPort = new SerialPort();
        //        _ComPort.PortName = cmbComPort.Text;
        //        //_ComPort.Parity = Parity.Even;
        //        _ComPort.DataBits = 8;
        //        _ComPort.BaudRate = 9600;
        //        _ComPort.Open();
        //        string sCmdData = "NKMLINE1_DISP:" + txtTitle.Text + "\r\n";
        //        _ComPort.Write(sCmdData);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        if (_ComPort != null && _ComPort.IsOpen == true)
        //            _ComPort.Close();
        //        _ComPort = null;
        //    }
        //}

        private void bbtnTotalSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ComPort = new SerialPort();
                _ComPort.PortName = cmbComPort.Text;
                //_ComPort.Parity = Parity.Even;
                _ComPort.DataBits = 8;
                _ComPort.BaudRate = 9600;
                _ComPort.Open();
                string sCmdData = "NKMLINE2:" + txtTotal.Text + "\r\n";
                _ComPort.Write(sCmdData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (_ComPort != null && _ComPort.IsOpen == true)
                    _ComPort.Close();
                _ComPort = null;
            }
        }

        private void btnOversSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ComPort = new SerialPort();
                _ComPort.PortName = cmbComPort.Text;
                //_ComPort.Parity = Parity.Even;
                _ComPort.DataBits = 8;
                _ComPort.BaudRate = 9600;
                _ComPort.Open();
                string sCmdData = "NKMLINE3:" + txtOvers.Text +  "\r\n";
                _ComPort.Write(sCmdData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (_ComPort != null && _ComPort.IsOpen == true)
                    _ComPort.Close();
                _ComPort = null;
            }
        }

        private void btnWicketsSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ComPort = new SerialPort();
                _ComPort.PortName = cmbComPort.Text;
                //_ComPort.Parity = Parity.Even;
                _ComPort.DataBits = 8;
                _ComPort.BaudRate = 9600;
                _ComPort.Open();
                string sCmdData = "NKMLINE4:" + txtWickets.Text + "\r\n";
                _ComPort.Write(sCmdData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (_ComPort != null && _ComPort.IsOpen == true)
                    _ComPort.Close();
                _ComPort = null;
            }
        }

        private void btnFirststInningSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _ComPort = new SerialPort();
                _ComPort.PortName = cmbComPort.Text;
                //_ComPort.Parity = Parity.Even;
                _ComPort.DataBits = 8;
                _ComPort.BaudRate = 9600;
                _ComPort.Open();
                string sCmdData = "NKMLINE5:" + txtFirstInnings.Text + "\r\n";
                _ComPort.Write(sCmdData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (_ComPort != null && _ComPort.IsOpen == true)
                    _ComPort.Close();
                _ComPort = null;
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string[] myPort = System.IO.Ports.SerialPort.GetPortNames();
            foreach(string oPort in myPort)
            {
                cmbComPort.Items.Add(oPort);
            }            
        }
    }
}
