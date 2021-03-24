using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using SM.Base.Controls;
using SM.Base.Window;
using SM2D.Controls;
using SM2D.Drawing;
using SM2D.Scene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmTwo.Menu
{
    class CheckBox : ItemCollection
    {
        public static readonly Vector2 Size = new Vector2(25);

        private DrawText _check;
        private DrawObject2D _border;

        private Camera _lastCamera;

        public bool Checked { get; private set; }

        public CheckBox()
        {
            _border = new DrawObject2D()
            {
                Mesh = Models.CreateBackgroundPolygon(Size, 5),
                ForcedMeshType = OpenTK.Graphics.OpenGL4.PrimitiveType.LineLoop
            };
            _border.Transform.Size.Set(1);
            
            _check = new DrawText(Fonts.FontAwesome, "\uf00c");
            _check.Transform.Position.Set(0, -2);
            Add(_border, _check);
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);
            if (_lastCamera == null) return;
            
            Vector2 mousePos = Mouse2D.InWorld(_lastCamera);
            if (Mouse2D.MouseOver(mousePos, _border))
            {
                _border.Color = Color4.AliceBlue;

                if (Mouse.LeftClick)
                {
                    Checked = !Checked;

                    _check.Active = Checked;
                }
            }
            else _border.Color = Color4.Blue;
        }

        public override void Draw(DrawContext context)
        {
            _lastCamera = context.UseCamera as Camera;

            base.Draw(context);
        }

        public void SetChecked(bool value)
        {
            Checked = value;
            _check.Active = value;
        }
    }
}
