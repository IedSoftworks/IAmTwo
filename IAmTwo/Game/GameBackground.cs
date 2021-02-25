using IAmTwo.Resources;
using IAmTwo.Shaders;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SM.Base.Windows;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.Game
{
    public class GameBackground : DrawBackground
    {
        public GameBackground(Camera cam)
        {
            Texture = Resource.RequestTexture(@".\Resources\background.png", TextureMinFilter.Nearest, TextureWrapMode.Repeat);

            float aspect = cam.WorldScale.Y / cam.WorldScale.X;
            float size = 50;
            TextureTransform.Scale.Set(size, aspect * size);

            float brightness = .5f;
            Color = new Color4(brightness, brightness, brightness, 1);
        }

        protected override void DrawContext(ref DrawContext context)
        {
            base.DrawContext(ref context);
        }
    }
}