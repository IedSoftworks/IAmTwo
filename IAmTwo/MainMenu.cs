using IAmTwo.Game;
using IAmTwo.LevelEditor;
using IAmTwo.LevelObjects;
using IAmTwo.Menu;
using IAmTwo.Resources;
using IAmTwo.Shaders;
using OpenTK;
using SM.Base;
using SM.Base.Window;
using SM2D.Drawing;
using SM2D.Scene;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IAmTwo
{
    class MainMenu : Scene
    {
        public static MainMenu Menu = new MainMenu();
        static Vector2 _sceneSize = new OpenTK.Vector2(1000, 1000 * LevelScene.Aspect);

        static Dictionary<string, Action> contactActions = new Dictionary<string, Action>()
        {
            { "Discord", DiscordAction },
            { "GitHub", GithubAction }
        };

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

            ItemCollection contacts = new ItemCollection();
            contacts.Transform.Position.Set(_sceneSize.X / 2 - 110, _sceneSize.Y / 2 - 50);

            float distance = 0;

            foreach(KeyValuePair<string, Action> pair in contactActions)
            {
                Button b = new Button(pair.Key.ToString(), 90);
                b.Click += pair.Value;
                b.Transform.Position.Set(0, -distance);

                contacts.Add(b);

                distance += b.Height + 10;
            }

            Objects.Add(buttons, _options, contacts);
            
        }

        public override void Draw(DrawContext context)
        {
            base.Draw(context);

            IAmTwo.Menu.DebugScreen.Screen.Draw(context);
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

            Button apply = new Button("Apply", 100);
            apply.Click += Apply_Click;
            button.Add(apply);

            float i = 1.2f;
            foreach(UserOption option in UserOption.Options)
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

        private void Apply_Click()
        {
            bool restartPipeline = false;

            foreach (UserOption option in UserOption.Options)
            {
                PropertyInfo property = typeof(UserSettings).GetProperty(option.Member, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (property != null)
                {
                    object data = option.GetSelectedOption();

                    if (property.GetValue(null) != data)
                    {
                        if (option.RequiresPipelineRestart) restartPipeline = true;

                        property.SetValue(null, data);
                    }
                }
            }

            if (restartPipeline)
            {
                GameRenderPipeline newPipeline = new GameRenderPipeline();
                newPipeline.Compile();

                RenderPipeline lastPipeline = SMRenderer.CurrentWindow.CurrentRenderPipeline;
                SMRenderer.CurrentWindow.SetRenderPipeline(newPipeline);

                lastPipeline.Dispose();
            }

            HideOptionMenu();
        }

        private void ShowOptionMenu()
        {
            foreach(Button button in _buttons)
            {
                button.React = false;
            }

            foreach (UserOption option in UserOption.Options)
            {
                PropertyInfo property = typeof(UserSettings).GetProperty(option.Member, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                
                if (property != null)
                {
                    switch (option.Type)
                    {
                        case OptionType.String:
                            option.SetString(property.GetValue(null).ToString());
                            break;
                        case OptionType.Bool:
                            option.SetBool((bool)property.GetValue(null));
                            break;
                    }
                }
            }

            _options.Active = true;
        }

        private void HideOptionMenu()
        {
            foreach (Button button in _buttons) button.React = true;

            _options.Active = false;
        }



        private static void GithubAction()
        {
            Process.Start("https://github.com/IedSoftworks/IAmTwo");
        }

        private static void DiscordAction()
        {

        }
    }
}
