using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_LED.ViewModel
{
    public class DisplaySettingViewModel : BaseViewModel
    {
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
        #endregion

        #region Constructor

        #endregion

        #region Field        
        private List<DisplayControlViewModel> _displayControlList;
        private DisplayControlViewModel _selectedDisplayControl;
        #endregion
    }
}
