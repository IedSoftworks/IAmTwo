using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using IAmTwo.Game;
using IAmTwo.Resources;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using SM.Base.Controls;
using SM.Base.Objects;
using SM.Base.Windows;
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
        protected DrawObject2D _border;
        protected Polygon _borderMesh;
        private Camera _lastCam;

        public event Action Click;

        public Button(string text, float? width = null)
        {
            DrawText drawText = new DrawText(Fonts.Button, text);
            drawText.GenerateMatrixes();

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

            if (_lastCam != null && Mouse2D.MouseOver(Mouse2D.InWorld(_lastCam), out _, _border))
            {
                _border.Color = Color4.LightBlue;
                if (Mouse.IsDown(MouseButton.Left, true)) TriggerClick();
            }
            else _border.Color = Color4.Blue;
        }

        public override void Draw(DrawContext context)
        {
            _lastCam = context.UseCamera as Camera;

            base.Draw(context);
        }

        public void TriggerClick()
        {
            Click?.Invoke();
        }
    }
}