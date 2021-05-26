using System.Collections.Generic;
using IAmTwo.Game;
using IAmTwo.LevelObjects;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using SM.Base;
using SM2D.Drawing;
using SM2D.Scene;

namespace IAmTwo.Menu.MainMenuParts
{
    public class PlayMenu : ItemCollection
    {
        private const float Offset = 20f;

        private ItemCollection _levelSelect;

        public PlayMenu(Vector2 _sceneSize)
        {
            Transform.ZIndex.Set(10);
            Vector2 size = _sceneSize * .6f;
            
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

            Button close = new Button("[X]", width:150, allowBorder: false);
            close.Click += MainMenu.Menu.HidePlayMenu;
            button.Add(close);

            int i = 1;
            foreach (KeyValuePair<string, List<LevelSet>> pair in LevelSet.LevelSets)
            {
                DrawText header = new DrawText(Fonts.Button, pair.Key);
                header.Transform.Position.Set(0, -i * Offset);
                button.Add(header);
                i++;

                foreach (LevelSet set in pair.Value)
                {
                    Button btn = new Button(set.Name, 150, allowBorder: false);
                    btn.Transform.Position.Set(0, -i * Offset);
                    btn.Click += () => SetLevelSelect(set);

                    button.Add(btn);
                    i++;
                }
            }


            DrawObject2D seperator = new DrawObject2D()
            {
                Mesh = Models.Border,
                Color = Color4.Blue,

                ShaderArguments =
                {
                    {"LineWidth", 1.5f},
                    {"Scale", 1f}
                }
            };
            seperator.Transform.Size.Set(size.Y * .95f);
            seperator.Transform.Rotation.Set(180);
            seperator.Transform.Position.Set(170, 10);

            _levelSelect = new ItemCollection();
            _levelSelect.Transform.Position.Set( 180, 0);

            button.Add(seperator, _levelSelect);

            Add(background, border, button);
        }

        private void SetLevelSelect(LevelSet set)
        {
            _levelSelect.Clear();

            int i = 0;
            foreach (LevelConstructor constructor in set.Levels)
            {
                Button btn = new Button( string.IsNullOrEmpty(constructor.LevelName) ? "UNNAMED LEVEL" : constructor.LevelName, 375, allowBorder: false);
                btn.Transform.Position.Set(0, -i * Offset);
                btn.Click += () =>
                {

                    MainMenu.Menu.ChangeScene(new PlayScene(constructor));
                };

                DrawText arrow = new DrawText(Fonts.FontAwesome, "\uf101");
                arrow.Transform.Position.Set(375, -i * Offset);

                _levelSelect.Add(btn, arrow);
                
                i++;
            }
        }
    }
}