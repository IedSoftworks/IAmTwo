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

        public static Polygon CreateBackgroundPolygon(Vector2 size, float cornerSize)
        {
            Vector2 halfSize = size / 2;

            return new Polygon(new Vector2[]
            {
                new Vector2(-halfSize.X, halfSize.Y - cornerSize),
                new Vector2(-halfSize.X + cornerSize, halfSize.Y),
                new Vector2(halfSize.X, halfSize.Y),
                new Vector2(halfSize.X, -halfSize.Y + cornerSize),
                new Vector2(halfSize.X - cornerSize, -halfSize.Y),
                new Vector2(-halfSize.X, -halfSize.Y),
            })
            {
                LineWidth = 2f
            };
        }
    }
}