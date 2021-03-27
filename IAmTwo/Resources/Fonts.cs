using SM.Base.Drawing.Text;

namespace IAmTwo.Resources
{
    public class Fonts
    {
        public static Font Button = new Font(@".\Resources\GapSansBold.ttf")
        {
            FontSize = 13,
            Spacing = .8f
        };

        public static Font Text  = new Font(@".\Resources\Urbanist-Regular.ttf")
        {
            FontSize = 13,
            Spacing = .8f
        };

        public static Font FontAwesome = new Font(@".\Resources\FontAwesome5.otf")
        {
            CharSet = new char[]
            {
                '\uf00c',
                '\uf061',
                '\uf09b', // Github
                '\uf392', // Discord 
            },
            FontSize = 13
        };
    }
}