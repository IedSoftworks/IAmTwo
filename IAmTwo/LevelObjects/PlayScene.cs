using OpenTK.Input;
using SM.Base;
using SM.Base.Window;
using Keyboard = SM.Base.Controls.Keyboard;

namespace IAmTwo.LevelObjects
{
    public class PlayScene : LevelScene
    {
        public PlayScene(LevelConstructor constructor) : base(constructor)
        {

        }

        public override void Initialization()
        {
            base.Initialization();

            Spawn();
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);

            if (Keyboard.IsDown(Key.Escape, true))
                SMRenderer.CurrentWindow.SetScene(MainMenu.Menu);
        }
    }
}