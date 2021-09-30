using IAmTwo.Game;
using IAmTwo.LevelObjects;
using IAmTwo.Resources;
using OpenTK;
using SM.Base.Animation;
using SM.Base.Textures;
using SM.Base.Window;
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
        InterpolationProcess proces;

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
            proces = item.Transform.Size.Interpolate(TimeSpan.FromSeconds(5), Vector2.Zero, ((Vector2)item.Transform.Size) * .25f, AnimationCurves.Smooth);
            proces.End += (a, b) => Switch();
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);
            if ((bool)Controller.Actor.Get("c_skipCredits")) Switch();
        }

        private void Switch()
        {
            if (proces.Running) proces.Stop();
            ChangeScene(MainMenu.Menu);
        }
    }
}
