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

        public static InstancedMesh QuadricBorder;


        static Models()
        {
            QuadricBorder = new InstancedMesh(PrimitiveType.Lines, new string[0]);
            QuadricBorder.Vertex.Add(new[]
            {
                new Vector3(-.4f, .5f, 0f),
                new Vector3(.4f, .5f, 0f),
                new Vector3(.5f, .4f, 0f),
                new Vector3(.5f, -.4f, 0f),
                new Vector3(.4f, -.5f, 0f),
                new Vector3(-.4f, -.5f, 0f),
                new Vector3(-.5f, -.4f, 0f),
                new Vector3(-.5f, .4f, 0f),
            });
            QuadricBorder.LineWidth = 2;
        }
    }
}