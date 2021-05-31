using IAmTwo.LevelObjects;
using OpenTK.Input;
using SM.Base;
using SM.Base.Window;
using Keyboard = SM.Base.Controls.Keyboard;

namespace IAmTwo.LevelEditor
{
    public class TestLevel : LevelScene
    {

        public TestLevel(LevelConstructor constructor) : base(constructor)
        {
            PlayActivationAnimation = false;
        }

        public override void Initialization()
        {
            base.Initialization();

            Spawn();
        }

        public override void Update(UpdateContext context)
        {
            if (Keyboard.IsDown(Key.F2, true) || Keyboard.IsDown(Key.Escape, true))
                SMRenderer.CurrentWindow.SetScene(LevelEditor.CurrentEditor);

            base.Update(context);
        }
    }
}