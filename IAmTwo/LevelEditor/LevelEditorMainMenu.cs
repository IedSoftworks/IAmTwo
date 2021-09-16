using System.IO;
using System.Windows.Forms;
using IAmTwo.Game;
using IAmTwo.LevelObjects;
using OpenTK;
using SM.Base;
using SM.Base.Scene;
using SM.Base.Window;
using SM2D.Scene;
using Button = IAmTwo.Menu.Button;

namespace IAmTwo.LevelEditor
{
    public class LevelEditorMainMenu : BaseScene
    {
        public static LevelEditorMainMenu Scene = new LevelEditorMainMenu();

        private LevelSetEditor editor;

        private LevelEditorMainMenu() {}

        public override void Initialization()
        {
            base.Initialization();

            Camera = new Camera()
            {
                RequestedWorldScale = MainMenu._sceneSize
            };
            BackgroundCamera = Camera;

            Background = new GameBackground(BackgroundCamera);

            Button newButton = new Button("New Level", 150, center:true, allowBorder: false);
            newButton.Click += CreateNewLevel;
            newButton.Transform.Position.Set(0, 100);

            Button loadButton = new Button("Load Level", 150, center: true, allowBorder: false);
            loadButton.Click += LoadLevel;
            loadButton.Transform.Position.Set(0, 50);
            /*
            Button packageButton = new Button("Show Package Editor", 200, center: true, allowBorder: false);
            packageButton.Click += ShowPackageManager;
            packageButton.Transform.Position.Set(150, 75);*/

            Button backButton = new Button("Return", 100, center: true);
            backButton.Click += () =>
            {
                LevelSet.Load();
                ChangeScene(MainMenu.Menu);
            };
            backButton.Transform.Position.Set(0, -200);


            Objects.Add(newButton, loadButton, /*packageButton,*/ backButton);
        }

        private void ShowPackageManager()
        {
            if (editor != null)
            {
                HidePackageManager();
            }

            foreach (IShowItem o in Objects)
            {
                if (o is IScriptable s) s.UpdateActive = false;
            }

            editor = new LevelSetEditor(MainMenu._sceneSize);

            Objects.Add(editor);

        }

        public void HidePackageManager()
        {
            Objects.Remove(editor);

            foreach (IShowItem o in Objects)
            {
                if (o is IScriptable s) s.UpdateActive = true;
            }
        }

        public static void LoadLevel()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = ".iatl";
            ofd.Multiselect = false;
            ofd.InitialDirectory = Path.GetFullPath("Levels");
            ofd.Filter = "I AM TWO Levels (*.iatl)|*.iatl|All Files (*.*)|*.*";
            
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
        }
    }
}