using IAmTwo.Game;
using IAmTwo.LevelEditor;
using IAmTwo.LevelObjects;
using IAmTwo.Menu;
using IAmTwo.Resources;
using OpenTK;
using SM.Base;
using SM.Base.Window;
using SM2D.Drawing;
using SM2D.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmTwo
{
    class MainMenu : Scene
    {
        public static MainMenu Menu = new MainMenu();
        static Vector2 _sceneSize = new OpenTK.Vector2(1000, 1000 * LevelScene.Aspect);

        private ItemCollection _options;
        private Button[] _buttons;        

        public override void Initialization()
        {
            base.Initialization();

            Camera = new Camera() {
                RequestedWorldScale = _sceneSize
            };
            Background.Color = ColorPallete.Background;

            const float offset = -40;
            const float width = 130;

            ItemCollection buttons = new ItemCollection();
            buttons.Transform.Position.Set(-425, -(Camera.RequestedWorldScale.Value.Y / 2) + 200);

            Button playButton = new Button("Play", width);
            playButton.Transform.Position.Set(0, offset * 0);

            Button editorButton = new Button("Level Editor", width);
            editorButton.Transform.Position.Set(0, offset * 1);
            editorButton.Click += () =>
            {
                HideOptionMenu();
                SMRenderer.CurrentWindow.SetScene(LevelEditorMainMenu.Scene);
            };
            Button optionButton = new Button("Options", width);
            optionButton.Transform.Position.Set(0, offset * 2);
            optionButton.Click += () => ShowOptionMenu();

            Button exitButton = new Button("Exit", width);
            exitButton.Transform.Position.Set(0, offset * 3);
            exitButton.Click += () => SMRenderer.CurrentWindow.Close();
            _buttons = new Button[] { playButton, editorButton, optionButton, exitButton };

            buttons.Add(_buttons);

            _options = CreateOptionMenu();
            _options.Active = false;

            Objects.Add(buttons, _options);
            
        }

        public override void Draw(DrawContext context)
        {
            base.Draw(context);

            IAmTwo.Menu.MouseCursor.Cursor.Draw(context);
        }

        private ItemCollection CreateOptionMenu()
        {
            Vector2 size = _sceneSize * .75f;
            const float offset = 30;

            ItemCollection col = new ItemCollection();

            DrawObject2D background = new DrawObject2D()
            {
                Mesh = Models.CreateBackgroundPolygon(size, 10),
                Color = ColorPallete.DarkBackground
            };
            background.Transform.Size.Set(1);

            ItemCollection button = new ItemCollection();
            button.Transform.Position.Set(-(size.X / 2) + 20, (size.Y / 2) - 20);

            int i = 0;
            foreach(UserOption option in UserSettings.Options)
            {
                DrawText title = new DrawText(Fonts.Button, option.Name);
                title.Transform.Position.Set(0, -offset * i);

                ItemCollection visual = option.GetVisual();
                visual.Transform.Position.Set(size.X / 2, -offset * i);

                button.Add(title, visual);
                i++;
            }

            col.Add(background, button);

            return col;
        }

        private void ShowOptionMenu()
        {
            foreach(Button button in _buttons)
            {
                button.React = false;
            }
            _options.Active = true;
        }
        private void HideOptionMenu()
        {
            _options.Active = false;
        }
    }
}
