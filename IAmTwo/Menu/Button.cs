using System;
using System.Drawing;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using SM.Base.Drawing;
using SM.Base.Objects.Static;
using SM.Base.Window;
using SM.OGL.Mesh;
using SM2D.Controls;
using SM2D.Drawing;
using SM2D.Object;
using SM2D.Scene;
using Font = SM.Base.Drawing.Text.Font;
using Mouse = SM.Base.Controls.Mouse;

namespace IAmTwo.Menu
{
    public class Button : ItemCollection
    {
        private Camera camera;

        protected DrawObject2D _border;
        protected Polygon _borderMesh;
        protected DrawText _text;

        public event Action Click;
        public bool React = true;
        public float Width;
        public float Height;

        public bool EnableBorder = true;

        public Button(string text, float? width = null, Font font = null, bool allowBorder = true)
        {
            _text = new DrawText(font ?? Fonts.Button, text);
            _text.GenerateMatrixes();
            Width = _text.Width;
            Height = _text.Height;

            float w = width ?? _text.Width;


            EnableBorder = allowBorder;

            if (allowBorder)
            {
                _borderMesh = Models.CreateBackgroundPolygon(new Vector2(w, _text.Height), 10);
            }

            _border = new DrawObject2D()
            {
                Mesh = allowBorder ? (GenericMesh)_borderMesh : Plate.Object,
                ForcedMeshType = PrimitiveType.LineLoop,
                ShaderArguments = { ["ColorScale"] = 1.4f },
                RenderActive = allowBorder
            };
            if (allowBorder)
            {
                _border.Transform.Size.Set(1.2f);
            } else _border.Transform.Size.Set(new Vector2(w, _text.Height));

            _border.Transform.Position.Set(w / 2 - 10, -(_text.Height - Fonts.Button.Height) / 2);

            Add(_text, _border);
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);

            DrawingBasis obj = (EnableBorder ? (DrawingBasis)_border : _text);

            if (React && camera != null && Mouse2D.MouseOver(Mouse2D.InWorld(camera), out _, _border))
            {
                obj.Material.Tint = Color4.LightBlue;
                if (Mouse.IsDown(MouseButton.Left, true)) TriggerClick();
            }
            else obj.Material.Tint = Color4.Blue;
        }

        public override void Draw(DrawContext context)
        {
            camera = context.UseCamera as Camera;

            base.Draw(context);
            if (!EnableBorder) _border.Transform.LastMaster = this.Transform.InWorldSpace;
        }

        public void TriggerClick()
        {
            Click?.Invoke();
        }
    }
}