using Central_LED.DAL;
using Central_LED.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_LED.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Event
        
        #endregion

        #region Property

        public List<DisplayControlViewModel> DisplayControlList
        {
            get
            {
                return _displayControlList;
            }
            set
            {
                _displayControlList = value;
                OnPropertyChanged("DisplayControlList");
            }
        }

        public DisplayControlViewModel SelectedDisplayControl
        {
            get
            {
                return _selectedDisplayControl;
            }
            set
            {
                _selectedDisplayControl = value;
                OnPropertyChanged("SelectedDisplayControl");
            }
        }

        public Modbus ModbusData
        {
            get
            {
                return _modbusData;
            }
            set
            {
                _modbusData = value;
                OnPropertyChanged("ModbusData");
            }
        }
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            SetDisplayData();
            GetModbusData();
        }
        #endregion

        #region Method

        #region Private Method
        public void SetDisplayData()
        {
            DbConnection dbConnection = new DbConnection();
            var displayDataList = dbConnection.GetDisplayList();
            DisplayControlList = new List<DisplayControlViewModel>();
            foreach(var display in displayDataList)
            {
                DisplayControlViewModel data = new DisplayControlViewModel();
                data.SetData(display);
                DisplayControlList.Add(data);
            }
            DisplayControlList = new List<DisplayControlViewModel>(DisplayControlList);            
        }

        public void GetModbusData()
        {
            DbConnection dbConnection = new DbConnection();
            ModbusData = dbConnection.GetModbusSetting();
        }
        #endregion

        #endregion

        #region Field
        private List<DisplayControlViewModel> _displayControlList;
        private DisplayControlViewModel _selectedDisplayControl;
        private Modbus _modbusData;
        #endregion
    }
}
