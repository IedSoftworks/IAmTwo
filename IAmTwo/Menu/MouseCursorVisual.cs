using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SM.Base;
using SM.Base.Controls;
using SM.Base.Textures;
using SM.Base.Window;
using SM2D.Controls;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.Menu
{
    public class MouseCursorVisual : ItemCollection
    {
        public static MouseCursorVisual visual = new MouseCursorVisual();

        private DrawObject2D _cursor;

        private MouseCursorVisual()
        {
            Texture t = Resource.RequestTexture(@".\Resources\cursor.png");
            _cursor = new DrawObject2D()
            {
                Texture = t,
            };
            _cursor.Material.Blending = true;
            _cursor.Transform.ApplyTextureSize(t);

            Add(_cursor);
        }

        public override void Draw(DrawContext context)
        {
            Vector2 windowSize = SMRenderer.CurrentWindow.WindowSize;
            Vector2 mousePos = Mouse2D.InWorld(worldScale: windowSize) + new Vector2(_cursor.Transform.Size.X, -_cursor.Transform.Size.Y) / 2;

            Transform.Position.Set(mousePos);

            GL.Disable(EnableCap.DepthTest);
            base.Draw(context);
            GL.Enable(EnableCap.DepthTest);
        }
    }
}