using System;
using IAmTwo.LevelObjects;
using IAmTwo.Resources;
using IAmTwo.Shaders;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SM.Base.Drawing;
using SM.Base.Utility;
using SM.Base.Window;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.Game
{
    public class GameBackground : DrawBackground
    {
        private const float Brightness = .1f;
        public static Color4 Color = new Color4(Brightness, Brightness, Brightness, 1f);

        public GameBackground(Camera cam)
        {
            Material.CustomShader = ShaderCollection.Shaders["Background"].GetShader();



            cam.WorldScaleChanged += CalculateGrid;
            CalculateGrid(cam);
        }

        private void CalculateGrid(Camera cam)
        {
            float aspect = cam.WorldScale.Y / cam.WorldScale.X;
            const float size = 50;
            TextureTransform.Scale.Set(size, aspect * size);
        }

        protected override void DrawContext(ref DrawContext context)
        {
            base.DrawContext(ref context);
        }
    }
}