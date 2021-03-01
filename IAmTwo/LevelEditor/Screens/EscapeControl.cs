using System;
using IAmTwo.Game;
using IAmTwo.Menu;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using SM2D.Drawing;
using SM2D.Scene;
using Keyboard = SM.Base.Controls.Keyboard;

namespace IAmTwo.LevelEditor
{
    public class EscapeControl : LevelEditorMenu
    {
        public static Vector2 Size = new Vector2(500, 400);

        private Button _testLevelButton;

        public override DrawObject2D Background { get; set; }

        public EscapeControl()
        {
            Background = new DrawObject2D {Color = ColorPallete.Background};
            Background.Transform.Size.Set(Size);

            DrawObject2D border = new DrawObject2D
            {
                Color = Color4.Red, 
                Mesh = Models.QuadricBorder
            };
            border.Transform.Size.Set(Size);

            ItemCollection items = new ItemCollection();
            items.Transform.Size.Set(.8f);

            ItemCollection testActions = CreateTestActions();
            testActions.Transform.Position.Set(-(Size.X / 2) - 50, Size.Y / 2 + 40);

            items.Add(testActions);

            Add(Background, border, items);
        }

        private ItemCollection CreateTestActions()
        {
            ItemCollection col = new ItemCollection();

            _testLevelButton = new Button("Test Level [F2]", -10, 150);
            _testLevelButton.Transform.Position.Set(10, -50);
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
        }

        public override bool Input()
        {
            return Keyboard.IsDown(Key.Escape, true);
        }

        public override void Open()
        {
            base.Open();
        }

        public override void Close()
        {
            base.Close();
        }
    }
}