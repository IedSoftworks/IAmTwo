using IAmTwo.Resources;
using SM2D.Drawing;
using SM2D.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmTwo.Menu
{
    class DropDown : ItemCollection
    {
        DrawObject2D _border;
        DrawText _display;
        DrawText _displayArrow;

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

            Add(_border, _display, _displayArrow);
        }
    }
}
