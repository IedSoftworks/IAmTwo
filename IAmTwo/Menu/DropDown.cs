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

namespace IAmTwo.Menu
{
    class DropDown : ItemCollection
    {
        private DrawObject2D _border;
        private DrawText _display;
        private DrawText _displayArrow;
        private ItemCollection _valueCol;

        private Camera _camera;

        public DropDown(float width, string[] values)
        {
            _border = new DrawObject2D()
            {
                Mesh = Models.CreateBackgroundPolygon(new OpenTK.Vector2(width, Fonts.Button.Height), 10),
                ForcedMeshType = OpenTK.Graphics.OpenGL4.PrimitiveType.LineLoop
            };
            _border.Transform.Position.Set((width / 2) - 20, 0);
            _border.Transform.Size.Set(1);

            _display = new DrawText(Fonts.Button, "- Nothing selected! -");

            _displayArrow = new DrawText(Fonts.FontAwesome, "\uf061");
            _displayArrow.GenerateMatrixes();
            _displayArrow.Transform.Position.Set((width - _displayArrow.Width * 1.5f), -1f);

            _valueCol = new ItemCollection();
            _valueCol.Transform.Position.Set(width, 0);
            _valueCol.Active = false;

            float y = 0;
            foreach (string value in values)
            {
                DrawText valText = new DrawText(Fonts.Button, value);
                valText.GenerateMatrixes();
                valText.Transform.Position.Set(0, -y);

                _valueCol.Add(valText);
                y += valText.Height;
            }

            Add(_border, _display, _displayArrow, _valueCol);
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);

            if (_camera != null)
            {
                Vector2 mousePos = Mouse2D.InWorld(_camera);
                if (Mouse2D.MouseOver(mousePos, _border))
                {
                    _border.Color = Color4.LightBlue;

                    if (Mouse.LeftClick)
                    {
                        _valueCol.Active = !_valueCol.Active;
                    }
                }
                else
                {
                    _border.Color = Color4.Blue;
                }
            }
        }

        public override void Draw(DrawContext context)
        {
            base.Draw(context);

            _camera = context.UseCamera as Camera;
        }
    }
}
