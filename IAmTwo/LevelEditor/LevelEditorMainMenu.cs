using System.IO;
using System.Windows.Forms;
using IAmTwo.LevelObjects;
using OpenTK;
using SM.Base;
using SM.Base.Window;
using SM2D.Scene;
using Button = IAmTwo.Menu.Button;
using MouseCursor = IAmTwo.Menu.MouseCursor;

namespace IAmTwo.LevelEditor
{
    public class LevelEditorMainMenu : Scene
    {
        public static LevelEditorMainMenu Scene = new LevelEditorMainMenu();

        private LevelEditorMainMenu() {}

        public override void Initialization()
        {
            base.Initialization();

            Camera = new Camera()
            {
                RequestedWorldScale = new Vector2(0, 500)
            };

            Button newButton = new Button("New Level", 150);
            newButton.Click += CreateNewLevel;

            Button loadButton = new Button("Load Level", 150);
            loadButton.Click += LoadLevel;
            loadButton.Transform.Position.Set(0, 50);

            Button backButton = new Button("Return", 150);
            backButton.Click += () => SMRenderer.CurrentWindow.SetScene(MainMenu.Menu);
            backButton.Transform.Position.Set(0, -200);


            Objects.Add(newButton, loadButton, backButton);
        }

        public static void LoadLevel()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".iatl";
            ofd.Multiselect = false;
            ofd.Filter = "I AM TWO Levels (*.iatl) | *.iatl | All Files (*.*) | *.*";
            
            if (ofd.ShowDialog() != DialogResult.OK) return;

            LevelConstructor constructor;
            using (FileStream stream = new FileStream(ofd.FileName, FileMode.Open))
            {
                constructor = LevelConstructor.Load(stream);
            }

            constructor.LevelPath = ofd.FileName;
            SMRenderer.CurrentWindow.SetScene(new LevelEditor(constructor));
        }

        private void CreateNewLevel()
        {
            LevelConstructor level = new LevelConstructor();

            SMRenderer.CurrentWindow.SetScene(new LevelEditor(level));
        }

        public override void Draw(DrawContext context)
        {
            base.Draw(context);

            MouseCursor.Cursor.Draw(context);
        }
    }
}