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
    /// Interaction logic for ScrollingLineDisplay.xaml
    /// </summary>
    public partial class ScrollingLineDisplay : UserControl
    {
        public ScrollingLineDisplay()
        {
            InitializeComponent();
        }

        public void SetData(string scrollingMessage, bool isLastRow, bool isAutoMode)
        {
            scrollingText.Text = scrollingMessage;
            if(isLastRow)
            {
                MsgDisplay.BorderThickness = new Thickness(1, 1, 1, 1);
            }
            else
            {
                MsgDisplay.BorderThickness = new Thickness(1, 1, 1, 0);
            }

            _isAutoMode = isAutoMode;
        }

        public void StartAnimation()
        {
            if (_isAnimationRunning == false && _isAutoMode == false)
            {
                LeftToRightMarqueeOnTextBox();
                _isAnimationRunning = true;
            }
        }

        public void StopAnimation()
        {
            if (_isAnimationRunning && _isAutoMode == false)
            {
                scrollingText.BeginAnimation(TextBox.PaddingProperty, null);
                _isAnimationRunning = false;
            }
        }
       
        private void LeftToRightMarqueeOnTextBox()
        {
            string Copy = " " + scrollingText.Text;
            double TextGraphicalWidth = new FormattedText(scrollingText.Text, System.Globalization.CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface(scrollingText.FontFamily.Source), scrollingText.FontSize, scrollingText.Foreground).WidthIncludingTrailingWhitespace;
            //BorderTextBoxMarquee.Width = TextGraphicalWidth + 5;

            ThicknessAnimation ThickAnimation = new ThicknessAnimation();
            ThickAnimation.From = new Thickness(scrollingText.ActualWidth, 0, 0, 0);
            ThickAnimation.To = new Thickness(0, 0, 0, 0);
            ThickAnimation.RepeatBehavior = RepeatBehavior.Forever;
            ThickAnimation.Duration = new Duration(TimeSpan.FromSeconds(8));
            scrollingText.BeginAnimation(TextBox.PaddingProperty, ThickAnimation);
        }

        #region Field
        private bool _isAnimationRunning = false;
        private bool _isAutoMode = false;
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(_isAutoMode)
            {
                LeftToRightMarqueeOnTextBox();
                _isAnimationRunning = true;
            }
        }
    }
}
