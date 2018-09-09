using Central_LED.DAL;
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
        public DisplaySettingViewModel()
        {
            GetDisplayData();
        }
        #endregion

        #region Method

        #region Private Method
        private void GetDisplayData()
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
        #endregion

        #endregion

        #region Field
        private List<DisplayControlViewModel> _displayControlList;
        private DisplayControlViewModel _selectedDisplayControl;
        #endregion
    }
}
