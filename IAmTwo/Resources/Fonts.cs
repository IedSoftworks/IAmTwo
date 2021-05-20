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
                '\uf00c',
                '\uf061',
                '\uf09b', // Github
                '\uf392', // Discord 
                '\uf101' 
            },
            FontSize = 13
        };
        
        public static Font PS = new Font(@".\Resources\PS4.otf")
        {
            FontSize = 20,
            CharSet = new char[]
            {
                'x'
            }
        };
        public static Font XBOX = new Font(@".\Resources\XBOX.otf")
        {
            FontSize = 20,
            CharSet = new char[]
            {
                'a'
            }
        };
    }
}