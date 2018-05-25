using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_LED.ViewModel
{
    public class LineControlViewModel : BaseViewModel
    {
        #region Property
        public int SelectedLineType
        {
            get
            {
                return _selectedLineType;
            }
            set
            {
                _selectedLineType = value;
                OnPropertyChanged("SelectedLineType");
            }
        }

        public int DataAddress
        {
            get
            {
                return _dataAddress;
            }
            set
            {
                _dataAddress = value;
                OnPropertyChanged("DataAddress");
            }
        }

        public string ParameterValue
        {
            get
            {
                return _parameterValue;
            }
            set
            {
                _parameterValue = value;
                OnPropertyChanged("ParameterValue");
            }
        }

        public string Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
                OnPropertyChanged("Unit");
            }
        }

        public string ScrollingMessage
        {
            get
            {
                return _scrollingMessage;
            }
            set
            {
                _scrollingMessage = value;
                OnPropertyChanged("ScrollingMessage");
            }
        }

        public int SelectedScrollingType
        {
            get
            {
                return _selectedScrollingType;
            }
            set
            {
                _selectedScrollingType = value;
                OnPropertyChanged("SelectedScrollingType");
            }
        }

        public int SelectedSpeed
        {
            get
            {
                return _selectedSpeed;
            }
            set
            {
                _selectedSpeed = value;
                OnPropertyChanged("SelectedSpeed");
            }
        }

        public int SelectedCharacterType
        {
            get
            {
                return _selectedCharacterType;
            }
            set
            {
                _selectedCharacterType = value;
                OnPropertyChanged("SelectedCharacterType");
            }
        }

        public string BlinkingMessage1
        {
            get
            {
                return _blinkingMessage1;
            }
            set
            {
                _blinkingMessage1 = value;
                OnPropertyChanged("BlinkingMessage1");
            }
        }

        public string BlinkingMessage2
        {
            get
            {
                return _blinkingMessage2;
            }
            set
            {
                _blinkingMessage2 = value;
                OnPropertyChanged("BlinkingMessage2");
            }
        }
        #endregion

        #region Constructor

        #endregion

        #region Field
        private int _selectedLineType;
        private string _parameterValue;
        private string _scrollingMessage;
        private int _dataAddress;
        private string _unit;
        private int _selectedScrollingType;
        private int _selectedSpeed;
        private int _selectedCharacterType;
        private string _blinkingMessage1;
        private string _blinkingMessage2;
        #endregion
    }
}
