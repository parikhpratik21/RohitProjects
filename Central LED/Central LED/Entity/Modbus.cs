using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_LED.Entity
{
    public class Modbus
    {
        public int ID
        {
            get;
            set;
        }
        public string ComPort
        {
            get;
            set;
        }
        public int DeviceId
        {
            get;
            set;
        }
        public int Address
        {
            get;
            set;
        }
        public int Length
        {
            get;
            set;
        }
        public int UpdateTime
        {
            get;
            set;
        }
        public int FunctionType
        {
            get;
            set;
        }
        public int Type
        {
            get;
            set;
        }
        public int DataType
        {
            get;
            set;
        }
    }
}
