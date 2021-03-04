using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SM.Base.Objects;
using SM2D.Object;

namespace IAmTwo.Resources
{
    public class Models
    {
        public static PolyLine Border = new PolyLine(new Vector2[] { Vector2.Zero, Vector2.UnitY })
        {
            LineWidth = 5f
        };

        public static PolyLine QuadricBorder;
        public static PolyLine QuadricBorderNotConnected;


        static Models()
        {
            QuadricBorder = new PolyLine(
                new[]
                {
                    new Vector2(-.4f, .5f), new Vector2(.5f, .5f),
                    new Vector2(.5f, -.4f), new Vector2(.4f, -.5f),
                    new Vector2(-.5f, -.5f), new Vector2(-.5f, .4f),
                }, PolyLineType.ConnectedLoop)
            {
                LineWidth = 2
            };

            QuadricBorderNotConnected = new PolyLine(new []
            {
                new Vector2(-.4f, .5f), new Vector2(.4f, .5f),
                new Vector2(.5f, .4f), new Vector2(.5f, -.4f),
                new Vector2(.4f, -.5f), new Vector2(-.4f, -.5f),
                new Vector2(-.5f, -.4f), new Vector2(-.5f, .4f),
            })
            {
                LineWidth = 2
            };
        }
    }
}