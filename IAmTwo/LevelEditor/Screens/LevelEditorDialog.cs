using IAmTwo.Game;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SM2D.Drawing;
using SM2D.Object;

namespace IAmTwo.LevelEditor
{
    public class LevelEditorDialog : LevelEditorMenu
    {
        public override DrawObject2D Background { get; set; }

        public LevelEditorDialog(Vector2 size)
        {
            Polygon mesh = Models.CreateBackgroundPolygon(size, 10);

            Background = new DrawObject2D { Color = ColorPallete.Background, Mesh = mesh };
            Background.Transform.Size.Set(1);

            DrawObject2D border = new DrawObject2D
            {
                Color = Color4.Red,
                Mesh = mesh,
                ForcedMeshType = PrimitiveType.LineLoop
            };
            border.ShaderArguments["ColorScale"] = 1.2f;
            border.Transform.Size.Set(1);

            Add(Background, border);
        }

        public override bool Input()
        {
            return false;
        }

        public virtual void Show() {}
        public virtual void Hide() {}
    }
}