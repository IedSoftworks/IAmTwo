using System.IO;
using System.Windows.Forms;
using IAmTwo.LevelObjects;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using SM.Base;
using SM2D.Drawing;
using SM2D.Scene;
using Button = IAmTwo.Menu.Button;
using Keyboard = SM.Base.Controls.Keyboard;

namespace IAmTwo.LevelEditor
{
    public class EscapeControl : LevelEditorDialog
    {
        public static Vector2 Size = new Vector2(500, 400);

        private Button _newL;
        private Button save;
        private Button load;
        private Button exit;

        private Button _testLevelButton;

        public override DrawObject2D Background { get; set; }

        public EscapeControl() : base(Size)
        {
            ItemCollection items = new ItemCollection();
            items.Transform.Position.Set(0, Size.Y / 2 - 30);
            items.Transform.Size.Set(.8f);

            ItemCollection testActions = CreateTestActions();
            testActions.Transform.Position.Set(-(Size.X / 2) - 50, 0);

            float borderBrightness = .2f;
            DrawObject2D itemsBorder = new DrawObject2D {Mesh = Models.Border, Color = new Color4(borderBrightness, borderBrightness, borderBrightness, 1f)};
            itemsBorder.Transform.Position.Set(0, 0);
            itemsBorder.Transform.Rotation.Set(180);
            itemsBorder.Transform.Size.Set(1, Size.Y);
            itemsBorder.ShaderArguments["LineWidth"] = 1f;

            ItemCollection fileActions = CreateFileActions();
            fileActions.Transform.Position.Set(20, 0);

            items.Add(testActions, itemsBorder, fileActions);

            Add(items);
        }

        private ItemCollection CreateTestActions()
        {
            ItemCollection col = new ItemCollection();

            _testLevelButton = new Button("Test Level\n\t[F2]", 150);
            _testLevelButton.Transform.Position.Set(10, 0);
            _testLevelButton.Click += () => LevelEditor.CurrentEditor.StartTestLevel(false);

            col.Add(_testLevelButton);

            return col;
        }


        public override void Keybinds()
        {
            base.Keybinds();

            if (Keyboard.IsDown(Key.F2, true))
            {
                _testLevelButton.TriggerClick();
            }

            if (Keyboard.IsDown(Key.ControlLeft))
            {
                if (Keyboard.IsDown(Key.N)) _newL.TriggerClick();
                if (Keyboard.IsDown(Key.S, true)) save.TriggerClick();
                if (Keyboard.IsDown(Key.L, true)) load.TriggerClick();
                if (Keyboard.IsDown(Key.X, true)) exit.TriggerClick();
            }
        }

        public override bool Input()
        {
            return Keyboard.IsDown(Key.Escape, true);
        }

        private ItemCollection CreateFileActions()
        {
            ItemCollection col = new ItemCollection();

            DrawText header = new DrawText(Fonts.Text, "Save/Load");

            int width = 130;
            float offset = -60;

            _newL = new Button("New Level \n  [CTRL-N]", width);
            _newL.Transform.Position.Set(10, offset);
            _newL.Click += () => SMRenderer.CurrentWindow.SetScene(new LevelEditor(new LevelConstructor()));
            save = new Button("Save Level\n  [CTRL-S]", width);
            save.Transform.Position.Set(10, offset * 2);
            save.Click += SaveFile;
            load = new Button("Load Level\n  [CTRL-L]", width);
            load.Transform.Position.Set(10, offset * 3);
            load.Click += LevelEditorMainMenu.LoadLevel;

            exit = new Button("Exit Editor\n  [CTRL-X]", width);
            exit.Transform.Position.Set(10, -400);
            exit.Click += () => SMRenderer.CurrentWindow.SetScene(LevelEditorMainMenu.Scene);

            col.Add(header, _newL, save, load, exit);

            return col;
        }

        private void SaveFile()
        {
            LevelConstructor constructor = LevelEditor.CurrentEditor.Constructor;

            if (string.IsNullOrEmpty(constructor.LevelPath))
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.DefaultExt = ".iatl";
                sfd.Title = "Select a path where to store the level.";
                sfd.InitialDirectory = Path.GetFullPath("Levels");
                sfd.OverwritePrompt = true;
                
                if (sfd.ShowDialog() != DialogResult.OK) return;

                constructor.LevelPath = sfd.FileName;
            }

            if (File.Exists(constructor.LevelPath)) File.Delete(constructor.LevelPath);
            using (FileStream stream = new FileStream(constructor.LevelPath, FileMode.OpenOrCreate))
                constructor.Store(stream, LevelEditor.CurrentEditor.Objects);

        }
    }
}