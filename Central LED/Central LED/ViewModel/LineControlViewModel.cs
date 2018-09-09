using Central_LED.Entity;
using Central_LED.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_LED.ViewModel
{
    public class LineControlViewModel : BaseViewModel
    {
        #region Events
        public delegate void UpdateSelectedLine(LineControlViewModel dataViewModel);
        public UpdateSelectedLine OnUpdateSelectedLine;
        #endregion

        #region Property
        public bool IsModified
        {
            get;
            set;
        }
        public int SelectedLineType
        {
            get
            {
                return LineData.Type;
            }
            set
            {
                LineData.Type = value;
                SetDefaultData();
                IsModified = true;
                OnPropertyChanged("SelectedLineType");
            }
        }

        public int DataAddress
        {
            get
            {
                return LineData.DataAddress;
            }
            set
            {
                LineData.DataAddress = value;
                IsModified = true;
                OnPropertyChanged("DataAddress");
            }
        }

        public string ParameterName
        {
            get
            {
                return LineData.ParameterName;
            }
            set
            {
                LineData.ParameterName = value;
                IsModified = true;
                OnPropertyChanged("ParameterName");
            }
        }

        public float ParameterValue
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

        public float ScrollingValue
        {
            get
            {
                return _scrollingValue;
            }
            set
            {
                _scrollingValue = value;
                OnPropertyChanged("ScrollingValue");
            }
        }
        public float BlinkingValue1
        {
            get
            {
                return _blinkingValue1;
            }
            set
            {
                _blinkingValue1 = value;
                OnPropertyChanged("BlinkingValue1");
            }
        }
        public float BlinkingValue2
        {
            get
            {
                return _blinkingValue2;
            }
            set
            {
                _blinkingValue2 = value;
                OnPropertyChanged("BlinkingValue2");
            }
        }

        public string Unit
        {
            get
            {
                return LineData.Unit;
            }
            set
            {
                LineData.Unit = value;
                IsModified = true;
                OnPropertyChanged("Unit");
            }
        }

        public string ScrollingMessage
        {
            get
            {
                return LineData.ScrollingMessage;
            }
            set
            {
                LineData.ScrollingMessage = value;
                IsModified = true;
                OnPropertyChanged("ScrollingMessage");
            }
        }

        public int SelectedScrollingType
        {
            get
            {
                return LineData.ScrollingType;
            }
            set
            {
                LineData.ScrollingType = value;
                IsModified = true;
                OnPropertyChanged("SelectedScrollingType");
            }
        }

        public int SelectedScrollingType2
        {
            get
            {
                return LineData.ScrollingType2;
            }
            set
            {
                LineData.ScrollingType2 = value;
                IsModified = true;
                OnPropertyChanged("SelectedScrollingType2");
            }
        }

        public int SelectedSpeed
        {
            get
            {
                return LineData.Speed;
            }
            set
            {
                LineData.Speed = value;
                IsModified = true;
                OnPropertyChanged("SelectedSpeed");
            }
        }

        public int SelectedCharacterType
        {
            get
            {
                return LineData.CharacterType;
            }
            set
            {
                LineData.CharacterType = value;
                IsModified = true;
                OnPropertyChanged("SelectedCharacterType");
            }
        }

        public List<StaticLineDataColumns> DataColumnList
        {
            get
            {
                return LineData.StaticDataAddressList;
            }
            set
            {
                LineData.StaticDataAddressList = value;
                OnPropertyChanged("DataColumnList");
            }
        }

        public StaticLineDataColumns SelectedDataColumn
        {
            get
            {
                return _selectedDataColumn;
            }
            set
            {
                _selectedDataColumn = value;
                OnPropertyChanged("SelectedDataColumn");
                OnPropertyChanged("SelectedDataColumnDataAddress");
            }
        }

        public int SelectedDataColumnDataAddress
        {
            get
            {
                if(_selectedDataColumn != null)
                {
                    return _selectedDataColumn.DataAddress;
                }

                return 0;
            }
            set
            {
                if (_selectedDataColumn != null)
                {
                    _selectedDataColumn.DataAddress = value; ;
                }
                OnPropertyChanged("SelectedDataColumnDataAddress");
            }
        }
        public string CommandCharacterType
        {
            get
            {
                if(SelectedCharacterType == (int)Helper.CharacterType.Small)
                {
                    return "S";
                }
                else
                {
                    return "B";
                }
            }
        }

        public string CommandScrollingType
        {
            get
            {
                if (SelectedScrollingType == (int)Helper.LineActionType.Auto)
                {
                    return "A";
                }
                else
                {
                    return "M";
                }
            }
        }

        public string CommandScrollingType2
        {
            get
            {
                if (SelectedScrollingType2 == (int)Helper.LineActionType.Auto)
                {
                    return "A";
                }
                else
                {
                    return "M";
                }
            }

        }
        public string BlinkingMessage1
        {
            get
            {
                return LineData.BlinkingMessage1;
            }
            set
            {
                LineData.BlinkingMessage1 = value;
                IsModified = true;
                OnPropertyChanged("BlinkingMessage1");
            }
        }

        public string BlinkingMessage2
        {
            get
            {
                return LineData.BlinkingMessage2;
            }
            set
            {
                LineData.BlinkingMessage2 = value;
                IsModified = true;
                OnPropertyChanged("BlinkingMessage2");
            }
        }

        public int Blinking2DataAddress
        {
            get
            {
                return LineData.Blinking2DataAddress;
            }
            set
            {
                LineData.Blinking2DataAddress = value;
                IsModified = true;
                OnPropertyChanged("Blinking2DataAddress");
            }
        }

        public string LineName
        {
            get
            {
                return LineData.LineName;
            }
            set
            {
                LineData.LineName = value;
                IsModified = true;
                OnPropertyChanged("LineName");
            }
        }

        public Line LineData
        {
            get
            {
                return _lineData;
            }
            set
            {
                _lineData = value;
                IsModified = true;
                OnPropertyChanged("LineData");
            }
        }
       
        #endregion

        #region Constructor
        public LineControlViewModel()
        {
            LineData = new Line();

        }
        #endregion

        #region Method
        private void SetDefaultData()
        {
            if(SelectedLineType == (int)LineType.Static)
            {
                this.ParameterName = "Parameter";
                this.Unit = "Unit";
                this.DataAddress = 0;
            }
            else if(SelectedLineType == (int)LineType.Scrolling)
            {
                this.ScrollingMessage = "Write message here!";
                this.DataAddress = 0;
            }
            else if(SelectedLineType == (int)LineType.Blinking)
            {
                this.BlinkingMessage1 = "Blink message 1";
                this.BlinkingMessage2 = "Blink message 2";
                this.DataAddress = 0;
            }

            if(OnUpdateSelectedLine != null)
            {
                OnUpdateSelectedLine.Invoke(this);
            }
        }
        #endregion

        #region Field
        private Line _lineData;
        private float _parameterValue;
        private float _scrollingValue;
        private float _blinkingValue1;
        private float _blinkingValue2;
        private StaticLineDataColumns _selectedDataColumn;
        #endregion
    }
}
