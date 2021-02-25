using IAmTwo.Game;
using IAmTwo.Menu;
using IAmTwo.Resources;
using OpenTK.Graphics;
using OpenTK.Input;
using SM.Utility;
using SM2D.Drawing;
using SM2D.Scene;
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
                Spacing = .5f
            };
            text.GenerateMatrixes();
            text.Transform.Position.Set(-text.Width / 2 + 10, -text.Height/ 2 - 10);
            text.Transform.Size.Set(.7f);

            Background = new DrawObject2D {Color = ColorPallete.Background};
            Background.Transform.Size.Set(text.Width, text.Height);

            DrawObject2D border = new DrawObject2D()
            {
                Color = Color4.Green
            };
            border.Mesh = ObjectButton.Border;
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