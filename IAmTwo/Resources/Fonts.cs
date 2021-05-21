using SM.Base.Drawing.Text;

namespace IAmTwo.Resources
{
    public class Fonts
    {
        public static Font Button = new Font(@".\Resources\GapSansBold.ttf")
        {
            FontSize = 13
        };
        public static Font HeaderFont = new Font(@".\Resources\GapSansBold.ttf")
        {
            FontSize = 52,
        };

        public static Font Text  = new Font(@".\Resources\Urbanist-Regular.ttf")
        {
            FontSize = 13
        };

        public static Font FontAwesome = new Font(@".\Resources\FontAwesome5.otf")
        {
            CharSet = new char[]
            {
                '\uf00c',   // Check
                '\uf061',   // Arrow-Right
                '\uf09b',   // Github
                '\uf392',   // Discord 
                '\uf101',   // angle-double-right
                '\uf11b',   // Gamepad
                '\uf11c'    // Keyboard
            },
            FontSize = 13
        };
        
        public static Font PS = new Font(@".\Resources\PS4.otf")
        {
            FontSize = 20,
            BaselineAdjust = 1.3f,
            CharSet = new char[]
            {
                'x', 't', 'c'
            }
        };
        public static Font XBOX = new Font(@".\Resources\XBOX.otf")
        {
            FontSize = 20,
            BaselineAdjust = 1.5f,
            CharSet = new char[]
            {
                'a', 'y', 'b'
            }
        };
    }
}