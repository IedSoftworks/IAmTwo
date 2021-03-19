using IAmTwo.Game;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using SM.Base.Controls;
using SM.Base.Objects.Static;
using SM.Base.Window;
using SM2D.Controls;
using SM2D.Drawing;
using SM2D.Scene;
using SM2D.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmTwo.Menu
{
    class OptionSelector : ItemCollection
    {
        private Dictionary<string, DrawText> _textLocation = new Dictionary<string, DrawText>();
        private Dictionary<DrawText, DrawObject2D> _buttonBackgrounds = new Dictionary<DrawText, DrawObject2D>();
        private Camera _camera;
        private DrawText _selected;

        public string SelectedText => _selected.Text;
        public OptionSelector(string[] values)
        {
            Transform.Size.Set(.8f);

            float x = 10;
            float h = 0;
            foreach(string value in values)
            {
                DrawText txt = new DrawText(Fonts.Button, value);
                txt.GenerateMatrixes();
                txt.Transform.Position.Set(x, 0);

                DrawObject2D obj = new DrawObject2D()
                {
                    Color = new Color4(1, 1, 1, 0)
                };
                obj.Material.Blending = true;
                obj.Transform.Size.Set(txt.Width, txt.Height);
                obj.Transform.Position.Set((txt.Width/ 2) + (x - 7), 0);

                Add(obj, txt);
                _buttonBackgrounds.Add(txt, obj);
                _textLocation.Add(value, txt);
                x += txt.Width + 10;
                h = Math.Max(txt.Height, h);
            }

            DrawObject2D border = new DrawObject2D()
            {
                Mesh = Models.CreateBackgroundPolygon(new OpenTK.Vector2(x, h), 10),
                ForcedMeshType = OpenTK.Graphics.OpenGL4.PrimitiveType.LineLoop
            };
            border.Color = Color4.Blue;
            border.Transform.Size.Set(1.1f);
            border.Transform.Position.Set(x / 2 - 10, 0);
            
            Add(border);
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);

            if (_camera != null)
            {
                Vector2 mousePos = Mouse2D.InWorld(_camera);
                foreach (KeyValuePair<DrawText, DrawObject2D> pair in _buttonBackgrounds)
                {
                    if (pair.Key == _selected) continue;

                    pair.Value.Color = new Color4(0, 0, 0, 0);
                    if (Mouse2D.MouseOver(mousePos, pair.Value))
                    {
                        pair.Key.Color = Color4.LightBlue;

                        if (Mouse.IsDown(OpenTK.Input.MouseButton.Left, true))
                        {
                            _selected = pair.Key;
                            pair.Key.Color = new Color4(0,1f, 0f, 1f);
                        }
                    }
                    else pair.Key.Color = Color4.Blue;
                }
            }
        }

        public override void Draw(DrawContext context)
        {
            base.Draw(context);

            _camera = context.UseCamera as Camera;

        }

        public void Select(string value)
        {
            if (_textLocation.ContainsKey(value))
            {

                _selected = _textLocation[value];
                _selected.Color = new Color4(0, 1f, 0, 1f);
            }
        }
    }
}
