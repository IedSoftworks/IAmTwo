using IAmTwo.Resources;
using SM2D.Drawing;
using SM2D.Scene;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using SM.Base.Controls;
using SM.Base.Window;
using SM2D.Controls;
using IAmTwo.Game;
using SM.Base.Drawing.Text;
using SM2D.Types;
using SM.Base.Objects.Static;

namespace IAmTwo.Menu
{
    public class DropDown : ItemCollection
    {
        private DrawObject2D _border;
        private DrawText _display;
        private DrawText _displayArrow;
        private ItemCollection _valueCol;

        private Dictionary<DrawText, Transformation> _texts;

        private DrawText _selected;

        public string SelectedText => _selected.Text;

        public DropDown(float width, string[] values)
        {
            Transform.ZIndex.Set(1);

            _border = new DrawObject2D()
            {
                Mesh = Models.CreateBackgroundPolygon(new OpenTK.Vector2(width, Fonts.Button.Height), 10),
                ForcedMeshType = OpenTK.Graphics.OpenGL4.PrimitiveType.LineLoop
            };
            _border.Transform.Position.Set((width / 2) - 20, 0);
            _border.Transform.Size.Set(1);

            _display = new DrawText(Fonts.Button, "- Nothing selected! -");

            _displayArrow = new DrawText(Fonts.FontAwesome, "\uf061")
            {
                Origin = TextOrigin.Center
            };
            _displayArrow.GenerateMatrixes();
            _displayArrow.Transform.Position.Set((width - _displayArrow.Width - 20), 0);

            _valueCol = new ItemCollection();
            _valueCol.Transform.Position.Set(width, 0);
            _valueCol.Active = false;

            _texts = new Dictionary<DrawText, Transformation>();
            float y = 0;
            float x = 0;
            foreach (string value in values)
            {
                DrawText valText = new DrawText(Fonts.Button, value);
                valText.GenerateMatrixes();
                valText.Transform.Position.Set(0, -y);

                Transformation t = new Transformation();
                t.Size.Set(valText.Width, valText.Height);
                t.Position.Set(valText.Width / 2, -y);
                t.GetMatrix();

                _texts.Add(valText, t);
                y += valText.Height;
                x = Math.Max(x, valText.Width);
            }

            DrawObject2D valuesBack = new DrawObject2D()
            {
                Color = ColorPallete.LightBackground,
                Mesh = Models.CreateBackgroundPolygon(new Vector2(x + 20, y + 10), 10)
            };
            valuesBack.Transform.Position.Set((x / 2) - 1, -(y  / 2) + 10);
            valuesBack.Transform.Size.Set(1);
            valuesBack.Transform.ZIndex.Set(-1);
            _valueCol.Add(valuesBack);
            _valueCol.AddRange(_texts.Keys);

            Add(_border, _display, _displayArrow, _valueCol);
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);

            if (_border.LastDrawingCamera != null)
            {
                Vector2 mousePos = Mouse2D.InWorld(_border.LastDrawingCamera as Camera);
                if (Mouse2D.MouseOver(mousePos, _border))
                {
                    _border.Color = Color4.LightBlue;

                    if (Controller.Actor.Get<bool>("g_click"))
                    {
                        _valueCol.Active = !_valueCol.Active;
                        _displayArrow.Transform.Rotation.Interpolate(TimeSpan.FromSeconds(.1f), _valueCol.Active ? 180 : 0);
                    }
                }
                else
                {
                    _border.Color = Color4.Blue;
                }

                if (_valueCol.Active)
                {
                    foreach (KeyValuePair<DrawText, Transformation> pair in _texts) {
                        if (_selected == pair.Key) continue;

                        if (Mouse2D.MouseOver(mousePos, Plate.Object.BoundingBox, pair.Value))
                        {
                            pair.Key.Color = Color4.Beige;

                            if (Controller.Actor.Get<bool>("g_click"))
                            {
                                SetValue(pair.Key);
                            }
                        }
                        else pair.Key.Color = Color4.Aqua;
                    }
                }
            }
        }

        public override void Draw(DrawContext context)
        {
            base.Draw(context);

            if (_valueCol.Active) 
                foreach(Transformation t in _texts.Values)
                {
                    t.LastMaster = _valueCol.Transform.InWorldSpace;
                }
        }

        public void SetValue(string value)
        {
            DrawText drawing = _texts.FirstOrDefault(a => a.Key.Text == value).Key;
            if (drawing != null) SetValue(drawing);
        }

        private void SetValue(DrawText text)
        {
            _selected = text;
            text.Color = new Color4(0, 1f, 0f, 1f);

            _display.Text = text.Text;
            if (_valueCol.Active) _valueCol.Active = false;
        }
    }
}
