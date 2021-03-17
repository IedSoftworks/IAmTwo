using IAmTwo.Game;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using SM.Base.Utility;
using SM2D.Drawing;
using SM2D.Object;
using Keyboard = SM.Base.Controls.Keyboard;

namespace IAmTwo.LevelEditor
{
    public class HelpScreen : LevelEditorMenu
    {
        public HelpScreen()
        {
            Transform.Size.Set(1);

            DrawText text = new DrawText(Fonts.Text, AssemblyUtility.ReadAssemblyFile("IAmTwo.LevelEditor.Screens.HelpText.txt"))
            {
                Spacing = .75f
            };
            text.GenerateMatrixes();
            text.Transform.Position.Set(-text.Width / 2 + 10, text.Height/ 2 - 10);
            text.Transform.Size.Set(.7f);

            Polygon p = Models.CreateBackgroundPolygon(new Vector2(text.Width, text.Height), 10);

            Background = new DrawObject2D {Color = ColorPallete.Background, Mesh = p };
            Background.Transform.Size.Set(1);

            DrawObject2D border = new DrawObject2D()
            {
                Color = Color4.Green,
                Mesh = p,
                ForcedMeshType = PrimitiveType.LineLoop
            };
            border.ShaderArguments["ColorScale"] = 2.1f;
            border.Transform.Size = Background.Transform.Size;

            Add(Background, text, border);
        }

        public override DrawObject2D Background { get; set; }

        public override bool Input()
        {
            return Keyboard.IsDown(Key.F1, true);
        }
    }
}