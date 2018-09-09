using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_LED.Entity
{
    public class Line
    {
        public int ID
        {
            get;
            set;
        }

        public int DisplayControlID
        {
            get;
            set;
        }

        public int Type
        {
            get;
            set;
        }

        public int DataAddress
        {
            get;
            set;
        }

        public int Blinking2DataAddress
        {
            get;
            set;
        }

        public string ParameterName
        {
            get;
            set;
        }

        public string Unit
        {
            get;
            set;
        }

        public string ScrollingMessage
        {
            get;
            set;
        }

        public int ScrollingType
        {
            get;
            set;
        }

        public int ScrollingType2
        {
            get;
            set;
        }

        public int Speed
        {
            get;
            set;
        }

        public int CharacterType
        {
            get;
            set;
        }

        public string BlinkingMessage1
        {
            get;
            set;
        }

        public string BlinkingMessage2
        {
            get;
            set;
        }

        public string LineName
        {
            get;
            set;
        }

        public List<StaticLineDataColumns> StaticDataAddressList
        {
            get;
            set;
        }
    }
}
