using IAmTwo.Game;
using IAmTwo.LevelObjects;
using IAmTwo.LevelObjects.Objects;
using KWEngine.Hitbox;
using OpenTK.Input;
using SM.Base;
using SM.Base.Windows;
using SM2D.Scene;
using Keyboard = SM.Base.Controls.Keyboard;

namespace IAmTwo.LevelEditor
{
    public class TestLevel : LevelScene
    {

        public TestLevel(LevelConstructor constructor) : base(constructor)
        {
            
        }

        public override void Initialization()
        {
            base.Initialization();

            Spawn();
        }

        public override void Update(UpdateContext context)
        {
            if (Keyboard.IsDown(Key.F5, true) || Keyboard.IsDown(Key.Escape, true))
                SMRenderer.CurrentWindow.SetScene(LevelEditor.CurrentEditor);

            base.Update(context);
        }
    }
}