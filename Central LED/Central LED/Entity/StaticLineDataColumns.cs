using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_LED.Entity
{
    public class StaticLineDataColumns
    {
        public int ID
        {
            get;
            set;
        }

        public int LineID
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }

        public string DataColumnName
        {
            get;
            set;
        }

        public int DataAddress
        {
            get;
            set;
        }

        public float DataValue
        {
            get;
            set;
        }

        public int DisplayControlId
        {
            get;
            set;
        }
    }
}
