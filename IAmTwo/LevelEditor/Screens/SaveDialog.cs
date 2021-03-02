using IAmTwo.Game;
using IAmTwo.Menu;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.LevelEditor
{
    public class SaveDialog : LevelEditorDialog
    {
        public static Vector2 Size = new Vector2(350,300);

        public override DrawObject2D Background { get; set; }

        public SaveDialog() : base(Size)
        {
            DrawText name = new DrawText(Fonts.Button, "Level Name:")
            {
                Spacing = .9f,
            };

            TextField input = new TextField(Fonts.Button);
            input.Transform.Position.Set(5, -name.Height - 25);

            DialogObjects.Add(name, input);
        }
    }
}