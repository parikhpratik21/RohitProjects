using Central_LED.Entity;
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
        #region Events
        public delegate void UpdateSelectedLine();
        public UpdateSelectedLine OnUpdateSelectedLine;
        #endregion

        #region Property
        public bool IsModified
        {
            get;
            set;
        }
        public string IP
        {
            get
            {
                return DisplayData.IP;
            }
            set
            {
                DisplayData.IP = value;
                IsModified = true;
                OnPropertyChanged("IP");
            }
        }

        public int Port
        {
            get
            {
                return DisplayData.Port;
            }
            set
            {
                DisplayData.Port = value;
                IsModified = true;
                OnPropertyChanged("Port");
            }
        }

        public int Lines
        {
            get
            {
                return DisplayData.Lines;
            }
            set
            {
                DisplayData.Lines = value;
                IsModified = true;
                OnPropertyChanged("Lines");                
                CreateLineViewModelBasedonTotalLines();
            }
        }

        public int DataColumns
        {
            get
            {
                return DisplayData.DataColumns;
            }
            set
            {
                DisplayData.DataColumns = value;
                IsModified = true;
                UpdateDataColumns();
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
                IsModified = true;
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
                if (OnUpdateSelectedLine != null)
                {
                    OnUpdateSelectedLine.Invoke();
                }
                OnPropertyChanged("SelectedLine");
            }
        }

        public string DisplayName
        {
            get
            {
                return DisplayData.DisplayName;
            }
            set
            {
                DisplayData.DisplayName = value;
                IsModified = true;
                OnPropertyChanged("DisplayName");
            }
        }

        public Display DisplayData
        {
            get
            {
                return _displayData;
            }
            set
            {
                _displayData = value;
                OnPropertyChanged("DisplayData");
            }
        }
        #endregion

        #region Constructor
        public DisplayControlViewModel()
        {
            DisplayData = new Display();
        }
        #endregion

        #region Method

        #region Public Method

        public void SetData(Display data)
        {
            DisplayData = data;
            LineList = new List<LineControlViewModel>();
            foreach(Line lineData in data.LineList)
            {
                LineControlViewModel viewModel = new LineControlViewModel();
                viewModel.LineData = lineData;
                LineList.Add(viewModel);
            }

            LineList = new List<LineControlViewModel>(LineList);
            SelectedLine = LineList.FirstOrDefault();

            OnPropertyChanged("DisplayName");
            OnPropertyChanged("DataColumns");
            OnPropertyChanged("Columns");
            OnPropertyChanged("Lines");
            OnPropertyChanged("IP");

            IsModified = false;
        }
     
        public static List<LineControlViewModel> GenerateLine(int totalLine, int dataColumn)
        {
            var lineViewModelList = new List<LineControlViewModel>();
            for(int index = 0; index<totalLine; index++)
            {
                var dataViewModel = GenerateDefaultStaticLine(index, dataColumn);
                lineViewModelList.Add(dataViewModel);
            }

            return lineViewModelList;
        }
        #endregion

        #region Private Method

        private void UpdateDataColumns()
        {
            if(LineList != null && LineList.Any())
            {
                foreach(var lineData in LineList)
                {
                    if(lineData.SelectedLineType == (int)LineType.Static)
                    {
                        if(lineData.LineData.StaticDataAddressList != null && lineData.LineData.StaticDataAddressList.Any())
                        {
                            int oldDataColumnCount = lineData.LineData.StaticDataAddressList.Count;

                            if(DataColumns > oldDataColumnCount)
                            {
                                for(int index=oldDataColumnCount;index<DataColumns;index++)
                                {
                                    StaticLineDataColumns data = new StaticLineDataColumns();
                                    data.LineID = lineData.LineData.ID;
                                    data.DisplayControlId = DisplayData.ID;
                                    data.Index = index + 1;
                                    data.DataColumnName = "DataAddress " + (index + 1).ToString();
                                    lineData.LineData.StaticDataAddressList.Add(data);
                                }

                                lineData.DataColumnList = new List<StaticLineDataColumns>(lineData.DataColumnList);
                            }
                            else if(DataColumns < oldDataColumnCount)
                            {
                                for(int index=oldDataColumnCount;index>DataColumns;index--)
                                {
                                    if (lineData.LineData.StaticDataAddressList.Count > (index - 1))
                                    {
                                        lineData.LineData.StaticDataAddressList.RemoveAt(index - 1);
                                    }
                                }

                                lineData.DataColumnList = new List<StaticLineDataColumns>(lineData.DataColumnList);
                            }
                        }
                    }
                }
            }
        }

        private void CreateLineViewModelBasedonTotalLines()
        {
            if (LineList != null)
            {
                if(Lines == 0)
                {
                    LineList = new List<LineControlViewModel>();
                    DisplayData.LineList = new List<Line>();
                }
                else if(LineList.Count > Lines)
                {
                    var totalLineCount = LineList.Count;
                    for (int index = Lines; index < totalLineCount; index++)
                    {
                        LineList.RemoveAt(LineList.Count - 1);
                        DisplayData.LineList.RemoveAt(DisplayData.LineList.Count - 1);
                    }

                    LineList = new List<LineControlViewModel>(LineList);
                }
                else if(Lines > LineList.Count)
                {
                    for (int index = LineList.Count; index < Lines; index++)
                    {
                        var dataViewModel = GenerateDefaultStaticLine(index, DisplayData.DataColumns);

                        LineList.Add(dataViewModel);
                        DisplayData.LineList.Add(dataViewModel.LineData);
                    }

                    LineList = new List<LineControlViewModel>(LineList);
                }
            }
        }

        private static LineControlViewModel GenerateDefaultStaticLine(int index, int dataColumn)
        {
            LineControlViewModel dataViewModel = new LineControlViewModel
            {
                SelectedLineType = (int)LineType.Static,
                LineName = "Line " + (index + 1).ToString(),
                DataAddress = 0,
                ParameterName = "Parameter",
                ParameterValue = 0,
                Unit = "Unit",
                IsModified = true
            };
            if (dataColumn > 0)
            {
                dataViewModel.LineData.StaticDataAddressList = new List<StaticLineDataColumns>();
                for (int innerIndex = 0; innerIndex < dataColumn; innerIndex++)
                {
                    StaticLineDataColumns data = new StaticLineDataColumns();
                    data.Index = innerIndex + 1;
                    data.LineID = 0;
                    data.DataAddress = 0;
                    data.DataColumnName = "DataAddress " + (innerIndex + 1).ToString();
                    dataViewModel.LineData.StaticDataAddressList.Add(data);
                }
            }

            return dataViewModel;
        }
        #endregion

        #endregion

        #region Field
        private Display _displayData;
        private List<LineControlViewModel> _lineList;
        private LineControlViewModel _selectedLine;
        #endregion
    }
}
