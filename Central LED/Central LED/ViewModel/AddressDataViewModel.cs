using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_LED.ViewModel
{
    public class AddressDataViewModel : BaseViewModel
    {
        #region Property
        public int Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged("Address");
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }
        #endregion

        #region Field
        private int _address;
        private string _value;
        #endregion
    }
}
