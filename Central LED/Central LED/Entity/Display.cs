using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_LED.Entity
{
    public class Display
    {
        public int ID
        {
            get;
            set;
        }

        public string IP
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public int Lines
        {
            get;
            set;
        }

        public int DataColumns
        {
            get;
            set;
        }       

        public List<Line> LineList
        {
            get;
            set;
        }       

        public string DisplayName
        {
            get;
            set;
        }
    }
}
