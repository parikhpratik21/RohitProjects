using Central_LED.Entity;
using Central_LED.Helper;
using Central_LED.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Central_LED.Controls
{
    /// <summary>
    /// Interaction logic for LiveDisplayController.xaml
    /// </summary>
    public partial class LiveDisplayController : UserControl
    {
        #region Constructor
        public LiveDisplayController()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods

        #region Public Method
        public void DisplayLiveData(Display data)
        {
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();

            _totalColumn = 3 + data.DataColumns;

            GenerateFirstRow(data.DataColumns);

            _staticControlList = new Dictionary<int, StaticLineDisplay>();
            _scrollingControlList = new Dictionary<int, ScrollingLineDisplay>();
            _blinkingControlList = new Dictionary<int, BlinkingLineDisplay>();

            if(data != null && data.LineList != null)
            {
                var noColumn = new ColumnDefinition();
                noColumn.Width = new GridLength(0.5, GridUnitType.Star);
                MainGrid.ColumnDefinitions.Add(noColumn);

                var parameterColumn = new ColumnDefinition();
                parameterColumn.Width = new GridLength(2, GridUnitType.Star);
                MainGrid.ColumnDefinitions.Add(parameterColumn);

                var unitColumn = new ColumnDefinition();
                unitColumn.Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions.Add(unitColumn);

                for(int columnIndex = 0; columnIndex < data.DataColumns; columnIndex++)
                {
                    var dataColumn = new ColumnDefinition();
                    dataColumn.Width = new GridLength(1, GridUnitType.Star);
                    MainGrid.ColumnDefinitions.Add(dataColumn);
                }               

                int count = 1;
                foreach(var lineData in data.LineList)
                {
                    if (lineData != null && lineData.Type != null)
                    {
                        if (lineData.Type == (int)Helper.LineType.Static)
                        {
                            GenerateStaticRow(lineData, count, count == data.LineList.Count, data.DataColumns);
                        }
                        else if (lineData.Type == (int)Helper.LineType.Blinking)
                        {
                            GenerateBlinkingRow(lineData, count, count == data.LineList.Count);
                        }
                        else if (lineData.Type == (int)Helper.LineType.Scrolling)
                        {
                            GenerateScrollingRow(lineData, count, count == data.LineList.Count);
                        }
                        else
                        {
                            GenerateBlanckRow(count, count == data.LineList.Count);
                        }
                        count++;
                    }
                }
            }
        }

        public void UpdateLiveDisplay(DisplayControlViewModel data)
        {
            if (data != null && data.LineList != null)
            {
                int count = 1;
                foreach (var lineData in data.LineList)
                {
                    if (lineData != null && lineData.SelectedLineType != null)
                    {
                        if (lineData.SelectedLineType == (int)Helper.LineType.Static)
                        {
                            if (_staticControlList.ContainsKey(lineData.LineData.ID))
                            {
                                var staticLIneControl = _staticControlList[lineData.LineData.ID];
                                staticLIneControl.UpdateData(lineData.LineData.StaticDataAddressList);
                            }
                        }
                        else if (lineData.SelectedLineType == (int)Helper.LineType.Scrolling)
                        {
                            if (_scrollingControlList.ContainsKey(lineData.LineData.ID))
                            {
                                var scrollingControl = _scrollingControlList[lineData.LineData.ID];
                                if(lineData.ScrollingValue > 0.8)
                                {
                                    scrollingControl.StartAnimation();
                                }
                                else
                                {
                                    scrollingControl.StopAnimation();
                                }                               
                            }
                        }
                        else if (lineData.SelectedLineType == (int)Helper.LineType.Blinking)
                        {
                            if (_blinkingControlList.ContainsKey(lineData.LineData.ID))
                            {
                                var blinkingControl = _blinkingControlList[lineData.LineData.ID];
                                if (lineData.BlinkingValue1 > 0.8)
                                {
                                    blinkingControl.StartAnimationForBlink1();
                                }
                                else
                                {
                                    blinkingControl.StopAnimationForBlink1();
                                }

                                if (lineData.BlinkingValue2 > 0.8)
                                {
                                    blinkingControl.StartAnimationForBlink2();
                                }
                                else
                                {
                                    blinkingControl.StopAnimationForBlink2();
                                }
                            }
                        }
                        count++;
                    }
                }
            }
        }

        #endregion

        #region Private Method
        private void GenerateStaticRow(Entity.Line lineData, int count, bool isLastRow, int dataColumn)
        {
            var dataRow = new RowDefinition();
            dataRow.Height = new GridLength(1,GridUnitType.Auto);
            MainGrid.RowDefinitions.Add(dataRow);

            Thickness borderThicknessForFirstCol;
            if(isLastRow)
            {
                borderThicknessForFirstCol = new Thickness(1, 1, 0, 1);
            }
            else
            {
                borderThicknessForFirstCol = new Thickness(1, 1, 0, 0);
            }

            Thickness borderThicknessForLastCol;
            if (isLastRow)
            {
                borderThicknessForLastCol = new Thickness(1, 1, 1, 1);
            }
            else
            {
                borderThicknessForLastCol = new Thickness(1, 1, 1, 0);
            }

            var noTitle = GenerateTextBlockData(count.ToString(), false, false, borderThicknessForFirstCol);
            Grid.SetColumn(noTitle, 0);
            Grid.SetRow(noTitle, count);
            MainGrid.Children.Add(noTitle);

            var parameterTitle = GenerateTextBlockData(lineData.ParameterName, false, false, borderThicknessForFirstCol);
            Grid.SetColumn(parameterTitle, 1);
            Grid.SetRow(parameterTitle, count);
            MainGrid.Children.Add(parameterTitle);

            var staticData = new StaticLineDisplay();
            Grid.SetColumn(staticData, 2);
            Grid.SetColumnSpan(staticData, dataColumn);
            Grid.SetRow(staticData, count);
            staticData.SetDataColumn(dataColumn, isLastRow);
            MainGrid.Children.Add(staticData);

            if (_staticControlList.ContainsKey(lineData.ID) == false)
            {
                _staticControlList.Add(lineData.ID, staticData);
            }

            var unitTitle = GenerateTextBlockData(lineData.Unit, false, true, borderThicknessForLastCol);
            Grid.SetColumn(unitTitle, _totalColumn - 1);
            Grid.SetRow(unitTitle, count);
            MainGrid.Children.Add(unitTitle);
        }

        private void GenerateBlanckRow(int count, bool isLastRow)
        {
            var dataRow = new RowDefinition();
            dataRow.Height = new GridLength(1, GridUnitType.Auto);
            MainGrid.RowDefinitions.Add(dataRow);

            Thickness borderThicknessForFirstCol;
            if (isLastRow)
            {
                borderThicknessForFirstCol = new Thickness(1, 1, 0, 1);
            }
            else
            {
                borderThicknessForFirstCol = new Thickness(1, 1, 0, 0);
            }

            Thickness borderThicknessForLastCol;
            if (isLastRow)
            {
                borderThicknessForLastCol = new Thickness(1, 1, 1, 1);
            }
            else
            {
                borderThicknessForLastCol = new Thickness(1, 1, 1, 0);
            }

            var noTitle = GenerateTextBlockData(count.ToString(), false, false, borderThicknessForFirstCol);
            Grid.SetColumn(noTitle, 0);
            Grid.SetRow(noTitle, count);
            MainGrid.Children.Add(noTitle);

            var parameterTitle = GenerateEmptyControlRow(borderThicknessForLastCol);
            Grid.SetColumn(parameterTitle, 1);
            Grid.SetRow(parameterTitle, count);
            Grid.SetColumnSpan(parameterTitle, 2 + _totalColumn);
            MainGrid.Children.Add(parameterTitle);
        }

        private void GenerateFirstRow(int dataColumns)
        {
            var firstRow = new RowDefinition();
            firstRow.Height = new GridLength(1,GridUnitType.Auto);
            MainGrid.RowDefinitions.Add(firstRow);

            var noTitle = GenerateTextBlockData("No", true, false, new Thickness(1,1,0,0));
            Grid.SetColumn(noTitle, 0);
            Grid.SetRow(noTitle, 0);
            MainGrid.Children.Add(noTitle);

            var parameterTitle = GenerateTextBlockData("Parameters", true, false, new Thickness(1, 1, 0, 0));
            Grid.SetColumn(parameterTitle, 1);
            Grid.SetRow(parameterTitle, 0);
            MainGrid.Children.Add(parameterTitle);

            var valueTitle = GenerateTextBlockData("Value", true, true, new Thickness(1, 1, 0, 0));
            Grid.SetColumn(valueTitle, 2);
            Grid.SetColumnSpan(valueTitle, dataColumns);
            Grid.SetRow(valueTitle, 0);            
            MainGrid.Children.Add(valueTitle);

            var unitTitle = GenerateTextBlockData("Unit", true, true, new Thickness(1, 1, 1, 0));
            Grid.SetColumn(unitTitle, _totalColumn);
            Grid.SetRow(unitTitle, 0);
            MainGrid.Children.Add(unitTitle);
        }

        private Border GenerateTextBlockData(string content, bool isBold, bool isCenterAlign, Thickness borderThickness)
        {
            Border parentControl = new Border();            
            TextBlock data = new TextBlock();
            data.Text = content;
            if(isBold)
            {
                data.FontWeight = FontWeights.Bold;
            }
            else
            {
                data.FontWeight = FontWeights.Normal;
            }
            data.Margin = new Thickness(5);
            if(isCenterAlign)
            {
                data.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            }
            else
            {
                data.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            }
           
            data.Height = 20;
            //data.HorizontalAlignment = HorizontalAlignment.Stretch;
            parentControl.Child = data;
            parentControl.BorderBrush = Brushes.Black;
            parentControl.BorderThickness = borderThickness;
            return parentControl;
        }

        private Border GenerateEmptyControlRow(Thickness borderThickness)
        {
            Border parentControl = new Border();            
            parentControl.BorderBrush = Brushes.Black;
            parentControl.BorderThickness = borderThickness;
            return parentControl;
        }

        private void GenerateScrollingRow(Entity.Line lineData, int count, bool isLastRow)
        {
            var dataRow = new RowDefinition();
            dataRow.Height = new GridLength(1,GridUnitType.Auto);
            MainGrid.RowDefinitions.Add(dataRow);

            Thickness colThickness;
            if(isLastRow)
            {
                colThickness = new Thickness(1, 1, 0, 1);
            }
            else
            {
                colThickness = new Thickness(1, 1, 0, 0);
            }
            var noTitle = GenerateTextBlockData(count.ToString(), false, false, colThickness);
            Grid.SetColumn(noTitle, 0);
            Grid.SetRow(noTitle, count);
            MainGrid.Children.Add(noTitle);

            var scrollingData = new ScrollingLineDisplay();
            bool isAutoMode = false;
            if (lineData.ScrollingType == (int)LineActionType.Auto)
            {
                isAutoMode = true;
            }
            scrollingData.SetData(lineData.ScrollingMessage, isLastRow, isAutoMode);           
            Grid.SetColumn(scrollingData, 1);
            Grid.SetRow(scrollingData, count);
            Grid.SetColumnSpan(scrollingData, 2 + _totalColumn);
            MainGrid.Children.Add(scrollingData);
            if (_scrollingControlList.ContainsKey(lineData.ID) == false)
            {
                _scrollingControlList.Add(lineData.ID, scrollingData);
            }
        }

        private void GenerateBlinkingRow(Entity.Line lineData, int count, bool isLastRow)
        {
            var dataRow = new RowDefinition();
            dataRow.Height = new GridLength(1,GridUnitType.Auto);
            MainGrid.RowDefinitions.Add(dataRow);

            Thickness colThickness;
            if (isLastRow)
            {
                colThickness = new Thickness(1, 1, 0, 1);
            }
            else
            {
                colThickness = new Thickness(1, 1, 0, 0);
            }

            var noTitle = GenerateTextBlockData(count.ToString(), false, false, colThickness);
            Grid.SetColumn(noTitle, 0);
            Grid.SetRow(noTitle, count);
            MainGrid.Children.Add(noTitle);

            bool isAutoMode1 = false;
            if (lineData.ScrollingType == (int)LineActionType.Auto)
            {
                isAutoMode1 = true;
            }

            bool isAutoMode2 = false;
            if (lineData.ScrollingType2 == (int)LineActionType.Auto)
            {
                isAutoMode2 = true;
            }

            var blinkingData = new BlinkingLineDisplay();
            blinkingData.SetData1(lineData.BlinkingMessage1, isLastRow, isAutoMode1);
            blinkingData.SetData2(lineData.BlinkingMessage2, isLastRow, isAutoMode2);
            Grid.SetColumn(blinkingData, 1);
            Grid.SetRow(blinkingData, count);
            Grid.SetColumnSpan(blinkingData, 2 + _totalColumn);            
            MainGrid.Children.Add(blinkingData);            

            if (_blinkingControlList.ContainsKey(lineData.ID) == false)
            {
                _blinkingControlList.Add(lineData.ID, blinkingData);
            }
        }
        #endregion

        #endregion

        #region Fields
        private Dictionary<int,ScrollingLineDisplay> _scrollingControlList;
        private Dictionary<int,BlinkingLineDisplay> _blinkingControlList;
        private Dictionary<int, StaticLineDisplay> _staticControlList;
        private int _totalColumn;
        #endregion
    }
}
