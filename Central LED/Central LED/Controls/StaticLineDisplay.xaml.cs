using Central_LED.Entity;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Central_LED.Controls
{
    /// <summary>
    /// Interaction logic for StaticLineDisplay.xaml
    /// </summary>
    public partial class StaticLineDisplay : UserControl
    {
        public StaticLineDisplay()
        {
            InitializeComponent();
        }

        public void SetDataColumn(int dataColumn, bool isLastRow)
        {
            _textBoxData = new Dictionary<int, Border>();
            MainGrid.ColumnDefinitions.Clear();
            for(int index = 0; index < dataColumn; index++)
            {
                var dataRow = new ColumnDefinition();
                dataRow.Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions.Add(dataRow);

                Thickness colThickness;
                if (isLastRow)
                {
                    colThickness = new Thickness(1, 1, 0, 1);
                }
                else
                {
                    colThickness = new Thickness(1, 1, 0, 0);
                }

                var borderControl = GenerateTextBlockData(string.Empty, false, false, colThickness);
                Grid.SetColumn(borderControl, index);
                Grid.SetRow(borderControl, 0);
                MainGrid.Children.Add(borderControl);

                if (_textBoxData.Keys.Contains(index + 1) == false)
                {
                    _textBoxData.Add(index + 1, borderControl);
                }
            }
        }

        public void UpdateData(List<StaticLineDataColumns> dataColumnList)
        {
            if(dataColumnList != null && dataColumnList.Any())
            {
                foreach(var staticData in dataColumnList)
                {
                    if (_textBoxData.Keys.Contains(staticData.Index))
                    {
                        var borderControl = _textBoxData[staticData.Index];
                        var textBoxControl = borderControl.Child as TextBlock;
                        if(textBoxControl != null)
                        {
                            textBoxControl.Text = staticData.DataValue.ToString();
                        }
                    }
                }
            }
        }

        private Border GenerateTextBlockData(string content, bool isBold, bool isCenterAlign, Thickness borderThickness)
        {
            Border parentControl = new Border();
            TextBlock data = new TextBlock();
            data.Text = content;
            if (isBold)
            {
                data.FontWeight = FontWeights.Bold;
            }
            else
            {
                data.FontWeight = FontWeights.Normal;
            }
            data.Margin = new Thickness(5);
            if (isCenterAlign)
            {
                data.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            }
            else
            {
                data.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            }

            data.Height = 20;
            data.HorizontalAlignment = HorizontalAlignment.Stretch;
            parentControl.Child = data;
            parentControl.BorderBrush = Brushes.Black;
            parentControl.BorderThickness = borderThickness;
            return parentControl;
        }
        #region Field
        private Dictionary<int, Border> _textBoxData;
        #endregion
    }
}
