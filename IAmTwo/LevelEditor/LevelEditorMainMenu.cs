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
                RequestedWorldScale = new Vector2(0, 1000)
            };

            Button newButton = new Button("New Level");
            newButton.Click += CreateNewLevel;

            Button loadButton = new Button("Load Level");
            loadButton.Click += LoadLevel;
            loadButton.Transform.Position.Set(0, 50);


            Objects.Add(newButton, loadButton);
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