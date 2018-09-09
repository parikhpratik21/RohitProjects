using Central_LED.DAL;
using Central_LED.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_LED.ViewModel
{
    public class ModbusSettingViewModel : BaseViewModel
    {
        #region Property
       
        public List<string> ComPortList
        {
            get
            {
                return _comPortList;
            }
            set
            {
                _comPortList = value;
                OnPropertyChanged("ComPortList");
            }
        }
        public string ComPort
        {
            get
            {
                return _modbusData.ComPort;
            }
            set
            {
                _modbusData.ComPort = value;
                OnPropertyChanged("ComPort");
            }
        }
        public int DeviceId
        {
            get
            {
                return _modbusData.DeviceId;
            }
            set
            {
                _modbusData.DeviceId = value;
                OnPropertyChanged("DeviceId");
            }
        }
        public int Address
        {
            get
            {
                return _modbusData.Address;
            }
            set
            {
                _modbusData.Address = value;
                OnPropertyChanged("Address");
            }
        }
        public int Length
        {
            get
            {
                return _modbusData.Length;
            }
            set
            {
                _modbusData.Length = value;
                OnPropertyChanged("Length");
            }
        }
        public int UpdateTime
        {
            get
            {
                return _modbusData.UpdateTime;
            }
            set
            {
                _modbusData.UpdateTime = value;
                OnPropertyChanged("UpdateTime");
            }
        }
        public int Type
        {
            get
            {
                return _modbusData.Type;
            }
            set
            {
                _modbusData.Type = value;
                OnPropertyChanged("Type");
            }
        }

        public int DataType
        {
            get
            {
                return _modbusData.DataType;
            }
            set
            {
                _modbusData.DataType = value;
                OnPropertyChanged("DataType");
            }
        }

        public int FunctionType
        {
            get
            {
                return _modbusData.FunctionType;
            }
            set
            {
                _modbusData.FunctionType = value;
                OnPropertyChanged("FunctionType");
            }
        }

        public List<AddressDataViewModel> FirstAddressData
        {
            get
            {
                return _firstAddressData;
            }
            set
            {
                _firstAddressData = value;
                OnPropertyChanged("FirstAddressData");
            }
        }

        public List<AddressDataViewModel> SecondAddressData
        {
            get
            {
                return _secondAddressData;
            }
            set
            {
                _secondAddressData = value;
                OnPropertyChanged("SecondAddressData");
            }
        }
        #endregion

        #region Constructor
        public ModbusSettingViewModel()
        {
            ComPortList = System.IO.Ports.SerialPort.GetPortNames().ToList();

            GetModbusData();

        }
        #endregion

        #region Methods
                
        private void GetModbusData()
        {
            DbConnection dbConnection = new DbConnection();
            var data = dbConnection.GetModbusSetting();
            SetModbusData(data);
        }

        private void SetModbusData(Modbus modbusData)
        {
            _modbusData = modbusData;
            OnPropertyChanged("UpdateTime");
            OnPropertyChanged("Length");
            OnPropertyChanged("Address");
            OnPropertyChanged("DeviceId");
            OnPropertyChanged("ComPort");
        }

        public bool SaveMoubusSettings()
        {
            DbConnection dbConnection = new DbConnection();
            return dbConnection.SaveModbusSetting(_modbusData);
        }

        public string ValidateSetting()
        {
            if(string.IsNullOrEmpty(ComPort))
            {
                return "Please select valid com port";
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Field
        private Modbus _modbusData;
        private List<string> _comPortList;
        private List<AddressDataViewModel> _firstAddressData;
        private List<AddressDataViewModel> _secondAddressData;
        #endregion
        
    }
}
