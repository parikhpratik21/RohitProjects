using Central_LED.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_LED.ViewModel
{
    public class DisplayControlViewModel : BaseViewModel
    {
        #region Property
        public string IP
        {
            get
            {
                return _ip;
            }
            set
            {
                _ip = value;
                OnPropertyChanged("IP");
            }
        }

        public int Lines
        {
            get
            {
                return _lines;
            }
            set
            {
                _lines = value;
                OnPropertyChanged("Lines");                
                CreateLineViewModelBasedonTotalLines();
            }
        }

        public int DataColumns
        {
            get
            {
                return _dataColumns;
            }
            set
            {
                _dataColumns = value;
                OnPropertyChanged("DataColumns");
                OnPropertyChanged("Columns");
            }
        }

        public int Columns
        {
            get
            {
                return DataColumns + 2;
            }
        }

        public List<LineControlViewModel> LineList
        {
            get
            {
                return _lineList;
            }
            set
            {
                _lineList = value;
                OnPropertyChanged("LineList");
            }
        }

        public LineControlViewModel SelectedLine
        {
            get
            {
                return _selectedLine;
            }
            set
            {
                _selectedLine = value;
                OnPropertyChanged("SelectedLine");
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        #endregion

        #region Constructor

        #endregion

        #region Method

        #region Public Method
        public static List<LineControlViewModel> GenerateLine(int totalLine)
        {
            var lineViewModelList = new List<LineControlViewModel>();
            for(int index = 0; index<totalLine; index++)
            {
                LineControlViewModel dataViewModel = new LineControlViewModel
                {
                    SelectedLineType = (int)LineType.Static,
                    Name = "Line " + (index + 1).ToString(),
                    DataAddress = 0,
                    ParameterValue = "Temparature",
                    Unit = "Sel"
                };

                lineViewModelList.Add(dataViewModel);
            }

            return lineViewModelList;
        }
        #endregion

        #region Private Method
        private void CreateLineViewModelBasedonTotalLines()
        {
            if (LineList != null)
            {
                if(Lines == 0)
                {
                    LineList = new List<LineControlViewModel>();
                }
                else if(LineList.Count > Lines)
                {
                    var totalLineCount = LineList.Count;
                    for (int index = Lines; index < totalLineCount; index++)
                    {
                        LineList.RemoveAt(LineList.Count - 1);
                    }

                    LineList = new List<LineControlViewModel>(LineList);
                }
                else if(Lines > LineList.Count)
                {
                    for (int index = LineList.Count; index < Lines; index++)
                    {
                        LineControlViewModel dataViewModel = new LineControlViewModel
                        {
                            SelectedLineType = (int)LineType.Static,
                            Name = "Line " + (index + 1).ToString(),
                            DataAddress = 0,
                            ParameterValue = "Temparature",
                            Unit = "Sel"
                        };

                        LineList.Add(dataViewModel);
                    }

                    LineList = new List<LineControlViewModel>(LineList);
                }
            }
        }
        #endregion

        #endregion

        #region Field
        private string _ip;
        private string _name;
        private int _lines;
        private int _dataColumns;
        private List<LineControlViewModel> _lineList;
        private LineControlViewModel _selectedLine;
        #endregion
    }
}
