using System;
using System.ComponentModel;
using System.IO;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Input;
using SM.Base;
using SM.Base.Drawing.Text;
using SM.Base.Utility;
using SM.Base.Window;
using SM2D.Drawing;
using SM2D.Scene;
using Keyboard = SM.Base.Controls.Keyboard;

namespace IAmTwo.Game
{
    public class CreditsScene : BaseScene
    {
        private static string _credits;

        static CreditsScene()
        {
            _credits = AssemblyUtility.ReadAssemblyFile("IAmTwo.Game.Credits.txt");
        }

        private string credits;

        private DrawText _text;

        public CreditsScene(LevelSet set)
        {
            credits = _credits.Replace("%%%LEVEL%%%",  set.Credits != null ? "Level Pack Credits:\n" + set.Credits : "");
        }

        public override void Initialization()
        {
            base.Initialization();

            

            _text = new DrawText(Fonts.HeaderFont, credits)
            {
                Origin = TextOrigin.Center
            };
            _text.GenerateMatrixes();

            Camera = new Camera()
            {
                RequestedWorldScale = new Vector2(_text.Width, 0)
            };
            

            _text.Transform.Position.Y = -Camera.CalculatedWorldScale.Y / 2;
            _text.Transform.Size.Set(.75f);

            Objects.Add(_text);
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);
            const float scrollSpeed = 50;

            if (_text.Transform.Position.Y > Camera.CalculatedWorldScale.Y / 2 + _text.Height || Controller.Actor.Get<bool>("c_skipCredits"))
                ChangeScene(MainMenu.Menu);

            _text.Transform.Position.Y += context.Deltatime * scrollSpeed;
        }
    }
}