using System.Dynamic;
using IAmTwo.LevelObjects;
using IAmTwo.Menu;
using OpenTK;
using SM.Base.Windows;
using SM2D.Scene;

namespace IAmTwo.LevelEditor
{
    public class LevelEditorMainMenu : Scene
    {
        public override void Initialization()
        {
            base.Initialization();

            Camera = new Camera()
            {
                RequestedWorldScale = new Vector2(0, 1000)
            };

            Button newButton = new Button("New Level");
            newButton.Click += CreateNewLevel;


            Objects.Add(newButton);
        }

        private void CreateNewLevel(UpdateContext context)
        {
            LevelConstructor level = new LevelConstructor();

            context.Window.SetScene(new LevelEditor(level));
        }
    }
}