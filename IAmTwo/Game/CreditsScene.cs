using System;
using System.ComponentModel;
using System.IO;
using IAmTwo.Resources;
using OpenTK;
using SM.Base;
using SM.Base.Drawing.Text;
using SM.Base.Utility;
using SM.Base.Window;
using SM2D.Drawing;
using SM2D.Scene;

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
            credits = set.Credits != null ? _credits.Replace("%%%LEVEL%%%", "Level Pack Credits:\n" + set.Credits) : null;
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
                RequestedWorldScale = new Vector2(_text.Width, 1000)
            };

            _text.Transform.Position.Y = -500 - _text.Font.Height / 2;
            _text.Transform.Size.Set(.75f);

            Objects.Add(_text);
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);
            const float scrollSpeed = 50;

            if (_text.Transform.Position.Y > 500 + _text.Height)
                ChangeScene(MainMenu.Menu);

            _text.Transform.Position.Y += context.Deltatime * scrollSpeed;
        }
    }
}