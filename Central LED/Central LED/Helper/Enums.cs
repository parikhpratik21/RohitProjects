using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_LED.Helper
{
    public class Enums
    {
        public enum LineType
        {
            Static = 0,
            Scrolling = 1,
            Blinking = 2,
            Blank = 3
        }

        public enum LineActionType
        {
            Manual = 0,
            Auto = 1
        }

        public enum CharacterType
        {
            Small = 0,
            Big = 1
        }
    }
}
