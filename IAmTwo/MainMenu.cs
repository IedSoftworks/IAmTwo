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
using System.Windows.Forms.VisualStyles;
using IAmTwo.Menu.MainMenuParts;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SM.Base.Animation;
using SM.Base.Controls;
using SM.Base.Drawing;
using SM.Base.Drawing.Text;
using SM.Base.Time;
using SM.Utils.Controls;
using SM2D.Object;
using SM2D.Types;

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

        private GameController _listenController;

        private ItemCollection _options;
        private ItemCollection _playMenu;

        private ItemCollection _indicator;
        private DrawText _indicatorTxt;

        private InterpolationProcess _inputDeviceAnimation;
        private Timer _inputDeviceHoldTimer;

        private Button[] _buttons;

        public override void Initialization()
        {
            base.Initialization();

            _listenController = new GameController(0);


            Camera = new Camera()
            {
                RequestedWorldScale = new Vector2(700, 700 * LevelScene.Aspect)
            };
            BackgroundCamera = Camera;
            Background = new GameBackground(Camera);
            
            Player[] players = new []{ new Player(false), new Player(true) };
            int i = -1;
            foreach (Player player in players)
            {
                player.UpdateActive = false;
                player.Transform.Position.Set(200 * i, -50);

                if (i == 1) player.Transform.Size.X *= -1;
                player._head.Transform.Rotation.Set(-5);

                TimeSpan span = TimeSpan.FromSeconds(2);
                InterpolationProcess intPros = player.Transform.Position.Interpolate(span, new Vector2(player.Transform.Position.X, -55), AnimationCurves.Smooth);
                intPros.End += (timer, context) =>
                {
                    InterpolationProcess intPros2 =
                        player.Transform.Position.Interpolate(span, new Vector2(player.Transform.Position.X, -50), AnimationCurves.Smooth);
                    intPros2.End += (timer1, updateContext) => intPros.Start();
                };

                i = 1;
            }

            Objects.Add(players);

            HUDCamera = new Camera() {
                RequestedWorldScale = _sceneSize
            };

            const float offset = -40;
            const float width = 130;

            DrawText header = new DrawText(Fonts.HeaderFont, "I AM\n\tTWO")
            {
                Origin = TextOrigin.Center
            };
            header.Transform.Position.Set(0, 200);
            header.Transform.Size.Set(1);

            HUD.Add(header);

            ItemCollection buttons = new ItemCollection();
            //buttons.Transform.Position.Set(-425, -(Camera.RequestedWorldScale.Value.Y / 2) + 200);

            Button playButton = new Button("Play", width, allowBorder: false, center: true);
            playButton.Transform.Position.Set(0, offset * 0);
            playButton.Click += ShowPlayMenu;

            Button editorButton = new Button("Level Editor", width, allowBorder: false, center: true);
            editorButton.Transform.Position.Set(0, offset * 1);
            editorButton.Click += () =>
            {
                HideOptionMenu();
                SMRenderer.CurrentWindow.SetScene(LevelEditorMainMenu.Scene);
            };

            Button optionButton = new Button("Options", width, allowBorder: false, center: true);
            optionButton.Transform.Position.Set(0, offset * 2);
            optionButton.Click += ShowOptionMenu;

            Button exitButton = new Button("Exit", width, allowBorder: false, center: true);
            exitButton.Transform.Position.Set(0, offset * 3);
            exitButton.Click += () => SMRenderer.CurrentWindow.Close();
            _buttons = new Button[] { playButton, editorButton, optionButton, exitButton };
            foreach (Button b in _buttons)
            {
                b.Color = Color4.LightBlue;

                b.HoverAction = basis =>
                {
                    b.Transform.Size.Set(1.2f);
                    basis.Material.Tint = Color4.LightGreen;
                };
                b.RecoverAction = basis =>
                {
                    b.Transform.Size.Set(1f);
                    basis.Material.Tint = Color4.LightBlue;
                };
            }

            buttons.Add(_buttons);

            _options = CreateOptionMenu();
            _options.Active = false;

            _playMenu = new PlayMenu(_sceneSize);
            _playMenu.Active = false;
            
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

            DrawObject2D obj = new DrawObject2D();
            obj.Material.ShaderArguments.Add("MenuRect", Vector4.Zero);
            obj.Transform.Size.Set(HUDCamera.CalculatedWorldScale);

            _indicator = new ItemCollection();

            Polygon background = Models.CreateBackgroundPolygon(new Vector2(50, 150), 10);

            DrawObject2D back = new DrawObject2D()
            {
                Color = ColorPallete.DarkBackground,
                Mesh = background,
                Material =
                {
                    Blending = true,
                    Tint = new Color4(.005f, .005f, .005f, .5f)
                }
            };
            back.Transform.Size.Set(1);
            DrawObject2D border = new DrawObject2D()
            {
                Mesh = background,
                ForcedMeshType = PrimitiveType.LineLoop,

                Transform = back.Transform,
                Color = Color4.Blue,

                ShaderArguments =
                {
                    {"LineWidth", 1f},
                    {"Scale", 2f}
                }
            };
            _indicatorTxt = new DrawText(Fonts.FontAwesome, "\uf11c")
            {
                Origin = TextOrigin.Center
            };
            _indicatorTxt.Transform.Position.Set(-5, -55f);

            _indicator.Add(back, border, _indicatorTxt);
            _indicator.Transform.Position.Set(350, HUDCamera.CalculatedWorldScale.Y / 2 + 100);

            _inputDeviceAnimation = _indicator.Transform.Position.Interpolate(TimeSpan.FromSeconds(.5f),
                new Vector2(350, HUDCamera.CalculatedWorldScale.Y / 2 + 30), AnimationCurves.Smooth, false);
            InterpolationProcess _inDev2 = _indicator.Transform.Position.Interpolate(TimeSpan.FromSeconds(.5f), new Vector2(350, HUDCamera.CalculatedWorldScale.Y / 2 + 30),
                new Vector2(350, HUDCamera.CalculatedWorldScale.Y / 2 + 100), AnimationCurves.Smooth, false);
            

            _inputDeviceHoldTimer = new Timer(5);
            _inputDeviceHoldTimer.End += (timer1, updateContext) => _inDev2.Start();
            _inputDeviceHoldTimer.Start();

            _inputDeviceAnimation.End += (timer, context) =>
            {
                _inputDeviceHoldTimer.Start();
            };

            HUD.Add(buttons, _options, _playMenu, contacts, _indicator);
            
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);
            
            GameControllerState state = _listenController.GetState();
            if (!Controller.IsController && state.AnyInteraction) ChangeInput(true);
            if (Controller.IsController && (Keyboard.IsAnyKeyPressed || Mouse.LeftClick || Mouse.RightClick)) ChangeInput(false);
        }

        private void ChangeInput(bool controller)
        {
            Controller.Actor = controller ? GameKeybindActor.CreateControllerActor(0) : GameKeybindActor.CreateKeyboardActor();

            (SMRenderer.CurrentWindow as GLWindow).CursorVisible = !controller;

            _indicatorTxt.Text = controller ? "\uf11b" : "\uf11c";
            Mouse.StopTracking = controller;

            Button editor = _buttons[1];
            editor.React = !controller;
            editor.Color = controller ? ColorPallete.Background : Color4.LightBlue;


            if (!(_inputDeviceAnimation.Running && _inputDeviceHoldTimer.Running)) _inputDeviceAnimation.Start();
        }

        public override void Draw(DrawContext context)
        {
            base.Draw(context);
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
            DrawObject2D border = new DrawObject2D()
            {
                Mesh = background.Mesh,
                ForcedMeshType = PrimitiveType.LineLoop,

                Transform = background.Transform,
                Color = Color4.Blue,

                ShaderArguments =
                {
                    {"LineWidth", 1f},
                    {"Scale", .5f}
                }
            };

            ItemCollection button = new ItemCollection();
            button.Transform.Position.Set(-(size.X / 2) + 20, (size.Y / 2) - 20);

            Button apply = new Button("Apply", 100, allowBorder:false);
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



            col.Add(background, border, button);

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

        private void ShowPlayMenu()
        {
            foreach (Button button in _buttons)
            {
                button.React = false;
            }

            LevelSet.Load();
            _playMenu.Active = true;
        }

        public void HidePlayMenu()
        {
            _playMenu.Active = false;

            foreach (Button button in _buttons)
            {
                button.React = true;
            }
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
