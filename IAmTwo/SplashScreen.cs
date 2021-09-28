using IAmTwo.Game;
using IAmTwo.LevelObjects;
using IAmTwo.Resources;
using OpenTK;
using SM.Base.Animation;
using SM.Base.Textures;
using SM2D.Drawing;
using SM2D.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmTwo
{
    class SplashScreen : BaseScene
    {
        public override void Initialization()
        {
            base.Initialization();

            Camera = new Camera()
            {
                RequestedWorldScale = new Vector2(700, 700 * LevelScene.Aspect)
            };
            BackgroundCamera = Camera;
            Background = new GameBackground(Camera);

            DrawObject2D item = new DrawObject2D()
            {
                Material =
                {
                    Texture = Resource.RequestTexture(@".\Resources\large_logo.png"),
                    Blending = true
                }
            };
            item.Transform.ApplyTextureSize((Texture)item.Material.Texture);
            Objects.Add(item);

            Vector2 start = ((Vector2)item.Transform.Size) * .5f;
            InterpolationProcess interpolation = item.Transform.Size.Interpolate(TimeSpan.FromSeconds(5), Vector2.Zero, ((Vector2)item.Transform.Size) * .25f, AnimationCurves.Smooth);
            interpolation.End += (a,b) => ChangeScene(MainMenu.Menu);
        }
    }
}
