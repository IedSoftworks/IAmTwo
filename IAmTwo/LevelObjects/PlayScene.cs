using IAmTwo.Resources;
using OpenTK;
using OpenTK.Input;
using SM.Base;
using SM.Base.Drawing.Text;
using SM.Base.Window;
using SM2D.Drawing;
using SM2D.Scene;
using Keyboard = SM.Base.Controls.Keyboard;

namespace IAmTwo.LevelObjects
{
    public class PlayScene : LevelScene
    {
        private bool _prep = true;
        private ItemCollection _start;

        private int _target = 0;

        public PlayScene(LevelConstructor constructor) : base(constructor)
        {

        }

        public override void Initialization()
        {
            base.Initialization();

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

            if (Keyboard.IsDown(Key.Escape, true))
                SMRenderer.CurrentWindow.SetScene(MainMenu.Menu);
        }

        public void AddTarget()
        {
            _target++;

            if (_target >= 2)
            {
                ItemCollection _col = new ItemCollection();

                DrawText text = new DrawText(Fonts.HeaderFont, "Level Completed!")
                {
                    Origin = TextOrigin.Center
                };

                _col.Add(text);

                HUD.Add(_col);
            }
        }
    }
}