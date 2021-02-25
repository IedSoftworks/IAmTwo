using System.Dynamic;
using IAmTwo.LevelObjects;
using IAmTwo.Menu;
using OpenTK;
using SM.Base;
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

        private void CreateNewLevel()
        {
            LevelConstructor level = new LevelConstructor();

            SMRenderer.CurrentWindow.SetScene(new LevelEditor(level));
        }
    }
}