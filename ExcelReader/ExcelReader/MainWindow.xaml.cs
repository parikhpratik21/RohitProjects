using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BarcodeScanner
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



        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            var result = openFile.ShowDialog();
            if(result != null && result.Value)
            {
                txtFilePath.Text = openFile.FileName;
            }
        }      

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            var xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlApp.DisplayAlerts = false;
            var xlWorkBook = xlApp.Workbooks.Open(txtFilePath.Text, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value);

            txtBarcodeAnalys.Text = string.Empty;
            //var xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);           
            int totalWorksheet = xlWorkBook.Worksheets.Count;
            try
            {
                int allSheetCount = 0;
                for (int index = 1; index <= totalWorksheet; index++)
                {
                    var xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(index);
                    int worksheetCount = 0;
                    Microsoft.Office.Interop.Excel.Range range = xlWorkSheet.UsedRange;
                    int rowCount = range.Rows.Count;
                    int columnCount = range.Columns.Count;
                    for (int row = 1; row <= rowCount; row++)
                    {
                        for(int column = 1; column < columnCount; column++)
                        {
                            var barcodeValue = (range.Cells[row, column] as Microsoft.Office.Interop.Excel.Range).Value2;
                            if ((barcodeValue != null && barcodeValue.GetType() != typeof(string))
                                || (barcodeValue != null &&  barcodeValue.GetType() == typeof(string) && barcodeValue != string.Empty))
                            {
                                worksheetCount++;
                            }
                        }                                              
                    }

                    if (txtBarcodeAnalys.Text == string.Empty)
                    {
                        txtBarcodeAnalys.Text = txtBarcodeAnalys.Text + xlWorkSheet.Name + " Count: " + worksheetCount.ToString();
                    }
                    else
                    {
                        txtBarcodeAnalys.Text = txtBarcodeAnalys.Text + Environment.NewLine + xlWorkSheet.Name + " Count: " + worksheetCount.ToString();
                    }

                    allSheetCount = allSheetCount + worksheetCount;
                }

                txtTotalBarcode.Text = allSheetCount.ToString();
            }
            finally
            {
                xlWorkBook.Close();
                xlApp.Quit();
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);
            }                      

            txtBarcodeAnalys.Focus();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            SerialPort _ComPort = null;
            try
            {
                _ComPort = new SerialPort();
                _ComPort.PortName = cmbComPort.Text;
                //_ComPort.Parity = Parity.Even;
                _ComPort.DataBits = 8;
                _ComPort.BaudRate = Convert.ToInt32(cmbBaudrate.Text);
                _ComPort.Open();
                string sCmdData = "NKM_Count:" + txtTotalBarcode.Text + "\r\n";
                _ComPort.Write(sCmdData);

                MessageBox.Show("Total count send successfully");
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] myPort = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string oPort in myPort)
            {
                cmbComPort.Items.Add(oPort);
            } 
        }

       
    }
}
