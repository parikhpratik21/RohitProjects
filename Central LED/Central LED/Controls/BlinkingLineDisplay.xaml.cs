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
    /// Interaction logic for BlinkingLineDisplay.xaml
    /// </summary>
    public partial class BlinkingLineDisplay : UserControl
    {
        public BlinkingLineDisplay()
        {
            InitializeComponent();
        }

        public void SetData1(string blinkMsg1, bool isLastRow, bool isAutoMode)
        {
            blnkMsg1.Text = blinkMsg1;
         
            if (isLastRow)
            {
                FirstMsgBorder.BorderThickness = new Thickness(1, 1, 1, 1);               
            }
            else
            {
                FirstMsgBorder.BorderThickness = new Thickness(1, 1, 1, 0);               
            }

            _isAutoMode1 = isAutoMode;
        }

        public void SetData2(string blinkMsg2, bool isLastRow, bool isAutoMode)
        {           
            blnkMsg2.Text = blinkMsg2;

            if (isLastRow)
            {
                SecondMsgBorder.BorderThickness = new Thickness(0, 1, 1, 1);
            }
            else
            {
                SecondMsgBorder.BorderThickness = new Thickness(0, 1, 1, 0);
            }

            _isAutoMode2 = isAutoMode;
        }

        public void StartAnimationForBlink1()
        {
            if (_isAnimationRunningForMsg1 == false && _isAutoMode1 == false)
            {
                RightToLeftMarqueeForMsg1();
                _isAnimationRunningForMsg1 = true;
            }
        }

        public void StartAnimationForBlink2()
        {
            if (_isAnimationRunningForMsg2 == false && _isAutoMode2 == false)
            {
                RightToLeftMarqueeForMsg2();
                _isAnimationRunningForMsg2 = true;
            }
        }

        public void StopAnimationForBlink1()
        {
            if (_isAnimationRunningForMsg1 && _isAutoMode1 == false)
            {
                blnkMsg1.BeginAnimation(TextBlock.OpacityProperty, null);
                _isAnimationRunningForMsg1 = false;
            }           
        }

        public void StopAnimationForBlink2()
        {
            if (_isAnimationRunningForMsg2 && _isAutoMode2 == false)
            {               
                blnkMsg2.BeginAnimation(TextBlock.OpacityProperty, null);
                _isAnimationRunningForMsg2 = false;
            }
        }
       
        private void RightToLeftMarqueeForMsg1()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;                      
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.RepeatBehavior = RepeatBehavior.Forever;
            blnkMsg1.BeginAnimation(TextBlock.OpacityProperty, da);            
        }

        private void RightToLeftMarqueeForMsg2()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            da.RepeatBehavior = RepeatBehavior.Forever;           
            blnkMsg2.BeginAnimation(TextBlock.OpacityProperty, da);
        }

        #region Field
        private bool _isAnimationRunningForMsg1 = false;
        private bool _isAnimationRunningForMsg2 = false;
        private bool _isAutoMode1 = false;
        private bool _isAutoMode2 = false;
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(_isAutoMode1)
            {
                RightToLeftMarqueeForMsg1();               
                _isAnimationRunningForMsg1 = true;                
            }

            if (_isAutoMode2)
            {             
                RightToLeftMarqueeForMsg2();                
                _isAnimationRunningForMsg2 = true;
            }
        }
    }
}
