using System;
using System.Windows.Media;

namespace ViewNote
{
    public static class VNcolors
    {        
        private static Color _colorDark;
        public static Color ColorDark
        {
            get
            {
                if ( _colorDark == null )
                    _colorDark = new Color() { A = 0, R = 0, G = 0, B = 0 };
                return _colorDark;
            }
        }

        private static SolidColorBrush _colorAppBg;
        public static SolidColorBrush ColorAppBg
        {
            get
            {
                if ( _colorAppBg == null )
                    _colorAppBg = new SolidColorBrush(Colors.Gray);
                return _colorAppBg;
            }
        }
    }
}
