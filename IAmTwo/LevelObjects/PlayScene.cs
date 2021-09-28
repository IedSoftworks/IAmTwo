using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using IAmTwo.Game;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Input;
using SM.Base;
using SM.Base.Drawing.Text;
using SM.Base.Scene;
using SM.Base.Window;
using SM2D.Drawing;
using SM2D.Scene;
using Keyboard = SM.Base.Controls.Keyboard;
using IAmTwo.LevelObjects.Objects.SpecialObjects;

namespace IAmTwo.LevelObjects
{
    public class PlayScene : LevelScene
    {
        static Dictionary<string, Tuple<string, string>> _endKeybinds = new Dictionary<string, Tuple<string, string>>()
        {
            {"Continue", new Tuple<string, string>("Space", "A")}, 
            {"Retry", new Tuple<string, string>("R", "Y")},
            {"Main-Menu", new Tuple<string, string>("ESC", "B")}
        };

        private bool _prep = true;
        private bool _completed = false;

        private ItemCollection _start;
        private ItemCollection _end;

        private int _inGoal = 0;
        private int _targetInGoal;

        public PlayScene(LevelConstructor constructor) : base(constructor)
        {

        }

        public override void Initialization()
        {
            base.Initialization();

            Type targetType = typeof(Goal);
            _targetInGoal = Constructor.Objects.Count(a => a.ObjectType == targetType);

            HUDCamera = new Camera()
            {
                RequestedWorldScale = new Vector2(1000, 1000 * Aspect)
            };

            _start = new ItemCollection();

            DrawText lvl = new DrawText(Fonts.HeaderFont, Constructor.LevelName)
            {
                Origin = TextOrigin.Center
            };
            lvl.Transform.Position.Y = 240;
            lvl.Transform.Size.Set(.5f);

            DrawText prompt = Controller.IsController ? new DrawText(Controller.IsPS ? Fonts.PS : Fonts.XBOX, Controller.GetCorrectName("A")) : new DrawText(Fonts.Button, $"[Space]")
            {
                Origin = TextOrigin.Center
            };
            prompt.Transform.Position.Y = -240;

            _start.Add(lvl, prompt);

            _end = new ItemCollection();

            DrawText complete = new DrawText(Fonts.HeaderFont, "Level Completed!")
            {
                Origin = TextOrigin.Center,

            };
            complete.Transform.Position.Y = 200;
            complete.Transform.Size.Set(.5f);

            int i = - (_endKeybinds.Count - 1) / 2;

            float dif = 1000f / _endKeybinds.Count;

            foreach (KeyValuePair<string, Tuple<string, string>> pair in _endKeybinds)
            {
                ItemCollection col = new ItemCollection();

                DrawText p = Controller.IsController
                    ? new DrawText(Controller.IsPS ? Fonts.PS : Fonts.XBOX, Controller.GetCorrectName(pair.Value.Item2))
                    : new DrawText(Fonts.Button, $"[{pair.Value.Item1}]");
                p.GenerateMatrixes();

                DrawText header = new DrawText(Fonts.Button, pair.Key);
                header.GenerateMatrixes();
                header.Transform.Position.Set(p.Width, 0);

                col.Add(p, header);

                col.Transform.Position.Set(i * dif - (p.Width + header.Width) / 2, -200);
                _end.Add(col);

                i++;
            }

            _end.Add(complete);

            HUD.Add(_start);
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);

            if (_prep && Controller.Actor.Get<bool>("l_start"))
            {
                HUD.Remove(_start);
                Spawn();
                _prep = false;
            }


            if (_completed)
            {
                if (Controller.Actor.Get<bool>("l_continue"))
                {
                    LevelSet set = Constructor.Set;
                    ChangeScene(Constructor.SetIndex == set.Levels.Count - 1 ? (GenericScene)new CreditsScene(set) : new PlayScene(Constructor.Set.Levels[Constructor.SetIndex + 1]));
                }


            }

            if (Controller.Actor.Get<bool>("l_retry")) SMRenderer.CurrentWindow.SetScene(new PlayScene(Constructor));

            if (Controller.Actor.Get<bool>("l_exit")) ChangeScene(MainMenu.Menu);
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public void AddTarget()
        {
            _inGoal++;

            if (_inGoal >= _targetInGoal)
            {
                HUD.Add(_end);
                _completed = true;
            }
        }
    }
}