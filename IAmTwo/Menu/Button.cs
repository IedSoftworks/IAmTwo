using System;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using SM.Base.Drawing.Text;
using SM.Base.Window;
using SM2D.Controls;
using SM2D.Drawing;
using SM2D.Object;
using SM2D.Scene;
using Mouse = SM.Base.Controls.Mouse;

namespace IAmTwo.Menu
{
    public class Button : ItemCollection
    {
        protected DrawObject2D _border;
        protected Polygon _borderMesh;
        private Camera _lastCam;

        public event Action Click;
        public bool React = true;
        public float Width;
        public float Height;

        public Button(string text, float? width = null, Font font = null)
        {
            DrawText drawText = new DrawText(font ?? Fonts.Button, text);
            drawText.GenerateMatrixes();
            Width = drawText.Width;
            Height = drawText.Height;

            float w = width ?? drawText.Width;

            _borderMesh = Models.CreateBackgroundPolygon(new Vector2(w, drawText.Height), 10);

            _border = new DrawObject2D()
            {
                Mesh = _borderMesh,
                ForcedMeshType = PrimitiveType.LineLoop,
                ShaderArguments = { ["ColorScale"] = 1.4f},
            };
            _border.Transform.Size.Set(1.2f);
            _border.Transform.Position.Set(w / 2 - 10, -(drawText.Height - Fonts.Button.Height) / 2);
            
            Add(drawText, _border);
        }

        public override void Update(UpdateContext context)
        {
            base.Update(context);

            if (React && _border.LastDrawingCamera != null && Mouse2D.MouseOver(Mouse2D.InWorld(_border.LastDrawingCamera as Camera), out _, _border))
            {
                _border.Color = Color4.LightBlue;
                if (Mouse.IsDown(MouseButton.Left, true)) TriggerClick();
            }
            else _border.Color = Color4.Blue;
        }

        public void TriggerClick()
        {
            Click?.Invoke();
        }
    }
}