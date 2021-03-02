using IAmTwo.Game;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.LevelEditor
{
    public class LevelEditorDialog : LevelEditorMenu
    {
        public ItemCollection DialogObjects { get; private set; }

        public override DrawObject2D Background { get; set; }
        public override bool Input()
        {
            return false;
        }

        public LevelEditorDialog(Vector2 size)
        {
            Background = new DrawObject2D {Color = ColorPallete.Background};
            Background.Transform.Size.Set(size);

            DrawObject2D border = new DrawObject2D();
            border.Mesh = Models.QuadricBorder;
            border.Color = Color4.Blue;
            border.Transform.Size.Set(size);

            DialogObjects = new ItemCollection();
            DialogObjects.Transform.Position.Set(-size.X / 2 * .95f, size.Y / 2 *.9f);

            Add(Background, border, DialogObjects);
        }

        public void Show()
        {
            LevelEditor.CurrentEditor.CloseAllMenus(this);
            RenderActive = true;
        }

        public void Hide()
        {
            RenderActive = false;
        }
    }
}