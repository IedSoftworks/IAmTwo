using System;
using IAmTwo.Game;
using IAmTwo.LevelObjects;
using IAmTwo.Menu;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using SM.Base;
using SM.Base.Objects;
using SM.Base.Windows;
using SM2D.Drawing;
using SM2D.Scene;
using Keyboard = SM.Base.Controls.Keyboard;

namespace IAmTwo.LevelEditor
{
    public class EscapeControl : LevelEditorMenu
    {
        public static Vector2 Size = new Vector2(500, 400);

        private Button _newL;
        private Button save;
        private Button load;
        private Button exit;

        private Button _testLevelButton;

        public override DrawObject2D Background { get; set; }

        public EscapeControl()
        {
            Background = new DrawObject2D {Color = ColorPallete.Background};
            Background.Transform.Size.Set(Size);

            DrawObject2D border = new DrawObject2D
            {
                Color = Color4.Red, 
                Mesh = Models.QuadricBorderNotConnected
            };
            border.ShaderArguments["ColorScale"] = 1.2f;
            border.Transform.Size.Set(Size);

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

            Add(Background, border, items);
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
            save.Click += () => LevelEditor.CurrentEditor.SaveDialog.Show();
            load = new Button("Load Level\n  [CTRL-L]", width);
            load.Transform.Position.Set(10, offset * 3);

            exit = new Button("Exit Editor\n  [CTRL-X]", width);
            exit.Transform.Position.Set(10, -400);
            exit.Click += () => SMRenderer.CurrentWindow.SetScene(LevelEditorMainMenu.Scene);

            col.Add(header, _newL, save, load, exit);

            return col;
        }
    }
}