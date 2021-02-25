using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
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
        private DrawObject2D _border;
        private InstancedMesh _borderMesh;
        private Camera _lastCam;

        public event Action Click;

        public Button(string text, float? start = null, float? width = null)
        {
            DrawText drawText = new DrawText(Fonts.Button, text);
            drawText.GenerateMatrixes();

            float w = width.HasValue ? width.Value : drawText.Width;
            float s = start.HasValue ? start.Value : -Fonts.Button.Positions[text[0]].Width / 2;

            _borderMesh = new InstancedMesh(PrimitiveType.Lines, new string[0]);
            _borderMesh.Vertex.Add(
                new Vector3(s, Fonts.Button.Height / 2, 0),
                new Vector3(s, -Fonts.Button.Height / 2,0),
                new Vector3(s + w, -Fonts.Button.Height / 2, 0),
                new Vector3(s + w, Fonts.Button.Height / 2, 0)
            );
            _borderMesh.LineWidth = 2;

            _border = new DrawObject2D();
            _border.Transform.Size.Set(1.5f);
            _border.Mesh = _borderMesh;
            _border.ShaderArguments["ColorScale"] = 1.4f;
            
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