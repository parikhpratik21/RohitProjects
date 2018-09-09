using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        Timer tmrTimer;
        List<string> availableBarcodes;       

        public MainWindow()
        {
            InitializeComponent();

            tmrTimer = new Timer();
            tmrTimer.Elapsed += tmrTimer_Elapsed;
            tmrTimer.Interval = 100;
        }

        void tmrTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                tmrTimer.Enabled = false;
                this.Dispatcher.Invoke(new Action(() => Processbarcode()));                
            }
            catch(Exception)
            {

            }            
        }

        private void Processbarcode()
        {
            if (txtBarcodeValue.Text == null || txtBarcodeValue.Text == string.Empty
                || txtBarcodeValue.Text.Length == 0)
            {
                return;
            }
            lock (txtBarcodeValue)
            {
                var barcodeValue = txtBarcodeValue.Text;
                txtBarcodeValue.Text = string.Empty;
                if (availableBarcodes.Contains(barcodeValue) == false)
                {
                    txtTotalBarcode.Text = (Convert.ToInt32(txtTotalBarcode.Text) + 1).ToString();
                    availableBarcodes.Add(barcodeValue);
                    txtBarcodeScaneMessage.Text = "Barcode successfully added to excel.";

                    var xlApp = new Microsoft.Office.Interop.Excel.Application();
                    xlApp.DisplayAlerts = false;
                    var xlWorkBook = xlApp.Workbooks.Open(txtFilePath.Text, System.Reflection.Missing.Value,
                       System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                       System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                       System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                       System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                    var xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);  
                    try
                    {
                        xlWorkSheet.Cells[availableBarcodes.Count, 1] = barcodeValue;

                        xlWorkBook.SaveAs(txtFilePath.Text, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                        false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    }
                    finally
                    {
                        xlWorkBook.Close();
                        xlApp.Quit();
                        Marshal.ReleaseComObject(xlWorkBook);
                        Marshal.ReleaseComObject(xlApp);
                    }                                      
                }
                else
                {
                    txtBarcodeScaneMessage.Text = "Barcode already exist.";
                }
            }
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

        private void txtBarcodeValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            tmrTimer.Enabled = false;
            tmrTimer.Enabled = true;
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            availableBarcodes = new List<string>();

            var xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlApp.DisplayAlerts = false;
            var xlWorkBook = xlApp.Workbooks.Open(txtFilePath.Text, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value);

            var xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);           

            try
            {
                Microsoft.Office.Interop.Excel.Range range = xlWorkSheet.UsedRange;
                int rowCount = range.Rows.Count;
                for (int row = 1; row <= rowCount; row++)
                {
                    var barcodeValue = (range.Cells[row, 1] as Microsoft.Office.Interop.Excel.Range).Value2;
                    if ((barcodeValue != null && barcodeValue.GetType() != typeof(string))
                        || (barcodeValue != null && barcodeValue.GetType() == typeof(string) && barcodeValue != string.Empty))
                    {
                        availableBarcodes.Add(barcodeValue.ToString());
                    }
                }

                txtTotalBarcode.Text = availableBarcodes.Count.ToString();
            }
            finally
            {
                xlWorkBook.Close();
                xlApp.Quit();
                Marshal.ReleaseComObject(xlWorkBook);
                Marshal.ReleaseComObject(xlApp);
            }                      

            txtBarcodeValue.Focus();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (xlWorkBook != null)
            //{
            //    xlWorkBook.SaveAs(txtFilePath.Text, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
            //        false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
            //        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //    xlWorkBook.Close();               

            //    Marshal.ReleaseComObject(xlWorkBook);
            //    Marshal.ReleaseComObject(xlApp);
            //}
        }
    }
}
